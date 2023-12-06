global using Advent._2023.Core;
using Advent._2023.Week1;
using Advent._2023.Week2;
using BenchmarkDotNet.Running;
using Microsoft.Diagnostics.Tracing.Parsers.Clr;
using Xunit;

namespace Advent._2023;

public class Program
{
    private static void Main(string[] args)
    {
        BenchmarkRunner.Run<AocBenchmark>();
    }

    [Theory]
    [AocData("Week1/Day1Example1.txt", part1: 142)]
    [AocData("Week1/Day1Example2.txt", part2: 281)]
    [AocData("Week1/Day1.txt", part1: 52974, part2: 53340)]
    public void Day01Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day1>(data, part1, part2);

    [Theory]
    [AocData("Week1/Day2Example.txt", part1: 8, part2: 2286)]
    [AocData("Week1/Day2.txt", part1: 2377, part2: 71220)]
    public void Day02Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day2>(data, part1, part2);

    [Theory]
    [AocData("Week1/Day3Example.txt", part1: 4361, part2: 467835)]
    [AocData("Week1/Day3.txt", part1: 539590, part2: 80703636)]
    public void Day03Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day3>(data, part1, part2);

    [Theory]
    [AocData("Week1/Day4Example.txt", part1: 13, part2: 30)]
    [AocData("Week1/Day4.txt", part1: 21959, part2: 5132675)]
    public void Day04Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day4>(data, part1, part2);

    [Theory]
    [AocData("Week1/Day5Example.txt", part1: 35, part2: 46)]
    [AocData("Week1/Day5.txt", part1: 84470622, part2: 26714516, Skip = "This brute force takes 5-10 seconds to finish")]
    public void Day05Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day5>(data, part1, part2);

    [Theory]
    [AocData("Week1/Day6Example.txt", part1: 288, part2: 71503)]
    [AocData("Week1/Day6.txt", part1: 252000, part2: 36992486)]
    public void Day06Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day6>(data, part1, part2);

    [Theory]
    [AocData("Week1/Day7Example.txt", part1: null, part2: null)]
    [AocData("Week1/Day7.txt", part1: null, part2: null)]
    public void Day07Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day7>(data, part1, part2);

    [Theory]
    [AocData("Week2/Day8Example.txt", part1: null, part2: null)]
    [AocData("Week2/Day8.txt", part1: null, part2: null)]
    public void Day08Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day8>(data, part1, part2);

    [Theory]
    [AocData("Week2/Day9Example.txt", part1: null, part2: null)]
    [AocData("Week2/Day9.txt", part1: null, part2: null)]
    public void Day09Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day9>(data, part1, part2);

    [Theory]
    [AocData("Week2/Day10Example.txt", part1: null, part2: null)]
    [AocData("Week2/Day10.txt", part1: null, part2: null)]
    public void Day10Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day10>(data, part1, part2);

    [Theory]
    [AocData("Week2/Day11Example.txt", part1: null, part2: null)]
    [AocData("Week2/Day11.txt", part1: null, part2: null)]
    public void Day11Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day11>(data, part1, part2);

    [Theory]
    [AocData("Week2/Day12Example.txt", part1: null, part2: null)]
    [AocData("Week2/Day12.txt", part1: null, part2: null)]
    public void Day12Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day12>(data, part1, part2);

    [Theory]
    [AocData("Week2/Day13Example.txt", part1: null, part2: null)]
    [AocData("Week2/Day13.txt", part1: null, part2: null)]
    public void Day13Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day13>(data, part1, part2);

    [Theory]
    [AocData("Week2/Day14Example.txt", part1: null, part2: null)]
    [AocData("Week2/Day14.txt", part1: null, part2: null)]
    public void Day14Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day14>(data, part1, part2);

    [Theory]
    [AocData("Week3/Day15Example.txt", part1: null, part2: null)]
    [AocData("Week3/Day15.txt", part1: null, part2: null)]
    public void Day15Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day15>(data, part1, part2);

    [Theory]
    [AocData("Week3/Day16Example.txt", part1: null, part2: null)]
    [AocData("Week3/Day16.txt", part1: null, part2: null)]
    public void Day16Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day16>(data, part1, part2);

    [Theory]
    [AocData("Week3/Day17Example.txt", part1: null, part2: null)]
    [AocData("Week3/Day17.txt", part1: null, part2: null)]
    public void Day17Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day17>(data, part1, part2);


    [Theory]
    [AocData("Week3/Day18Example.txt", part1: null, part2: null)]
    [AocData("Week3/Day18.txt", part1: null, part2: null)]
    public void Day18Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day18>(data, part1, part2);

    [Theory]
    [AocData("Week3/Day19Example.txt", part1: null, part2: null)]
    [AocData("Week3/Day19.txt", part1: null, part2: null)]
    public void Day19Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day19>(data, part1, part2);

    [Theory]
    [AocData("Week3/Day20Example.txt", part1: null, part2: null)]
    [AocData("Week3/Day20.txt", part1: null, part2: null)]
    public void Day20Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day20>(data, part1, part2);

    [Theory]
    [AocData("Week3/Day21Example.txt", part1: null, part2: null)]
    [AocData("Week3/Day21.txt", part1: null, part2: null)]
    public void Day21Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day21>(data, part1, part2);

    [Theory]
    [AocData("Week4/Day22Example.txt", part1: null, part2: null)]
    [AocData("Week4/Day22.txt", part1: null, part2: null)]
    public void Day22Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day22>(data, part1, part2);

    [Theory]
    [AocData("Week4/Day23Example.txt", part1: null, part2: null)]
    [AocData("Week4/Day23.txt", part1: null, part2: null)]
    public void Day23Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day23>(data, part1, part2);

    [Theory]
    [AocData("Week4/Day24Example.txt", part1: null, part2: null)]
    [AocData("Week4/Day24.txt", part1: null, part2: null)]
    public void Day24Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day24>(data, part1, part2);

    [Theory]
    [AocData("Week4/Day25Example.txt", part1: null, part2: null)]
    [AocData("Week4/Day25.txt", part1: null, part2: null)]
    public void Day25Tests(string[] data, object? part1, object? part2) => AocTester.Test<Day25>(data, part1, part2);

}