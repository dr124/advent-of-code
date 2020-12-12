using System;

namespace Advent.Core
{
    public struct Vec2
    {
        #region constructor

        public Vec2(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public Vec2((int x, int y) p)
        {
            X = p.x;
            Y = p.y;
        }

        #endregion

        #region properties

        public int X { get; set; }
        public int Y { get; set; }
        public double Distance => Math.Sqrt(X * X + Y * Y);
        public int ManhattanDistance => Math.Abs(X) + Math.Abs(Y);
        
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

        public Vec2 Scale(int s)
        {
            return new Vec2(X * s, Y * s);
        }

        public Vec2 Scale(Vec2 p)
        {
            return new Vec2(X * p.X, Y * p.Y);
        }

        public Vec2 Scale((int x, int y) p)
        {
            return Scale(p.Vec2());
        }

        public Vec2 RotateLeft()
        {
            return (-Y, X);
        }

        public Vec2 RotateRight()
        {
            return (Y, -X);
        }
        #endregion

        #region conversions

        public static implicit operator Vec2((int x, int y) p)
        {
            return new Vec2(p);
        }

        public static implicit operator Vec2(int a)
        {
            return new Vec2(a, a);
        }

        public static implicit operator (int x, int y)(Vec2 p)
        {
            return (p.X, p.Y);
        }

        #endregion

        #region operators

        public static Vec2 operator +(Vec2 a)
        {
            return a;
        }

        public static Vec2 operator -(Vec2 a)
        {
            return new Vec2(-a.X, -a.Y);
        }

        public static Vec2 operator +(Vec2 a, Vec2 b)
        {
            return new Vec2(a.X + b.X, a.Y + b.Y);
        }

        public static Vec2 operator -(Vec2 a, Vec2 b)
        {
            return a + -b;
        }

        public static Vec2 operator +(Vec2 a, int b)
        {
            return new Vec2(a.X + b, a.Y + b);
        }

        public static Vec2 operator -(Vec2 a, int b)
        {
            return a + -b;
        }

        public static Vec2 operator +(Vec2 a, (int x, int y) b)
        {
            return new Vec2(a.X + b.x, a.Y + b.y);
        }

        public static Vec2 operator -(Vec2 a, (int x, int y) b)
        {
            return new Vec2(a.X - b.x, a.Y - b.y);
        }


        public static bool operator ==(Vec2 a, Vec2 b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Vec2 a, Vec2 b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        #endregion

        #region Equality

        public bool Equals(Vec2 other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is Vec2 other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (X, Y).GetHashCode();
        }

        #endregion

        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }
}