global using Advent._2024.Core;
using Advent._2024.Week1;
using Advent._2024.Week2;
using Advent._2024.Week3;
using Advent._2024.Week4;
using BenchmarkDotNet.Running;

namespace Advent._2024;

[TestFixture]
public class Program
{
    private static void Main(string[] args)
    {
        BenchmarkRunner.Run<AocBenchmark>(args: args);
    }

    [Test]
    [AocData("Week1/Day1Example.txt", part1: 11, part2: 31)]
    [AocData("Week1/Day1.txt", part1: 1646452, part2: 23609874)]
    public void Day01Tests(AocData data) => AocTester.Test<Day1>(data);

    [Test]
    [AocData("Week1/Day2Example.txt", part1: 2, part2: 4)]
    [AocData("Week1/Day2.txt", part1: 598, part2: 634)]
    public void Day02Tests(AocData data) => AocTester.Test<Day2>(data);

    [Test]
    [AocData("Week1/Day3Example1.txt", part1: 161)]
    [AocData("Week1/Day3Example2.txt", part2: 48)]
    [AocData("Week1/Day3.txt", part1: 165225049L, part2: 108830766L)]
    public void Day03Tests(AocData data) => AocTester.Test<Day3>(data);

    [Test]
    [AocData("Week1/Day4Example.txt", part1: 18, part2: 9)]
    [AocData("Week1/Day4.txt", part1: 2685, part2: 2048)]
    public void Day04Tests(AocData data) => AocTester.Test<Day4>(data);

    [Test]
    [AocData("Week1/Day5Example.txt", part1: 143, part2: 123)]
    [AocData("Week1/Day5.txt", part1: 6041, part2: 4884)]
    public void Day05Tests(AocData data) => AocTester.Test<Day5>(data);

    [Test]
    [AocData("Week1/Day6Example.txt", part1: 41, part2: 6)]
    [AocData("Week1/Day6.txt", part1: 4967, part2: 1789)]
    public void Day06Tests(AocData data) => AocTester.Test<Day6>(data);

    [Test]
    [AocData("Week1/Day7Example.txt", part1: 3749, part2: 11387)]
    [AocData("Week1/Day7.txt", part1: 1038838357795UL, part2: 254136560217241UL)]
    public void Day07Tests(AocData data) => AocTester.Test<Day7>(data);

    [Test]
    [AocData("Week2/Day8Example.txt", part1: 14, part2: 34)]
    [AocData("Week2/Day8.txt", part1: 318, part2: 1126)]
    public void Day08Tests(AocData data) => AocTester.Test<Day8>(data);

    [Test]
    [AocData("Week2/Day9Example.txt", part1: 1928, part2: 2858)]
    [AocData("Week2/Day9.txt", part1: 6461289671426L, part2: 6488291456470L)]
    public void Day09Tests(AocData data) => AocTester.Test<Day9>(data);

    [Test]
    [AocData("Week2/Day10Example.txt", part1: 36, part2: 81)]
    [AocData("Week2/Day10.txt", part1: 430, part2: 928)]
    public void Day10Tests(AocData data) => AocTester.Test<Day10>(data);

    [Test]
    [AocData("Week2/Day11Example.txt", part1: 55312, part2: null)]
    [AocData("Week2/Day11.txt", part1: 212655, part2: 253582809724830L)]
    public void Day11Tests(AocData data) => AocTester.Test<Day11>(data);

    [Test]
    [AocData("Week2/Day12Example1.txt", part1: 140, part2: 80)]
    [AocData("Week2/Day12Example2.txt", part1: 772, part2: 436)]
    [AocData("Week2/Day12Example3.txt", part1: 1930, part2: 1206)]
    [AocData("Week2/Day12Example4.txt", part1: null, part2: 236)]
    [AocData("Week2/Day12Example5.txt", part1: null, part2: 368)]
    [AocData("Week2/Day12.txt", part1: 1446042, part2: 902742)]
    public void Day12Tests(AocData data) => AocTester.Test<Day12>(data);

    [Test]
    [AocData("Week2/Day13Example.txt", part1: 480, part2: null)]
    [AocData("Week2/Day13.txt", part1: 29517, part2: 103570327981381L)]
    public void Day13Tests(AocData data) => AocTester.Test<Day13>(data);

    [Test]
    [AocData("Week2/Day14Example.txt", part1: 12, part2: null)]
    [AocData("Week2/Day14.txt", part1: 218295000, part2: 6870)]
    public void Day14Tests(AocData data) => AocTester.Test<Day14>(data);

    [Test]
    [AocData("Week3/Day15Example1.txt", part1: 10092, part2: 9021)]
    [AocData("Week3/Day15Example2.txt", part1: 2028, part2: null)]
    [AocData("Week3/Day15.txt", part1: 1436690, part2: 1482350)]
    public void Day15Tests(AocData data) => AocTester.Test<Day15>(data);

    [Test]
    [AocData("Week3/Day16Example.txt", part1: null, part2: null)]
    [AocData("Week3/Day16.txt", part1: null, part2: null)]
    public void Day16Tests(AocData data) => AocTester.Test<Day16>(data);

    [Test]
    [AocData("Week3/Day17Example.txt", part1: null, part2: null)]
    [AocData("Week3/Day17.txt", part1: null, part2: null)]
    public void Day17Tests(AocData data) => AocTester.Test<Day17>(data);

    [Test]
    [AocData("Week3/Day18Example.txt", part1: null, part2: null)]
    [AocData("Week3/Day18.txt", part1: null, part2: null)]
    public void Day18Tests(AocData data) => AocTester.Test<Day18>(data);

    [Test]
    [AocData("Week3/Day19Example.txt", part1: null, part2: null)]
    [AocData("Week3/Day19.txt", part1: null, part2: null)]
    public void Day19Tests(AocData data) => AocTester.Test<Day19>(data);

    [Test]
    [AocData("Week3/Day20Example.txt", part1: null, part2: null)]
    [AocData("Week3/Day20.txt", part1: null, part2: null)]
    public void Day20Tests(AocData data) => AocTester.Test<Day20>(data);

    [Test]
    [AocData("Week3/Day21Example.txt", part1: null, part2: null)]
    [AocData("Week3/Day21.txt", part1: null, part2: null)]
    public void Day21Tests(AocData data) => AocTester.Test<Day21>(data);

    [Test]
    [AocData("Week4/Day22Example.txt", part1: null, part2: null)]
    [AocData("Week4/Day22.txt", part1: null, part2: null)]
    public void Day22Tests(AocData data) => AocTester.Test<Day22>(data);

    [Test]
    [AocData("Week4/Day23Example.txt", part1: null, part2: null)]
    [AocData("Week4/Day23.txt", part1: null, part2: null)]
    public void Day23Tests(AocData data) => AocTester.Test<Day23>(data);

    [Test]
    [AocData("Week4/Day24Example.txt", part1: null, part2: null)]
    [AocData("Week4/Day24.txt", part1: null, part2: null)]
    public void Day24Tests(AocData data) => AocTester.Test<Day24>(data);

    [Test]
    [AocData("Week4/Day25Example.txt", part1: null, part2: null)]
    [AocData("Week4/Day25.txt", part1: null, part2: null)]
    public void Day25Tests(AocData data) => AocTester.Test<Day25>(data);
}