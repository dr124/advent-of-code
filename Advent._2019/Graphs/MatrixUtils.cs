﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace Advent.Core_2019_2020.Graphs;

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

    public static T[,] Rotate<T>(this T[,] src) where T : struct
    {
        var M = src.GetLength(0);
        var N = src.GetLength(1);

        var xd = new T[N,M];
        for (int i = 0; i < M; i++)
        for (int j = 0; j < N; j++)
            xd[j, i] = src[M - i - 1, j];

        return xd;
    }

    public static T[,] Transpose<T>(this T[,] src) where T : struct
    {
        var M = src.GetLength(0);
        var N = src.GetLength(1);

        var xd = new T[N, M];
        for (int i = 0; i < N; i++)
        for (int j = 0; j < M; j++)
            xd[i, j] = src[j, i];

        return xd;
    }

    public static T[,] MultiplyElements<T>(T[,] m1, T[,] m2) where T : struct
    {
        var M = m1.GetLength(0);
        var N = m1.GetLength(1);

        var xd = new T[N, M];
        for (int i = 0; i < M; i++)
        for (int j = 0; j < N; j++)
            xd[i, j] = (dynamic) m1[i, j] * (dynamic) m2[i, j];

        return xd;
    }

    public static T[,] FlipUD<T>(this T[,] src) where T : struct
    {
        var M = src.GetLength(0);
        var N = src.GetLength(1);

        var xd = new T[M, N];
        for (int i = 0; i < M; i++)
        for (int j = 0; j < N; j++)
            xd[i, j] = src[M - i - 1, j];

        return xd;
    }

    public static T[,] FlipLR<T>(this T[,] src) where T : struct
    {
        var M = src.GetLength(0);
        var N = src.GetLength(1);

        var xd = new T[M, N];
        for (int i = 0; i < M; i++)
        for (int j = 0; j < N; j++)
            xd[i, j] = src[i, N -j-1];
        return xd;
    }

    public static IEnumerable<T> Flatten<T>(this T[,] src) where T : struct
    {
        var M = src.GetLength(0);
        var N = src.GetLength(1);

        for (int i = 0; i < M; i++)
        for (int j = 0; j < N; j++)
            yield return src[i, j];
    }
}