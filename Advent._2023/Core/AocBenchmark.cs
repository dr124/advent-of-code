using Advent._2023.Week1;
using Advent._2023.Week2;
using Advent._2023.Week3;
using BenchmarkDotNet.Attributes;

namespace Advent._2023.Core;

[SimpleJob]
public class AocBenchmark
{
    private string[] _input = null!;

    [GlobalSetup]
    public void Setup()
    {
        _input = File.ReadAllLines("Week3/Day17.txt");
    }

    [Benchmark]
    public void Part()
    {
        var day = new Day17(_input);
        day.Part1();
        day.Part2();
    }
}