global using Vec3 = Advent._2025.Vec3<int>;
using System.Diagnostics;
using System.Numerics;

namespace Advent._2025;

[DebuggerDisplay("({X},{Y},{Z})")]
public record struct Vec3<T> where T : struct, INumber<T>
{
    public Vec3()
    {
    }

    public Vec3(T X, T Y, T Z)
    {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
    }
    
    public T X;
    public T Y;
    public T Z;

    public static implicit operator Vec3<T>((T x, T y, T z) pair) => new() { X = pair.x, Y = pair.y, Z = pair.z };

    public static Vec3<T> operator *(Vec3<T> a, int b) => new(a.X * T.CreateChecked(b), a.Y * T.CreateChecked(b), a.Z * T.CreateChecked(b));
    public static Vec3<T> operator +(Vec3<T> a, Vec3<T> b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Vec3<T> operator -(Vec3<T> a, Vec3<T> b) => new (a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Vec3<T> operator -(Vec3<T> v) => new (-v.X, -v.Y, -v.Z);

    public override string ToString()
    {
        return $"({X},{Y},{Z})";
    }
}