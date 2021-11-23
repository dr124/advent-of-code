﻿namespace Advent.Core;

public record Vec2(int X, int Y)
{
    public static Vec2 operator +(Vec2 v) => new(v.X, v.Y);
    public static Vec2 operator -(Vec2 v) => new(v.X, v.Y);
    
    public static Vec2 operator+(Vec2 a, Vec2 b) => new(a.X + b.X, a.Y + b.Y);
    public static Vec2 operator+(Vec2 a, int b) => new(a.X + b, a.Y + b);

    public static Vec2 operator-(Vec2 a, Vec2 b) => new(a.X - b.X, a.Y - b.Y);
    public static Vec2 operator-(Vec2 a, int b) => new(a.X - b, a.Y - b);

    public static Vec2 operator*(Vec2 a, Vec2 b) => new(a.X * b.X, a.Y * b.Y);
    public static Vec2 operator*(Vec2 a, int b) => new(a.X * b, a.Y * b);

    public static Vec2 operator/(Vec2 a, Vec2 b) => new(a.X / b.X, a.Y / b.Y);
    public static Vec2 operator/(Vec2 a, int b) => new(a.X / b, a.Y / b);


    public static implicit operator Vec2((int x, int y) p) => new (p.x, p.y);

    public static implicit operator Vec2(int a) => new(a, a);

    public static implicit operator (int x, int y)(Vec2 p) => (p.X, p.Y);
}