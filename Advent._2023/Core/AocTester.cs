using FluentAssertions;

namespace Advent._2023.Core;

public static class AocTester
{
    public static void Test<T>(string[] data, object? part1, object? part2) where T : IDay
    {
        var day = (IDay)Activator.CreateInstance(typeof(T), [data])!;
        TestDay(day, part1, part2);
    }

    private static void TestDay(IDay day, object? resultA, object? resultB)
    {
        var part1 = day.Part1();
        if (resultA is not null)
        {
            Console.WriteLine($"Part 1: {part1}");
            part1.Should().Be(resultA);
        }

        if (resultB is not null)
        {
            var part2 = day.Part2();
            Console.WriteLine($"Part 2: {part2}");
            part2.Should().Be(resultB);
        }
    }
}