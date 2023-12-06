using Advent._2023.Week1;
using BenchmarkDotNet.Attributes;

namespace Advent._2023.Core;

[SimpleJob]
public class AocBenchmark
{
    private string[] _input = null!;

    [GlobalSetup]
    public void Setup()
    {
        _input = File.ReadAllLines("Week1/Day6.txt");
    }

    [Benchmark]
    public void Part()
    {
        var day = new Day6(_input);
        day.Part1();
        day.Part2();
    }
}