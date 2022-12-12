using System.Runtime.InteropServices;
using System.Text;

namespace Advent.Core.Extensions;

public static class MatrixUtils
{
    public static T[][] Transpose<T>(this T[][] tab)
    {
        var transposed = new T[tab[0].Length][];
        for (var j = 0; j < tab[0].Length; j++)
            transposed[j] = new T[tab.Length];

        for (var i = 0; i < tab.Length; i++)
            for (var j = 0; j < tab[i].Length; j++)
                transposed[j][i] = tab[i][j];
        return transposed;
    }

    public static string ToMatrixString<T>(this T[,] matrix, string delimiter = "\t", string format = null)
    {
        var s = new StringBuilder();
        for (var i = 0; i < matrix.GetLength(0); i++, s.AppendLine())
            for (var j = 0; j < matrix.GetLength(1); j++)
                s.Append(matrix[i, j]).Append(delimiter);
        return s.ToString();
    }

    public static T[,] ToMatrix<T>(this T[][] tab)
    {
        var matrix = new T[tab.Length, tab[0].Length];
        for (var i = 0; i < tab.Length; i++)
            for (var j = 0; j < tab[0].Length; j++)
                matrix[i, j] = tab[i][j];
        return matrix;
    }

    public static T[,] Copy<T>(this T[,] src)
    {
        var M = src.GetLength(0);
        var N = src.GetLength(1);

        var matrix = new T[M, N];
        for (var i = 0; i < M; i++)
            for (var j = 0; j < N; j++)
                matrix[i, j] = src[i, j];
        return matrix;
    }

    public static IEnumerable<(int i, int j)> Enumerate<T>(this T[,] src)
    {
        var M = src.GetLength(0);
        var N = src.GetLength(1);
        for (var i = 0; i < M; i++)
            for (var j = 0; j < N; j++)
                yield return (i, j);
    }

    public static IEnumerable<Vec2> EnumerateVec<T>(this T[,] src)
    {
        var M = src.GetLength(0);
        var N = src.GetLength(1);
        for (var y = 0; y < M; y++)
            for (var x = 0; x < N; x++)
                yield return (x, y);
    }

    public static Span<T> GetRowSpan<T>(this T[,] m, int row)
    {
        if (row < 0 || row >= m.GetLength(0))
            throw new ArgumentOutOfRangeException(nameof(row), "The row index isn't valid");
        return MemoryMarshal.CreateSpan(ref m[row, 0], m.GetLength(1));
    }

    public static ref T At<T>(this T[,] src, Vec2 v) => ref src[v.Y, v.X];
    public static ref T At<T>(this T[,] src, int x, int y) => ref src[y, x];

    public static IEnumerable<Vec2> Adjacent<T>(this T[,] map, Vec2 point, bool includeDiagonal = false)
    {
        var (x, y) = point;
        var (M, N) = (map.GetLength(0), map.GetLength(1));
        for (var i = -1; i <= 1; i++)
        for (var j = -1; j <= 1; j++)
        {
            if (i == 0 && j == 0)
            {
                continue;
            }
            
            if (!includeDiagonal && Math.Abs(i) + Math.Abs(j) > 1)
            {
                continue;
            }
            
            var n = point + (i, j);
            if (n.X < 0 || n.X >= N || n.Y < 0 || n.Y >= M)
            {
                continue;
            }

            yield return n;
        }
    }
}