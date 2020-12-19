using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Core
{
    public static class MatrixUtils
    {
        /// <summary>
        /// Returns string from 2d table [,]
        /// </summary>
        public static string ToMatrixString<T>(this T[,] matrix, string delimiter = "\t")
        {
            var s = new StringBuilder();

            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                    s.Append(matrix[i, j]).Append(delimiter);
                s.AppendLine();
            }

            return s.ToString();
        }

        
        public static Span<T> GetRowSpan<T>([NotNull] this T[,] m, int row)
        {
            if (row < 0 || row >= m.GetLength(0)) 
                throw new ArgumentOutOfRangeException(nameof(row), "The row index isn't valid");
            return MemoryMarshal.CreateSpan(ref m[row, 0], m.GetLength(1));
        }

        public static IEnumerable<T> GetRow<T>(this T[,] matrix, int row)
        {
            var n = matrix.GetLength(1);
            for (int i = 0; i < n; i++)
                yield return matrix[row, i];
        }
        public static IEnumerable<T> GetColumn<T>(this T[,] matrix, int col)
        {
            var n = matrix.GetLength(0);
            for (int i = 0; i < n; i++)
                yield return matrix[i, col];
        }

        public static int[,] BoolToIntMatrix(this bool[,] src)
        {
            var dst = new int[src.GetLength(0), src.GetLength(1)];
            for (int i = 0; i < src.GetLength(0); i++)
            for (int j = 0; j < src.GetLength(1); j++)
                dst[i, j] = src[i, j] ? 1 : 0;
            return dst;
        }
    }
}
