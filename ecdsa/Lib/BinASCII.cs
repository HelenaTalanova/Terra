using System.Numerics;
using TerraJigu.Extensions;

namespace TerraJigu.ecdsa.Lib
{
    /// <summary />
    public static class BinASCII
    {
        private const bool isUnsigned = true;
        private const bool isBigEndian = true;

        /// <summary>
        /// 
        /// </summary>
        public static BigInteger HexLify16(byte[] bytes)
        {
            return new BigInteger(bytes, isUnsigned, isBigEndian);
        }

        /// <summary>
        /// 
        /// </summary>
        public static BigInteger HexLify16(string str)
        {
            return new BigInteger(str.ToBytes(), isUnsigned, isBigEndian);
        }
    }
}
