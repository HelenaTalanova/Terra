using System.Numerics;

namespace TerraJigu.ecdsa
{
    /// <summary />
    public class Signature
    {
        /// <summary>
        /// 
        /// </summary>
        public BigInteger R { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BigInteger S { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public Signature(BigInteger r, BigInteger s)
        {
            R = r;
            S = s;
        }
    }
}
