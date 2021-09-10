using System.Numerics;
using TerraJigu.ecdsa.Lib;

namespace TerraJigu.ecdsa.ellipticcurve
{
    /// <summary />
    public struct CurveFp
    {
        /// <summary>
        /// 
        /// </summary>
        public BigInteger P { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public BigInteger A { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public BigInteger B { get; init; }

        /// <summary>
        /// .ctor
        /// </summary>
        public CurveFp(string p, string a, string b)
        {
            P = BinASCII.HexLify16(p);
            A = BinASCII.HexLify16(a);
            B = BinASCII.HexLify16(b);
        }
    }
}
