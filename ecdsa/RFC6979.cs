using System.Numerics;
using System.Security.Cryptography;
using TerraJigu.ecdsa.Lib;
using TerraJigu.Extensions;

namespace TerraJigu.ecdsa
{
    /// <summary />
    public class RFC6979
    {
        /// <summary>
        /// 
        /// </summary>
        public static BigInteger GenerateKey(BigInteger order, BigInteger secexp, HashAlgorithm hashfunc, byte[] data)
        {
            var qlen = order.GetBitLength();
            var holen = hashfunc.HashSize / 8;
            var rolen = (qlen + 7) / 8.0;
            var bx = Utils.NumberToString(secexp, order) + Bits2Octets(data, order);

            var v = Utils.B(0x01, holen);
            var k = Utils.B(0x00, holen);

            k = Hmac.SHA256CompileHash(k, v.Add(Utils.B(0x00, 1)).Add(bx.ToBytes()));

            v = Hmac.SHA256CompileHash(k, v);

            k = Hmac.SHA256CompileHash(k, v.Add(Utils.B(0x01, 1)).Add(bx.ToBytes()));

            v = Hmac.SHA256CompileHash(k, v);

            while (true)
            {
                var t = Utils.B(0, 0);

                //# Step H2
                while (t.Length < rolen)
                {
                    v = Hmac.SHA256CompileHash(k, v);
                    t = t.Add(v);
                }
                //# Step H3
                var secret = Bits2Int(t, qlen);

                if ((secret >= 1) && (secret < order))
                    return secret;

                k = Hmac.SHA256CompileHash(k, v.Add(Utils.B(0x00, 1)));
                v = Hmac.SHA256CompileHash(k, v);
            }
        }

        private static BigInteger Bits2Int(byte[] data, long qlen)
        {
            var x = BinASCII.HexLify16(data);
            var l = data.Length * 8;
            return l > qlen ?
                x >> (int)(l - qlen) :
                x;
        }

        private static string Bits2Octets(byte[] data, BigInteger order)
        {
            var z1 = Bits2Int(data, order.GetBitLength());
            var z2 = z1 - order;

            if (z2 < 0)
                z2 = z1;

            return Utils.NumberToString(z2, order, crop: true);
        }
    }
}
