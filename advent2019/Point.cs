using System;

namespace advent2019
{
    public struct Point
    {
        #region constructor

        public Point(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public Point((int x, int y) p)
        {
            X = p.x;
            Y = p.y;
        }

        #endregion

        #region properties

        public int X { get; set; }
        public int Y { get; set; }
        public double Distance => Math.Sqrt(X * X + Y * Y);

        public double AngleInDegrees
        {
            get
            {
                var value = Math.Atan2(X, Y) / Math.PI * 180f;
                return value < 0 ? value + 365 : value;
            }
        }

        #endregion

        #region methods

        public Point Scale(int s)
        {
            return new Point(X * s, Y * s);
        }

        public Point Scale(Point p)
        {
            return new Point(X * p.X, Y * p.Y);
        }

        public Point Scale((int x, int y) p)
        {
            return Scale(p.Point());
        }

        #endregion

        #region conversions

        public static implicit operator Point((int x, int y) p) => new Point(p);
        public static implicit operator Point(int a) => new Point(a, a);
        public static implicit operator (int x, int y)(Point p) => (p.X, p.Y);

        #endregion

        #region operators

        public static Point operator +(Point a) => a;
        public static Point operator -(Point a) => new Point(-a.X, -a.Y);

        public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);
        public static Point operator -(Point a, Point b) => a + -b;

        public static Point operator +(Point a, int b) => new Point(a.X + b, a.Y + b);
        public static Point operator -(Point a, int b) => a + -b;

        public static Point operator +(Point a, (int x, int y) b) => new Point(a.X + b.x, a.Y + b.y);
        public static Point operator -(Point a, (int x, int y) b) => new Point(a.X - b.x, a.Y - b.y);


        public static bool operator ==(Point a, Point b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Point a, Point b) => a.X != b.X || a.Y != b.Y;

        #endregion

        #region Equality

        public bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is Point other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        #endregion

        public override string ToString() => $"({X},{Y})";
    }
}