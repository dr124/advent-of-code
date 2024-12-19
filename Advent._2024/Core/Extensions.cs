using System.Numerics;
using System.Text;

namespace Advent._2024.Core;

public static class Extensions
{
    public static Vec2<T> Rotate<T>(this Vec2<T> v, Rotation rotation) 
        where T : struct, INumber<T>
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
        return v is { Y: >= 0, X: >= 0 } 
               && v.Y < matrix.Length
               && v.X < matrix[v.Y].Length;
    }
    
    public static bool IsInBounds<T>(this Vec2 v, T[][] matrix)
    {
        return v is { Y: >= 0, X: >= 0 } 
               && v.Y < matrix.Length
               && v.X < matrix[v.Y].Length;
    }
    
    public static bool IsInBounds<T>(this Vec2 v, T[,] matrix)
    {
        return v is { Y: >= 0, X: >= 0 }
               && v.Y < matrix.GetLength(1)
               && v.X < matrix.GetLength(1);
    }

    public static char On(this Vec2 v, string[] matrix)
    {
        return matrix[v.Y][v.X];
    }
    
    public static ref T On<T>(this Vec2 v, T[][] matrix)
    {
        return ref matrix[v.Y][v.X];
    }
    
    public static ref T On<T>(this Vec2 v, T[,] matrix)
    {
        return ref matrix[v.Y, v.X];
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
        from x in Enumerable.Range(0, input[y].Length)
        select new Vec2(x, y);

    public static Vec2<T>[] Sides<T>(this Vec2<T> p) where T : struct, INumber<T> =>
    [
        p + Vec2<T>.Up,
        p + Vec2<T>.Down, 
        p + Vec2<T>.Left,
        p + Vec2<T>.Right
    ];

    public static Vec2<T>[] Corners<T>(this Vec2<T> p) where T : struct, INumber<T> =>
    [
        p + Vec2<T>.Up + Vec2<T>.Left,
        p + Vec2<T>.Up + Vec2<T>.Right,
        p + Vec2<T>.Down + Vec2<T>.Left,
        p + Vec2<T>.Down + Vec2<T>.Right
    ];

    public static Vec2<T>[] Adjacent<T>(this Vec2<T> p) where T : struct, INumber<T> =>
    [
        ..p.Sides(), 
        ..p.Corners()
    ];
    
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
    
    public static string ConvertArray<T>(T[,] array, Func<T, string> elementToString)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < array.GetLength(0); i++) // Iterate rows
        {
            for (int j = 0; j < array.GetLength(1); j++) // Iterate columns
            {
                sb.Append(elementToString(array[i, j]));
                if (j < array.GetLength(1) - 1)
                    sb.Append(" "); // Add space between elements
            }
            sb.AppendLine(); // New line at the end of each row
        }
        return sb.ToString();
    }

    public static string ConvertArray<T>(T[][] array, Func<T, string> elementToString)
    {
        var sb = new StringBuilder();
        foreach (var row in array)
        {
            for (int j = 0; j < row.Length; j++)
            {
                sb.Append(elementToString(row[j]));
                if (j < row.Length - 1)
                    sb.Append(" "); // Add space between elements
            }
            sb.AppendLine(); // New line at the end of each row
        }
        return sb.ToString();
    }
}