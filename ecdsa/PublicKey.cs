using System.Numerics;
using TerraJigu.ecdsa.ellipticcurve;

namespace TerraJigu.ecdsa
{
    /// <summary />
    public class PublicKey
    {
        /// <summary>
        /// 
        /// </summary>
        public CurveFp Curve { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Point Generator { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Point Point { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BigInteger Order { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public PublicKey(Point generator, Point point)
        {
            Curve = generator.Curve;
            Generator = generator;
            Point = point;
        }
    }
}
