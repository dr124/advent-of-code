using Advent._2024.Week1;
using Advent._2024.Week2;
using Advent._2024.Week3;
using BenchmarkDotNet.Attributes;

namespace Advent._2024.Core;

[SimpleJob, MemoryDiagnoser]
public class AocBenchmark
{
    private string[] _input = null!;

    [GlobalSetup]
    public void Setup()
    {
        _input = File.ReadAllLines("Week3/Day19.txt");
    }

     [Benchmark(Baseline = true)]
     public void PartD()
     {
         var day = new Day19(_input);
         day.Part1();
         day.Part2();
    }
}