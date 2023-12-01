using Advent._2023.Week1;
using BenchmarkDotNet.Attributes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Advent._2023.Core;

[SimpleJob(iterationCount:100)]
public class AocBenchmark
{
    private string[] _input = null!;

    [GlobalSetup]
    public void Setup()
    {
        _input = File.ReadAllLines("Week1/Day1.txt");
    }

    [Benchmark]
    public void Part1() => new Day1(_input).Part1();

    [Benchmark]
    public void Part2() => new Day1(_input).Part2();
}