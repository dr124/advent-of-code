using Advent.Core;
using Advent.Core.Extensions;

namespace Advent._2021.Week2;

internal class Day11 : IReadInputDay
{
    public int[,] Input { get; set; }

    public void ReadData() =>
        Input = File.ReadAllLines("Week2/Day11.txt")
            .Select(line => line.Select(c => c - '0').ToArray())
            .ToArray()
            .ToMatrix();

    public object TaskA()
    {
        var matrix = Input.Copy();
        var M = matrix.GetLength(0);
        var adjacent = new Vec2[] { (1, 1), (1, 0), (1, -1), (0, 1), (0, -1), (-1, 1), (-1, 0), (-1, -1) };
        var flashes = 0;

        for (var e = 0; e < 100; e++)
        {
            foreach (var (i, j) in matrix.Enumerate())
                matrix[i, j] += 1;

            var flash = new int[M, M];
            for (var ee = 0; ee < M+M; ee++)
                foreach (var (i, j) in matrix.Enumerate())
                    if (matrix[i, j] > 9)
                    {
                        matrix[i, j] = 0;
                        flash[i, j] = 1;
                        flashes++;
                        foreach (var (x, y) in adjacent)
                            if (i + x >= 0 && j + y >= 0 && i + x < M && j + y < M)
                                matrix[i + x, j + y]++;
                    }

            foreach (var (i, j) in matrix.Enumerate())
                if (flash[i, j] == 1)
                    matrix[i, j] = 0;
        }

        return flashes;
    }

    public object TaskB()
    {
        var matrix = Input.Copy();
        var M = matrix.GetLength(0);
        var adjacent = new Vec2[] { (1, 1), (1, 0), (1, -1), (0, 1), (0, -1), (-1, 1), (-1, 0), (-1, -1) };

        for (var e = 0; e < 1000; e++)
        {
            foreach (var (i, j) in matrix.Enumerate())
                matrix[i, j] += 1;

            var flash = new int[M, M];
            for (var ee = 0; ee < M+M; ee++)
                foreach (var (i, j) in matrix.Enumerate())
                    if (matrix[i, j] > 9)
                    {
                        matrix[i, j] = 0;
                        flash[i, j] = 1;
                        foreach (var (x, y) in adjacent)
                            if (i + x >= 0 && j + y >= 0 && i + x < M && j + y < M)
                                matrix[i + x, j + y]++;
                    }
          
            foreach (var (i, j) in matrix.Enumerate())
                if (flash[i, j] == 1)
                    matrix[i, j] = 0;

            var isAll = flash.ToMatrixString().Count(x => x == '1');
            if (isAll == 100)
                return e + 1;
        }

        return -1;
    }
}