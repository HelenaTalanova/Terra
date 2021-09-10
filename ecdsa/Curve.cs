using System.Collections.Generic;
using System.Numerics;
using TerraJigu.ecdsa.ellipticcurve;

namespace TerraJigu.ecdsa
{
    /// <summary />
    public class Curve
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OpenSslName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CurveFp CurveFp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Point Generator { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BigInteger Order { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int BaseLenght { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int VerifyingKeyLength { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SignatureLength { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<byte> Oid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EncodedOid { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        private Curve(string name, CurveFp curveFp, Point generator, List<byte> oid, string openSslName)
        {
            Name = name;
            CurveFp = curveFp;
            Generator = generator;
            Oid = oid;
            OpenSslName = openSslName;

            Order = Generator.Order ?? default;
            BaseLenght = Utils.OrderLenght(Order);
            VerifyingKeyLength = BaseLenght * 2;
            SignatureLength = BaseLenght * 2;
            EncodedOid = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Curve SECP256k1 => new Curve("SECP256k1", ECDSAParams.SECP256k1_CurveEp, ECDSAParams.SECP256k1_Point, new List<byte>() { 1, 3, 132, 0, 10 }, "secp256k1");
    }
}
