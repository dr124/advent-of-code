using BenchmarkDotNet.Attributes;

namespace Advent._2024.Core;

[SimpleJob]
public class AocBenchmark
{
    private string[] _input = null!;

    [GlobalSetup]
    public void Setup()
    {
        _input = File.ReadAllLines("Week2/Day12.txt");
    }

    // [Benchmark]
    // public void Part()
    // {
    //     var day = new Day12(_input);
    //     day.Part1();
    //     day.Part2();
    // }
}