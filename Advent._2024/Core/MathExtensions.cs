using System.Numerics;

namespace Advent._2024.Core;

public static class MathExtensions
{
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