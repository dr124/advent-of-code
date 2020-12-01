//using System;
//using System.Collections;
//using System.Collections.Generic;

//namespace Advent._2019
//{
//    public class Vec3 : IComparable<Vec3>
//    {
//        #region constructor

//        public Vec3(int x = 0, int y = 0, int z = 0)
//        {
//            X = x;
//            Y = y;
//            Z = z;
//        }

//        public Vec3((int x, int y, int z) p) : this(p.x, p.y, p.z)
//        {
//        }

//        #endregion

//        #region properties

//        public int X;
//        public int Y;
//        public int Z;
//        public double Distance => Math.Pow(X * X + Y * Y + Z * Z, 1.0 / 3);
//        public int Sum => X + Y + Z;
//        public int AbsSum => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
//        public Vec3 Copy => new Vec3(this);
//        #endregion

//        #region methods

//        public Vec3 Scale(int s)
//        {
//            return new Vec3(X * s, Y * s, Z * s);
//        }

//        public Vec3 Scale(Vec3 p)
//        {
//            return new Vec3(X * p.X, Y * p.Y, Z * p.Z);
//        }

//        public Vec3 Scale((int x, int y, int z) p)
//        {
//            return Scale(p.Vec3());
//        }

//        #endregion

//        #region conversions

//        public static implicit operator Vec3((int x, int y, int z) p) => new Vec3(p);
//        public static implicit operator Vec3(int a) => new Vec3(a, a, a);
//        public static implicit operator (int x, int y, int z)(Vec3 p) => (p.X, p.Y, p.Z);

//        #endregion

//        #region operators

//        public static Vec3 operator +(Vec3 a) => a;
//        public static Vec3 operator -(Vec3 a) => new Vec3(-a.X, -a.Y, -a.Z);

//        public static Vec3 operator +(Vec3 a, Vec3 b) => new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
//        public static Vec3 operator -(Vec3 a, Vec3 b) => a + -b;

//        public static Vec3 operator +(Vec3 a, int b) => new Vec3(a.X + b, a.Y + b, a.Z + b);
//        public static Vec3 operator -(Vec3 a, int b) => a + -b;

//        public static Vec3 operator +(Vec3 a, (int x, int y, int z) b) => new Vec3(a.X + b.x, a.Y + b.y, a.Z + b.z);
//        public static Vec3 operator -(Vec3 a, (int x, int y, int z) b) => new Vec3(a.X - b.x, a.Y - b.y, a.Z - b.z);

//        public static bool operator ==(Vec3 a, Vec3 b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
//        public static bool operator !=(Vec3 a, Vec3 b) => a.X != b.X || a.Y != b.Y || a.Z != b.Z;

//        #endregion

//        #region Equality

//        public bool Equals(Vec3 other)
//        {
//            return X == other.X && Y == other.Y && Z == other.Z;
//        }

//        public override bool Equals(object obj)
//        {
//            return obj is Vec3 other && Equals(other);
//        }

//        public override int GetHashCode()
//        {
//            unchecked
//            {
//                var hashCode = X;
//                hashCode = (hashCode * 397) ^ Y;
//                hashCode = (hashCode * 397) ^ Z;
//                return hashCode;
//            }
//        }

//        #endregion

//        public override string ToString() => $"({X},{Y},{Z})";

//        public int CompareTo(Vec3 other)
//        {
//            var xComparison = X.CompareTo(other.X);
//            if (xComparison != 0) return xComparison;

//            var yComparison = Y.CompareTo(other.Y);
//            if (yComparison != 0) return yComparison;

//            return Z.CompareTo(other.Z);
//        }
//    }
//}