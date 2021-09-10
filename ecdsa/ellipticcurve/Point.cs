using System.Numerics;
using TerraJigu.ecdsa.Lib;
using TerraJigu.Extensions;

namespace TerraJigu.ecdsa.ellipticcurve
{
    /// <summary />
    public struct Point
    {
        /// <summary>
        /// 
        /// </summary>
        public CurveFp Curve { get; }

        /// <summary>
        /// 
        /// </summary>
        public BigInteger X { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BigInteger Y { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BigInteger? Order { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public Point(CurveFp curve, BigInteger x, BigInteger y, BigInteger? order = null)
        {
            Curve = curve;
            X = x;
            Y = y;
            Order = order;
        }

        /// <summary>
        /// .ctor
        /// </summary>
        public Point(CurveFp curve, string _Gx, string _Gy, string _r)
        {
            Curve = curve;
            X = BinASCII.HexLify16(_Gx);
            Y = BinASCII.HexLify16(_Gy);
            Order = BinASCII.HexLify16(_r);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Point INFINITY => new(default, default, default);

        /// <summary>
        /// 
        /// </summary>
        public Point Double()
        {
            if (this == INFINITY)
                return INFINITY;

            var p = Curve.P;
            var a = Curve.A;
            var x = X;
            var y = Y;

            var l = (3 * x * x + a) * InverseMod(2 * y, p).Mod(p);
            var x3 = (l * l - 2 * x).Mod(p);
            var y3 = (l * (x - x3) - y).Mod(p);
            return new Point(Curve, x3, y3);
        }

        /// <summary>
        /// 
        /// </summary>
        public static BigInteger InverseMod(BigInteger a, BigInteger m)
        {
            if (a < BigInteger.Zero || m <= a)
                a = a.Mod(m);

            var c = a;
            var d = m;
            var uc = BigInteger.One;
            var vc = BigInteger.Zero;
            var ud = BigInteger.Zero;
            var vd = BigInteger.One;

            while (c != BigInteger.Zero)
            {
                var q = d / c;
                var _c = c;
                c = d.Mod(c);
                d = _c;

                var _uc = uc;
                var _vc = vc;
                uc = ud - q * _uc;
                vc = vd - q * _vc;
                ud = _uc;
                vd = _vc;
            }

            return (ud > BigInteger.Zero) ? ud : ud + m;
        }

        #region override operators

        /// <summary>
        /// Умножение
        /// </summary>
        public static Point operator *(Point ths, BigInteger e)
        {
            static BigInteger leftmost_bit(BigInteger x)
            {
                var result = BigInteger.One;
                while (result <= x)
                    result <<= 1;

                return result >> 1;
            };

            if (ths.Order.HasValue)
                e = e.Mod(ths.Order.Value);

            if (e == BigInteger.Zero)
                return INFINITY;
            if (ths == INFINITY)
                return INFINITY;

            var e3 = 3 * e;
            var negative_self = new Point(ths.Curve, ths.X, -ths.Y, ths.Order);
            var i = leftmost_bit(e3) / 2;
            var result = ths;

            while (i > 1)
            {
                result = result.Double();

                if (((e3 & i) != BigInteger.Zero) && ((e & i) == BigInteger.Zero))
                    result += ths;

                if (((e3 & i) == BigInteger.Zero) && ((e & i) != BigInteger.Zero))
                    result += negative_self;

                i /= 2;
            }

            return result;
        }

        /// <summary>
        /// Сложение
        /// </summary>
        public static Point operator +(Point ths, Point other)
        {
            if (other == INFINITY)
                return ths;

            if (ths == INFINITY)
                return other;

            if (ths.X == other.X)
            {
                if (((ths.Y + other.Y).Mod(ths.Curve.P)) == BigInteger.Zero)
                    return INFINITY;
                else
                    return ths.Double();
            }

            var p = ths.Curve.P;
            var l = (other.Y - ths.Y) * InverseMod(other.X - ths.X, p).Mod(p);
            var x3 = (l * l - ths.X - other.X).Mod(p);
            var y3 = (l * (ths.X - x3) - ths.Y).Mod(p);
            return new Point(ths.Curve, x3, y3);
        }

        public static bool operator ==(Point a, Point b) => a.Equals(b);
        public static bool operator !=(Point a, Point b) => a.Equals(b);

        public override bool Equals(object obj)
        {
            if (obj is Point o)
                return o.Curve.A == o.Curve.A
                       && o.Curve.B == o.Curve.B
                       && o.Curve.P == o.Curve.P
                       && o.X == X
                       && o.Y == Y;

            return false;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        #endregion
    }
}
