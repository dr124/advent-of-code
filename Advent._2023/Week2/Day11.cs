using Galaxy = (int X,int Y);

namespace Advent._2023.Week2;

public class Day11 : IDay
{
    private readonly List<Galaxy> _galaxies = [];
    private readonly List<int> _emptyVertical = [];
    private readonly List<int> _emptyHorizontal = [];

    public Day11(string[] input)
    {
        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
                if (input[y][x] == '#')
                    _galaxies.Add((x, y));
        }

        for (var x = 0; x < input.Length; x++)
            if (input[x].All(c => c == '.'))
                _emptyHorizontal.Add(x);

        for (var y = 0; y < input[0].Length; y++)
            if (input.All(line => line[y] == '.'))
                _emptyVertical.Add(y);
    }

    public object Part1() => Calc(2);

    public object Part2() => Calc(1_000_000);

    private long Calc(long expansion)
    {
        var distances = 0L;
        // parallel for gives ~40% speedup
        Parallel.For(0, _galaxies.Count, i =>
        {
            var g1 = _galaxies[i];
            for (var j = i + 1; j < _galaxies.Count; j++)
            {
                var g2 = _galaxies[j];
                var d = GetDistance(g1, g2, expansion);
                distances += d;
            }
        });

        return distances;
    }

    private long GetDistance(Galaxy a, Galaxy b, long expansion)
    {
        var minX = Math.Min(a.X, b.X);
        var maxX = Math.Max(a.X, b.X);
        var minY = Math.Min(a.Y, b.Y);
        var maxY = Math.Max(a.Y, b.Y);

        // binary search for range gives ~70% speedup
        var verticalStartIndex = _emptyVertical.BinarySearch(minX);
        if (verticalStartIndex < 0) 
            verticalStartIndex = ~verticalStartIndex;

        var verticalEndIndex = _emptyVertical.BinarySearch(maxX);
        if (verticalEndIndex < 0) 
            verticalEndIndex = ~verticalEndIndex;

        var horizontalStartIndex = _emptyHorizontal.BinarySearch(minY);
        if (horizontalStartIndex < 0) 
            horizontalStartIndex = ~horizontalStartIndex;

        var horizontalEndIndex = _emptyHorizontal.BinarySearch(maxY);
        if (horizontalEndIndex < 0)
            horizontalEndIndex = ~horizontalEndIndex;

        var verticalBetween = verticalEndIndex - verticalStartIndex;
        var horizontalBetween = horizontalEndIndex - horizontalStartIndex;

        return maxX - minX
               + maxY - minY
               + verticalBetween * (expansion - 1)
               + horizontalBetween * (expansion - 1);
    }
}