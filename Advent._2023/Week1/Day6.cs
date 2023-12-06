using System.Text.RegularExpressions;
using BenchmarkDotNet.Loggers;

namespace Advent._2023.Week1;

public class Day6(string[] input) : IDay
{
    private readonly long[] _times = ParseRaces(input[0]);
    private readonly long[] _distances = ParseRaces(input[1]);

    public object Part1() => _times.Zip(_distances, Calc).Aggregate(1, (acc, x) => acc * x);

    public object Part2() => Calc(
        long.Parse(string.Join(string.Empty, _times)),
        long.Parse(string.Join(string.Empty, _distances)));

    private static int Calc(long time, long distance)
    {
        var a = -1;
        var b = time;
        var c = -distance;
        var deltaSqrt = Math.Sqrt(b * b - 4 * a * c);
        var t1 = (-b + deltaSqrt) / (2 * a);
        var t2 = (-b - deltaSqrt) / (2 * a);

        var t1i = Math.Floor(t1 + 1);
        var t2i = Math.Ceiling(t2 - 1);
        var res = (t2i - t1i + 1);
        return (int)res;
    }

    private static long[] ParseRaces(string line) => line
        .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
        .Skip(1)
        .Select(long.Parse)
        .ToArray();
}