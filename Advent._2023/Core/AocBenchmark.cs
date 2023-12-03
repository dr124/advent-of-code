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
        _input = File.ReadAllLines("Week1/Day3.txt");
    }

    [Benchmark]
    public void Part1() => new Day3(_input).Part1();

    [Benchmark]
    public void Part2() => new Day3(_input).Part2();
}