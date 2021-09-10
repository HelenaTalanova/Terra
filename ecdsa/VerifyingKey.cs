using System.Security.Cryptography;
using TerraJigu.ecdsa.ellipticcurve;

namespace TerraJigu.ecdsa
{
    /// <summary />
    public class VerifyingKey
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
        public PublicKey PublicKey { get; set; }

        /// <summary>
        /// Создать из пудличной точки
        /// </summary>
        public static VerifyingKey FromPublicPoint(Point point, Curve curve, HashAlgorithm  hashAlgorithm)
        {
            return new VerifyingKey()
            {
                Curve = curve,
                HashAlgorithm = hashAlgorithm,
                PublicKey = new PublicKey(curve.Generator, point)
                {
                    Order = curve.Order
                }
            };
        }
    }
}
