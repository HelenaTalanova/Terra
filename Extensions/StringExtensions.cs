using System;
using System.Collections.Generic;

namespace TerraJigu.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static byte[] ToBytes(this string str)
        {
            var bytes = new byte[str.Length / 2];

            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(str.Substring(i * 2, 2), 16);
            }

            return bytes;
        }

        /// <summary>
        /// 
        /// </summary>
        public static string ToStr(this IEnumerable<byte> bytes)
        {
            var str = "";

            foreach (var b in bytes)
            {
                str += b.ToString("x2");
            }

            return str;
        }
    }
}
