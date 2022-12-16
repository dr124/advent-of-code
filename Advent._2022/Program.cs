global using Advent.Core;
using System.Diagnostics;
using System.Reflection;

//#if RELEASE
using Advent._2022;
using BenchmarkDotNet.Running;
//BenchmarkRunner.Run<AocRunner>();
//return;
//#endif

var dayNumber = DateTime.Now.AddHours(-6).Day;
var day = DayFactory.GetDay(dayNumber, Assembly.GetExecutingAssembly());
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