global using Vec2 = Advent._2024.Core.Vec2<int>;
using System.Diagnostics;
using System.Numerics;

namespace Advent._2024.Core;

[DebuggerDisplay("({X},{Y})")]
public record struct Vec2<T> where T : struct, INumber<T>
{
    public Vec2()
    {
    }

    public Vec2(T X, T Y)
    {
        this.X = X;
        this.Y = Y;
    }
    
    public T X;
    public T Y;

    public static implicit operator Vec2<T>((T x, T y) pair) => new() { X = pair.x, Y = pair.y };

    public static Vec2<T> operator *(Vec2<T> a, int b) => new(a.X * T.CreateChecked(b), a.Y * T.CreateChecked(b));
    public static Vec2<T> operator *(Vec2<T> a, T b) => new(a.X * b, a.Y * b);
    public static Vec2<T> operator +(Vec2<T> a, Vec2<T> b) => new(a.X + b.X, a.Y + b.Y);
    public static Vec2<T> operator -(Vec2<T> a, Vec2<T> b) => new (a.X - b.X, a.Y - b.Y);
    public static Vec2<T> operator -(Vec2<T> v) => new (-v.X, -v.Y);

    public static Vec2<T> Up { get; } = new(T.Zero, -T.One);
    public static Vec2<T> Down { get; } = new(T.Zero, T.One);
    public static Vec2<T> Left { get; } = new(-T.One, T.Zero);
    public static Vec2<T> Right { get; } = new(T.One, T.Zero);
    public static Vec2<T> Zero { get; } = new(T.Zero, T.Zero);
    public static Vec2<T> One { get; } = new(T.One, T.One);
    
    public static readonly Vec2<T>[] Sides = [Up, Down, Left, Right];
    public static readonly Vec2<T>[] Corners = [Up + Left, Up + Right, Down + Left, Down + Right];
    public static readonly Vec2<T>[] Adjacent = [..Sides, ..Corners];
}

public enum Rotation
{
    Clockwise = 1,
    Flip = 2,
    CounterClockwise = 3,
}