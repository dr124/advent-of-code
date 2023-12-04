global using Advent._2023.Core;
using Advent._2023.Week1;
using BenchmarkDotNet.Running;
using Xunit;

namespace Advent._2023;

public class Program
{
    private static void Main(string[] args)
    {
        BenchmarkRunner.Run<AocBenchmark>();
    }

    [Theory]
    [AocData("Week1/Example1-1.txt", part1: 142)]
    [AocData("Week1/Example1-2.txt", part2: 281)]
    [AocData("Week1/Day1.txt", part1: 52974, part2: 53340)]
    public void Day1Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day1>(data, part1, part2);

    [Theory]
    [AocData("Week1/Example2.txt", part1: 8, part2: 2286)]
    [AocData("Week1/Day2.txt", part1: 2377, part2: 71220)]
    public void Day2Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day2>(data, part1, part2);

    [Theory]
    [AocData("Week1/Example3.txt", part1: 4361, part2: 467835)]
    [AocData("Week1/Day3.txt", part1: 539590, part2: 80703636)]
    public void Day3Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day3>(data, part1, part2);

    [Theory]
    [AocData("Week1/Example4.txt", part1: 13, part2: 30)]
    [AocData("Week1/Day4.txt", part1: 21959, part2: 5132675)]
    public void Day4Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day4>(data, part1, part2);
}