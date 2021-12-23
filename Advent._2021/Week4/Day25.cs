
using Advent.Core;
using Advent.Core.Extensions;

namespace Advent._2021.Week4;

public class Day25 : IReadInputDay
{
    private X[,] Map;

    public void ReadData() =>
        Map = File.ReadAllLines("Week4/Day25.txt")
            .Select(x => x.Select(c => (X)c).ToArray()).ToArray()
            .ToMatrix();

    public object TaskA()
    {
        var map = Map.Copy();
        var m = map.GetLength(0);
        var n = map.GetLength(1);
        bool @continue;
        int steps = 0;
        do
        {
            steps++;
            @continue = false;
            var copy = map.Copy();
            for (int y = 0; y < m; y++)
            for (int x = 0; x < n; x++)
            {
                var x1 = (x + 1) % n;
                if (copy[y, x] == X.Right && copy[y, x1] == X.Empty)
                {
                    map[y, x] = X.Empty;
                    map[y, x1] = X.Right;
                    @continue = true;
                }
            }

            copy = map.Copy();
            for (int y = 0; y < m; y++)
            for (int x = 0; x < n; x++)
            {
                var y1 = (y + 1) % m;
                if (copy[y, x] == X.Down && copy[y1, x] == X.Empty)
                {
                    map[y, x] = X.Empty;
                    map[y1, x] = X.Down;
                    @continue = true;
                }
            }
        } while (@continue);

        return steps;
    }

    public object TaskB() => null;

    enum X
    {
        Empty = '.',
        Down = 'v',
        Right = '>'
    }
}
