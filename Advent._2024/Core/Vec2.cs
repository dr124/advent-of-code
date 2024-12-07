using System.Diagnostics;

namespace Advent._2024.Core;

[DebuggerDisplay("({X},{Y})")]
public record struct Vec2
{
    public Vec2()
    {
    }

    public Vec2(int X, int Y)
    {
        this.X = X;
        this.Y = Y;
    }
    
    public int X;
    public int Y;

    public static implicit operator Vec2((int x, int y) pair) => new() { X = pair.x, Y = pair.y };

    public static Vec2 operator *(Vec2 a, int b) => (a.X * b, a.Y * b);
    public static Vec2 operator +(Vec2 a, Vec2 b) => (a.X + b.X, a.Y + b.Y);
    public static Vec2 operator -(Vec2 a, Vec2 b) => (a.X - b.X, a.Y - b.Y);
    public static Vec2 operator -(Vec2 v) => (-v.X, -v.Y);
    
    public static Vec2 Up { get; } = (0, -1);
    public static Vec2 Down { get; } = (0, 1);
    public static Vec2 Left { get; } = (-1, 0);
    public static Vec2 Right { get; } = (1, 0);
    public static Vec2 Zero { get; } = (0, 0);
    public static Vec2 One { get; } = (1, 1);
    
    public static readonly Vec2[] Sides = [Up, Down, Left, Right];
    public static readonly Vec2[] Corners = [Up + Left, Up + Right, Down + Left, Down + Right];
    public static readonly Vec2[] Adjacent = [..Sides, ..Corners];
}

public enum Rotation
{
    Clockwise = 1,
    Flip = 2,
    CounterClockwise = 3,
}

public static class Vec2Extensions
{
    public static Vec2 Rotate(this Vec2 v, Rotation rotation)
    {
        return rotation switch
        {
            Rotation.CounterClockwise => (v.Y, -v.X),
            Rotation.Clockwise => (-v.Y, v.X),
            Rotation.Flip => (-v.X, -v.Y),
            _ => v,
        };
    }

    public static bool IsInBounds(this Vec2 v, string[] matrix)
    {
        return v.Y >= 0 && v.Y < matrix.Length && v.X >= 0 && v.X < matrix[v.Y].Length;
    }

    public static char On(this Vec2 v, string[] matrix)
    {
        return matrix[v.Y][v.X];
    }
}