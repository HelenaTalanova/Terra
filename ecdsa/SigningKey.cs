using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace TerraJigu.ecdsa
{
    /// <summary />
    public class SigningKey
    {
        /// <summary>
        /// 
        /// </summary>
        public Curve Curve { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HashAlgorithm HashAlgorithm { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int BaseLenght { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public VerifyingKey VerifyingKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PrivateKey PrivateKey { get; set; }

        /// <summary>
        /// Создать из строки
        /// </summary>
        public static SigningKey FromString(string str, Curve curve, HashAlgorithm hashAlgorithm)
        {
            if (str.Length / 2 != curve.BaseLenght)
                throw new Exception($"Invalid length of private key, received {str.Length / 2}, expected {curve.BaseLenght}");

            var secexp = Utils.StringToNumber(str);

            return FromSecretExponent(secexp, curve, hashAlgorithm);
        }

        /// <summary>
        /// Создать из секретного показателя
        /// </summary>
        public static SigningKey FromSecretExponent(BigInteger secexp, Curve curve, HashAlgorithm hashAlgorithm)
        {
            if (!(1 <= secexp && secexp < curve.Order))
                throw new Exception($"Invalid value for secexp, expected integer between 1 and {curve.Order}");

            var pubKeyPoint = curve.Generator * secexp;
            var pubKey = new PublicKey(curve.Generator, pubKeyPoint) { Order = curve.Order };
            var verifyingKey = VerifyingKey.FromPublicPoint(pubKeyPoint, curve, hashAlgorithm);
            var privKey = new PrivateKey(pubKey, secexp) { Order = curve.Order };

            return new SigningKey()
            {
                Curve = curve,
                HashAlgorithm = hashAlgorithm,
                BaseLenght = curve.BaseLenght,
                VerifyingKey = verifyingKey,
                PrivateKey = privKey,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public byte[] SignDeterministic(string payload, HashAlgorithm hashAlgorithm, Utils.SigenCode sigencode)
        {
            var data = Encoding.UTF8.GetBytes(payload);
            var hash = hashAlgorithm.ComputeHash(data);

            return SignDigestDeterministic(hash, hashAlgorithm, sigencode);
        }

        /// <summary>
        /// 
        /// </summary>
        public byte[] SignDigestDeterministic(byte[] hash, HashAlgorithm hashAlgorithm, Utils.SigenCode sigencode)
        {
            var secexp = PrivateKey.SecretMultiplier;

            var k = RFC6979.GenerateKey(Curve.Generator.Order ?? default, secexp, hashAlgorithm, hash);

            return SignDigest(hash, sigencode, k);
        }

        /// <summary>
        /// 
        /// </summary>
        public byte[] SignDigest(byte[] digest, Utils.SigenCode sigencode, BigInteger k)
        {
            if (digest.Length > Curve.BaseLenght)
                throw new Exception($"this curve {Curve.Name} is too short for your digest {digest}");

            var number = Utils.StringToNumber(digest);
            var (r, s) = SignNumber(number, k);

            return sigencode.Invoke(r, s, PrivateKey.Order);
        }

        /// <summary>
        /// 
        /// </summary>
        public (BigInteger r, BigInteger s) SignNumber(BigInteger number, BigInteger? k = null)
        {
            var _k = k ?? BigInteger.Zero;

            var sig = PrivateKey.Sign(number, _k);
            return (sig.R, sig.S);
        }
        public override string ToString()
        {
            var secexp = PrivateKey.SecretMultiplier;
            return Utils.NumberToString(secexp, PrivateKey.Order); ;
        }
    }
}
