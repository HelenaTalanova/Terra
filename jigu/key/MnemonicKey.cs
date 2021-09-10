using Cryptography.ECDSA;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TerraJigu.ecdsa;
using TerraJigu.ecdsa.ellipticcurve;
using TerraJigu.ecdsa.Lib;
using TerraJigu.Extensions;

namespace TerraJigu.jigu.key
{
    public enum COIN_TYPE
    {
        LUNA = 330,
    }

    /// <summary />
    public class MnemonicKey : Key
    {
        string mnemonic;
        int account;
        int index;
        COIN_TYPE coinType;

        public MnemonicKey(string mnemonic, int account = 0, int index = 0, COIN_TYPE coinType = COIN_TYPE.LUNA)
        {
            this.mnemonic = mnemonic;
            this.account = account;
            this.index = index;
            this.coinType = coinType;
            var seed = new Mnemonic("english").to_seed(mnemonic);
            var root = BIP32Key.fromEntropy(seed);
            var child = derive_child(root, account, index, coinType);
            this.account = account;
            this.index = index;
            private_key = child.PrivateKey();
            public_key = child.PublicKey().ToBytes();
        }

        /// <summary>
        /// 
        /// </summary>
        public override byte[] Sign(string payload)
        {
            using var hashAlgorithm = SHA1.Create();
            var sk = SigningKey.FromString(private_key, Curve.SECP256k1, hashAlgorithm);

            using var hashAlgorithm2 = SHA256.Create();
            return sk.SignDeterministic(payload, hashAlgorithm2, Utils.SigenCodeStringCanonize);
        }

        const long BIP32_HARDEN = 0x80000000L;

        public BIP32Key derive_child(BIP32Key root, int account = 0, int index = 0, COIN_TYPE coinType = COIN_TYPE.LUNA)
        {
            return root
                .ChildKey(44 + BIP32_HARDEN)
                .ChildKey((long)coinType + BIP32_HARDEN)
                .ChildKey(account + BIP32_HARDEN)
                .ChildKey(0)
                .ChildKey(index);
        }

        public string base_address()
        {
            //    using var sha = SHA256.Create();
            //     var   rip = hashlib.new("ripemd160");
            //    var sha.update(public_key);
            //       rip.update(sha.digest())
            //return rip.digest()

            using var sha256 = SHA256.Create();
            return Ripemd160Manager.GetHash(sha256.ComputeHash(public_key)).ToStr();
        }
    }

    public class Mnemonic
    {
        string[] wordlist;

        public Mnemonic(string language)
        {
            var radix = 2048;
            var path = Path.Combine("wordlist", $"{language}.txt");
            if (File.Exists(path))
                wordlist = File.ReadAllLines(path);

            if (wordlist.Length != radix)
                throw new Exception($"Wordlist should contain {radix} words, but it contains {wordlist.Length} words.");
        }

        public byte[] to_seed(string mnemonic, string passphrase = "")
        {
            mnemonic = mnemonic.ToLower();
            passphrase = passphrase.ToLower();

            return new PBKDF2(mnemonic, "mnemonic" + passphrase, iterations: PBKDF2.ROUNDS).read(64);
        }
    }



    public class PBKDF2
    {
        public const int ROUNDS = 2048;

        string passphrase;
        string salt;
        int iterations;
        Func<byte[], byte[], byte[]> prf;
        long blockNum;
        byte[] buf;
        bool closed;

        public PBKDF2(string passphrase, string salt, int iterations = 1000)
        {
            setup(passphrase, salt, iterations, _pseudorandom);
        }

        public PBKDF2 setup(string passphrase, string salt, int iterations, Func<byte[], byte[], byte[]> prf)
        {
            this.passphrase = passphrase;
            this.salt = salt;
            this.iterations = iterations;
            this.prf = prf;
            this.blockNum = 0;
            this.buf = Array.Empty<byte>();
            this.closed = false;
            return this;
        }

        private byte[] _pseudorandom(byte[] key, byte[] msg)
        {
            return Hmac.SHA512CompileHash(key, msg);
        }


        public byte[] read(int bytes)
        {
            //"""Read the specified number of key bytes."""
            if (closed)
                throw new Exception("file-like object is closed");

            var size = buf.Length;
            var blocks = new List<byte>();
            blocks.AddRange(buf);
            var i = blockNum;
            while (size < bytes)
            {
                i += 1;
                if (i > 0xffffffffL || i < 1)
                    //# We could return "" here, but
                    throw new Exception("derived key too long");

                var block = __f(i);
                blocks.AddRange(block);
                size += block.Length;
            }

            buf = blocks.ToArray();
            var retval = buf.Take(bytes).ToArray();
            buf = buf.Skip(bytes).ToArray();
            blockNum = i;
            return retval;
        }

        private byte[] __f(long i)
        {
            var p = Encoding.UTF8.GetBytes(passphrase);
            var s = Encoding.UTF8.GetBytes(salt + pack(i));

            var U = prf(p, s);
            var result = U;
            for (var j = 2; j < 1 + iterations; j++)
            {
                U = prf(p, U);
                result = binxor(result, U);
            }
            return result;
        }

