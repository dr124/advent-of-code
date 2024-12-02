namespace Advent._2024.Week1;

public class Day2(string[] input) : IDay
{
    private readonly int[][] _records = Array.ConvertAll(input,
        record => record.Split(' ')
            .Select(int.Parse)
            .ToArray());

    public object Part1() => _records.Count(IsSafe);

    public object Part2() => _records.Count(record => record
        .Select((_, i) => record.Where((_, j) => j != i).ToArray())
        .Any(IsSafe));

    private static bool IsSafe(int[] arg)
    {
        var lastDiff = arg[1] - arg[0];
        for (var i = 1; i < arg.Length; i++)
        {
            var diff = arg[i] - arg[i - 1];
            
            if (Math.Sign(lastDiff) != Math.Sign(diff))
            {
                return false;
            }

            if (diff is > 3 or < -3 or 0)
            {
                return false;
            }
        }

        return true;
    }
}