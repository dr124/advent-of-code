using System.Numerics;

namespace Advent._2024.Core;

public static class Extensions
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

    public static IEnumerable<Vec2> ReadInput(string[] input, int character) =>
        from vec in EnumerateInput(input)
        where vec.On(input) == character
        select vec;
    
    public static IEnumerable<Vec2> ReadInput(string[] input, Predicate<char> character) =>
        from vec in EnumerateInput(input)
        where character(vec.On(input))
        select vec;

    public static IEnumerable<Vec2> EnumerateInput(string[] input) =>
        from y in Enumerable.Range(0, input.Length)
        from x in Enumerable.Range(0, input[0].Length)
        select new Vec2(x, y);
    
    /// <summary>
    /// This method concats two numbers with basic math.
    /// It's benchmarked about 5x faster than .ToString() + long.Parse().
    /// And about 2x faster than Math.Log + Math.Pow with casting
    /// </summary>
    public static T Concat<T>(T value1, T value2) where T : INumber<T>
    {
        var multiplier = T.One;
        var temp = value2;
        var t10 = T.CreateChecked(10);
        while (temp > T.Zero)
        {
            multiplier *= t10;
            temp /= t10;
        }

        return value1 * multiplier + value2;
    }
}