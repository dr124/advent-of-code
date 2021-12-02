namespace Advent.Core;

public record Vec3(int X, int Y, int Z)
{
    public static Vec3 operator +(Vec3 v) => new(v.X, v.Y, v.Z);
    public static Vec3 operator -(Vec3 v) => new(v.X, v.Y, v.Z);

    public static Vec3 operator +(Vec3 a, Vec3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Vec3 operator +(Vec3 a, int b) => new(a.X + b, a.Y + b, a.Y + b);

    public static Vec3 operator -(Vec3 a, Vec3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Vec3 operator -(Vec3 a, int b) => new(a.X - b, a.Y - b, a.Z - b);

    public static Vec3 operator *(Vec3 a, Vec3 b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
    public static Vec3 operator *(Vec3 a, int b) => new(a.X * b, a.Y * b, a.Z * b);

    public static Vec3 operator /(Vec3 a, Vec3 b) => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
    public static Vec3 operator /(Vec3 a, int b) => new(a.X / b, a.Y / b, a.Z / b);


    public static implicit operator Vec3((int x, int y, int z) p) => new(p.x, p.y, p.z);

    public static implicit operator Vec3(int a) => new(a, a, a);

    public static implicit operator (int x, int y, int z)(Vec3 p) => (p.X, p.Y, p.Z);
}