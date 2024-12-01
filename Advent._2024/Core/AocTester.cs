using System.Diagnostics;
using FluentAssertions;
using Humanizer;

namespace Advent._2024.Core;

public record AocData(string[]? Lines, object? Part1, object? Part2, string Path);

public static class AocTester
{
    public static void Test<T>(AocData data) where T : IDay
    {
        if (data.Lines is null)
        {
            Assert.Inconclusive($"Input file is missing. ({data.Path})");
        }

        var day = (IDay)Activator.CreateInstance(typeof(T), [data.Lines])!;

        var stopwatch = Stopwatch.StartNew();

        var resultA = day.Part1();
        Console.WriteLine($"Part 1: {resultA}");
        if (data.Part1 is not null)
        {
            resultA.Should().Be(data.Part1);
        }

        var resultB = day.Part2();
        Console.WriteLine($"Part 2: {resultB}");
        if (data.Part2 is not null)
        {
            resultB.Should().Be(data.Part2);
        }

        stopwatch.Stop();
        Console.WriteLine(stopwatch.Elapsed.Humanize());
    }
}