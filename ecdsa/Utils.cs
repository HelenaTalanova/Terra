using System.Linq;
using System.Numerics;
using TerraJigu.ecdsa.Lib;
using TerraJigu.Extensions;

namespace TerraJigu.ecdsa
{
    /// <summary />
    public static class Utils
    {
        /// <summary>
        /// 
        /// </summary>
        public delegate byte[] SigenCode(BigInteger r, BigInteger s, BigInteger order);

        /// <summary>
        /// 
        /// </summary>
        public static int OrderLenght(BigInteger? order)
        {
            return order?.ToString("x2").Length / 2 ?? 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public static string NumberToString(BigInteger num, BigInteger order, bool crop = false)
        {
            var l = OrderLenght(order) * 2;
            var str = num.ToString("x2");

            return
                str.Length < l ?
                new string('0', l - str.Length) + str :
                crop ? str.Substring(str.Length - l, l) :
                str.Substring(str.Length - l, l);
        }

        /// <summary>
        /// 
        /// </summary>
        public static BigInteger StringToNumber(byte[] bytes)
        {
            return BinASCII.HexLify16(bytes);
        }

        /// <summary>
        /// 
        /// </summary>
        public static BigInteger StringToNumber(string str)
        {
            return BinASCII.HexLify16(str);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string SigenCodeString(BigInteger r, BigInteger s, BigInteger order)
        {
            var (r_str, s_str) = SigenCodeStrings(r, s, order);
            return r_str + s_str;
        }

        /// <summary>
        /// 
        /// </summary>
        public static (string r_str, string s_str) SigenCodeStrings(BigInteger r, BigInteger s, BigInteger order)
        {
            var r_str = NumberToString(r, order);
            var s_str = NumberToString(s, order);
            return (r_str, s_str);
        }

        /// <summary>
        /// 
        /// </summary>
        public static byte[] SigenCodeStringCanonize(BigInteger r, BigInteger s, BigInteger order)
        {
            if (s > order / 2)
                s = order - s;

            return SigenCodeString(r, s, order).ToBytes();
        }

        /// <summary>
        /// 
        /// </summary>
        public static byte[] B(byte value, int count) => Enumerable.Range(0, count).Select(x => value).ToArray();

        /// <summary>
        /// 
        /// </summary>
        public static byte[] Add(this byte[] a, byte[] b)
        {
            return a.Concat(b).ToArray();
        }
    }
}