        private string pack(long i)
        {
            return $"{(char)(byte)(i << 24)}{(char)(byte)(i << 16)}{(char)(byte)(i << 8)}{(char)(byte)(i)}";
        }

        private byte[] binxor(byte[] a, byte[] b)
        {
            var result = new byte[a.Length];

            for (var i = 0; i < result.Length; i++)
            {
                result[i] = (byte)(a[i] ^ b[i]);
            }

            return result;
        }


    }

    public class BIP32Key
    {
        public bool pub;
        public SigningKey k;
        public VerifyingKey K;
        public byte[] C;
        public int depth;
        public long index;
        public byte[] parent_fpr;
        public bool testnet;

        public BIP32Key(byte[] secret, byte[] chain, int depth, long index, byte[] fpr, bool pub = false, bool testnet = false)
        {
            this.pub = pub;
            if (pub == false)
            {
                using var hashAlgorithm = SHA1.Create();
                this.k = SigningKey.FromString(secret.ToStr(), Curve.SECP256k1, hashAlgorithm);
                this.K = this.k.VerifyingKey;// get_verifying_key();
            }
            else
            {
                this.k = null;
                //this.K = secret;
            }

            this.C = chain;
            this.depth = depth;
            this.index = index;
            this.parent_fpr = fpr;
            this.testnet = testnet;
        }

        public static BIP32Key fromEntropy(byte[] entropy, bool pub = false, bool testnet = false)
        {
            const int MIN_ENTROPY_LEN = 128;
            //if (entropy is null)
            //entropy = os.urandom(MIN_ENTROPY_LEN / 8) # Python doesn't have os.random()
            if (!(entropy.Length >= MIN_ENTROPY_LEN / 8))
                throw new Exception($"Initial entropy {entropy.Length} must be at least {MIN_ENTROPY_LEN} bits");

            var I = Hmac.SHA512CompileHash(Encoding.UTF8.GetBytes("Bitcoin seed"), entropy);

            var Il = I.Take(32).ToArray();
            var Ir = I.Skip(32).ToArray();

            var key = new BIP32Key(secret: Il, chain: Ir, depth: 0, index: 0, fpr: new byte[] { 0, 0, 0, 0 }, pub: false, testnet: testnet);
            if (pub)
                key = key.SetPublic();

            return key;
        }

        public BIP32Key SetPublic()
        {
            k = null;
            pub = true;
            return this;
        }

        public BIP32Key ChildKey(long i)
        {
            if (pub == false)
                return CKDpriv(i);
            else
                return null;// CKDpub(i);
        }

        private byte[] packB(long i)
        {
            return new byte[] { (byte)(i << 24), (byte)(i << 16), (byte)(i << 8), (byte)(i) };
        }
        private string pack(long i)
        {
            return $"{(char)(byte)(i << 24)}{(char)(byte)(i << 16)}{(char)(byte)(i << 8)}{(char)(byte)(i)}";
        }

        public BIP32Key CKDpriv(long i)
        {
            const long BIP32_HARDEN = 0x80000000L;
            var i_str = (new string('0', 8) + i.ToString("x2"))[^8..];
            var data = string.Empty;

            //# Data to HMAC
            if ((i & BIP32_HARDEN) != 0)
                data = "00" + k.ToString()[^64..] + i_str;
            else
                data = PublicKey() + i_str;

            //# Get HMAC of data
            var (Il, Ir) = hmac(data.ToBytes());

            //# Construct new key material from Il and current private key
            var Il_int = Utils.StringToNumber(Il);
            if (Il_int > ECDSAParams.CURVE_ORDER)
                return null;

            var pvt_int = Utils.StringToNumber(k.ToString());
            var k_int = (Il_int + pvt_int).Mod(ECDSAParams.CURVE_ORDER);
            if (k_int == 0)
                return null;
            var secret = (new string('0', 32) + k_int.ToString("x2"))[^64..];
            
            //        # Construct and return a new BIP32Key
            return new BIP32Key(secret: secret.ToBytes(), chain: Ir, depth: depth + 1, index: i, fpr: Fingerprint(), pub: false, testnet: testnet);
        }

        public string PublicKey()
        {
            //"Return compressed public key encoding"
            var padx = (new string('0', 32) + K.PublicKey.Point.X.ToString("x2"))[^64..];

            if ((K.PublicKey.Point.Y & 1) != 0)
                return "03" + padx;
            else
                return "02" + padx;
        }

        public string PrivateKey()
        {
            // "Return private key as string"
            if (pub)
                throw new Exception("Publicly derived deterministic keys have no private half");
            else
                return k.ToString();
        }

        public (byte[] Il, byte[] Ir) hmac(byte[] data)
        {
            var I = Hmac.SHA512CompileHash(C, data);
            return (I[..32], I[32..]);
        }

        public byte[] Fingerprint()
        {
            return Identifier()[..4];
        }

        public byte[] Identifier()
        {
            var cK = PublicKey();
            using var sha256 = SHA256.Create();
            return Ripemd160Manager.GetHash(sha256.ComputeHash(cK.ToBytes()));
        }
    }
}
