using System;
using System.Numerics;
using TerraJigu.ecdsa.ellipticcurve;
using TerraJigu.Extensions;

namespace TerraJigu.ecdsa
{
    /// <summary />
    public class PrivateKey
    {
        /// <summary>
        /// 
        /// </summary>
        public PublicKey PublicKey { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public BigInteger SecretMultiplier { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public BigInteger Order { get; init; }

        /// <summary>
        /// .ctor
        /// </summary>
        public PrivateKey(PublicKey publicKey, BigInteger secretMultiplier)
        {
            PublicKey = publicKey;
            SecretMultiplier = secretMultiplier;
        }

        /// <summary>
        /// 
        /// </summary>
        public Signature Sign(BigInteger hash, BigInteger randomKey)
        {
            var g = PublicKey.Generator;
            var n = g.Order ?? default;
            var k = randomKey.Mod(n);
            var p1 = g * k;
            var r = p1.X;

            if (r == 0)
                throw new Exception("amazingly unlucky random number r");

            var s = (Point.InverseMod(k, n) * (hash + (SecretMultiplier * r).Mod(n))).Mod(n);
            if (s == 0)
                throw new Exception("amazingly unlucky random number s");

            return new Signature(r, s);
        }
    }
}
