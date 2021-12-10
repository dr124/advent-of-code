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
}