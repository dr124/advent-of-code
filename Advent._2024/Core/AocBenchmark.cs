using Advent._2024.Week1;
using BenchmarkDotNet.Attributes;

namespace Advent._2024.Core;

[SimpleJob, MemoryDiagnoser]
public class AocBenchmark
{
    private string[] _input = null!;

    [GlobalSetup]
    public void Setup()
    {
        _input = File.ReadAllLines("Week1/Day3.txt");
    }

     [Benchmark(Baseline = true)]
     public void PartD()
     {
         var day = new Day3(_input);
         day.Part1();
         day.Part2();
    }
}