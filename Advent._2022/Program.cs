using System.Diagnostics;
using System.Reflection;
using Advent._2022;
using Advent.Core;
using BenchmarkDotNet.Running;

var mode = Environment.GetEnvironmentVariable("mode");

if (mode is "run")
{
    var day = DayFactory.GetDay(24, Assembly.GetExecutingAssembly());
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
}

else if (mode is "benchmark")
{
    BenchmarkRunner.Run<AocRunner>();
}
else throw new InvalidOperationException("chopie..");


public class Start
{
    public int DayToRun => 1;
}