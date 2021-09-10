using System.Numerics;

namespace TerraJigu.Extensions
{
    /// <summary />
    public static class BigIntegerExtensions
    {
        /// <summary>
        /// Альтернативная операция получения остатка от деления
        /// operator - "%"
        /// </summary>
        public static BigInteger Mod(this BigInteger a, BigInteger b)
        {
            var correction = a < 0 ? 1 : 0;

            return a - (((a / b) - correction) * b);
        }

        /// <summary>
        /// Альтернативная операция получения остатка от деления
        /// operator - "%"
        /// </summary>
        public static BigInteger Mod(this BigInteger a, BigInteger? b)
        {
            return Mod(a, b ?? BigInteger.Zero);
        }
    }
}
