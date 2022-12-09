using Advent._2022.Week1;
using Advent.Core;
using Advent.Core.Extensions;

namespace Advent._2022.Week2;

public class Day8 : IReadInputDay
{
    private int[,] map;
    private int[,] visible;

    public void ReadData()
    {
        map = File.ReadLines("Week2/Day8.txt")
            .Select(y => y.Select(x => x - '0'))
            .To2dArray()
            .ToMatrix();

        visible = new int[map.GetLength(0), map.GetLength(1)];
    }

    public object? TaskA()
    {
        var N = map.GetLength(0);
        var M = map.GetLength(1);

        // set visible borders to 1
        for (int y = 0; y < M; y++)
            for (int x = 0; x < N; x++)
            {
                if (x == 0 || x == N - 1 || y == 0 || y == M - 1)
                {
                    visible[x, y] = 1;
                }
            }

        var maxFromLeft = new int[M];
        for (int x = 0; x < N; x++)
            for (int y = 0; y < M; y++)
            {
                if (map[x, y] > maxFromLeft[y])
                {
                    visible[x, y] = 1;
                    maxFromLeft[y] = map[x, y];
                }
            }

        var maxFromRight = new int[M];
        for (int y = 0; y < M; y++)
            for (int x = N - 1; x >= 0; x--)
            {
                if (map[x, y] > maxFromRight[y])
                {
                    visible[x, y] = 1;
                    maxFromRight[y] = map[x, y];
                }
            }

        var maxFromTop = new int[N];
        for (int x = 0; x < N; x++)
            for (int y = 0; y < M; y++)
            {
                if (map[x, y] > maxFromTop[x])
                {
                    visible[x, y] = 1;
                    maxFromTop[x] = map[x, y];
                }
            }

        var maxFromBottom = new int[N];
        for (int x = 0; x < N; x++)
        {
            for (int y = M - 1; y >= 0; y--)
            {
                if (map[x, y] > maxFromBottom[x])
                {
                    visible[x, y] = 1;
                    maxFromBottom[x] = map[x, y];
                }
            }
        }

        return visible.Enumerate().Select(p => visible[p.i, p.j]).Sum();
    }

    public object? TaskB()
    {
        var N = map.GetLength(0);
        var M = map.GetLength(1);

        var maxpt = 0;
        {
            for (int x = 1; x < N - 1; x++)
            for (int y = 1; y < M - 1; y++)
            {
                var pt = GetLowerPoints(x, y);
                if (pt > maxpt)
                {
                    maxpt = pt;
                }
            }
        }

        {
            int x = 17, y = 61;
            for (int i = x + 1; i < N && map[i, y] < map[x, y]; i++)
            {
                map[i, y] = 0;
            }
        }
        map[17, 61] = -1;
        Console.WriteLine(map.ToMatrixString("").Replace("-1","_"));

        return maxpt;
    }

    // get number of lower points in all directions
    private int GetLowerPoints(int x, int y)
    {
        var N = map.GetLength(0);
        var M = map.GetLength(1);
        var count = new int[4];

        if (M == 100)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Console.Write(map[x + i, y + j]);
                }
                Console.WriteLine();
            }

        }
        //count[0]++;

        var p = map[x, y];
        count[0]++;
        for (int i = x + 1; i < N && map[i, y] < p; i++, count[0]++) ;
        for (int j = y + 1; j < M && map[x, j] < p; j++, count[1]++) ;
        
        for (int i = x - 1; i >= 0 && map[i, y] < p; i--, count[2]++) ;
        for (int j = y - 1; j >= 0 && map[x, j] < p; j--, count[3]++) ;

        return count.Aggregate(1, (a, b) => a * b);
    }
}