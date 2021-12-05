global using System;
global using System.Collections.Generic;
global using System.IO;
global using System.Linq;
global using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using System.Diagnostics;
using System.Reflection;
using Advent.Core;

#if RELEASE

BenchmarkRunner.Run<AocRunner>();

[SimpleJob(RunStrategy.Throughput)]
public class AocRunner
{
    private readonly IDay _day;

    public AocRunner()
    {
        _day = DayFactory.GetDay(DateTime.Today.Day, Assembly.GetExecutingAssembly());
        if(_day is IReadInputDay readInputDay)
            readInputDay.ReadData();
    }
    
    [Benchmark]
    public object TaskA() => _day.TaskA();
    
    [Benchmark]
    public object TaskB() => _day.TaskB();
}

#else

var day = DayFactory.GetDay(DateTime.Today.Day, Assembly.GetExecutingAssembly());
if (day is IReadInputDay readInputDay) 
    readInputDay.ReadData();

// A 
var timeA = Stopwatch.StartNew();
var resultA = day.TaskA();
timeA.Stop();
Console.WriteLine(resultA is not null
    ? $"A: {resultA}, {timeA.ElapsedMilliseconds} ms"
    : $"A finished in {timeA.ElapsedMilliseconds} ms");

// B
var timeB = Stopwatch.StartNew();
var resultB = day.TaskB();
timeB.Stop();
Console.WriteLine(resultB is not null
    ? $"B: {resultB}, {timeB.ElapsedMilliseconds} ms"
    : $"B finished in {timeB.ElapsedMilliseconds} ms");

// results
Console.WriteLine();
Console.WriteLine("============================");
Console.WriteLine("All finished");
if (resultA is not null) Console.WriteLine($"A: {resultA}, {timeA.ElapsedMilliseconds} ms");
if (resultB is not null) Console.WriteLine($"B: {resultB}, {timeB.ElapsedMilliseconds} ms");
Console.WriteLine($"Total time: {timeA.ElapsedMilliseconds + timeB.ElapsedMilliseconds} ms");

_ = 0;
#endif