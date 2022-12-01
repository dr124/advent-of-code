using System.Reflection;
using Advent.Core;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace Advent._2022;

[SimpleJob(RunStrategy.Throughput)]
public class AocRunner
{
    private readonly IDay _day;

    public AocRunner()
    {
        var dayNumber = DateTime.Now.AddHours(-6).Day;
        _day = DayFactory.GetDay(dayNumber, Assembly.GetExecutingAssembly());
        if (_day is IReadInputDay readInputDay)
            readInputDay.ReadData();
    }

    [Benchmark]
    public object? TaskA() => _day.TaskA();

    [Benchmark]
    public object? TaskB() => _day.TaskB();
}