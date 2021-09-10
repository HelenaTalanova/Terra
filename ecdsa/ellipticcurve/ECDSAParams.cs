using System.Numerics;

namespace TerraJigu.ecdsa.ellipticcurve
{
    /// <summary>
    /// 
    /// </summary>
    public class ECDSAParams
    {
        private const string secp256k1_a = "0000000000000000000000000000000000000000000000000000000000000000";
        private const string secp256k1_b = "0000000000000000000000000000000000000000000000000000000000000007";
        private const string secp256k1_p = "fffffffffffffffffffffffffffffffffffffffffffffffffffffffefffffc2f";
        private const string secp256k1_Gx = "79be667ef9dcbbac55a06295ce870b07029bfcdb2dce28d959f2815b16f81798";
        private const string secp256k1_Gy = "483ada7726a3c4655da4fbfc0e1108a8fd17b448a68554199c47d08ffb10d4b8";
        private const string secp256k1_r = "fffffffffffffffffffffffffffffffebaaedce6af48a03bbfd25e8cd0364141";

        /// <summary>
        /// 
        /// </summary>
        public static CurveFp SECP256k1_CurveEp => new(secp256k1_p, secp256k1_a, secp256k1_b);

        /// <summary>
        /// 
        /// </summary>
        public static Point SECP256k1_Point => new(SECP256k1_CurveEp, secp256k1_Gx, secp256k1_Gy, secp256k1_r);

        public static Point CURVE_GEN => SECP256k1_Point;
        public static BigInteger CURVE_ORDER => CURVE_GEN.Order.Value;
        public static BigInteger FIELD_ORDER => SECP256k1_CurveEp.P;
    }
}
