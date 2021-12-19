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

    public Vec3 Abs() => (Math.Abs(X), Math.Abs(Y), Math.Abs(Z));

    public static Vec3 FromString(string str)
    {
        //(x,y) 
        var split = str.Replace("(", "")
            .Replace(")", "")
            .Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var x = int.Parse(split[0]);
        var y = int.Parse(split[1]);
        var z = int.Parse(split[2]);
        return (x, y, z);
    }

    public int ManhattanDistance() => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);

    public override string ToString() => $"({X}, {Y}, {Z})";

    //public Vec3 Rotate(RotateDirection dir) =>
    //    dir switch
    //    {
    //        RotateDirection.Left => (-Y, X),
    //        RotateDirection.Right => (Y, -X)
    //    };
}