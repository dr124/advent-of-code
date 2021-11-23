using System;
using System.Diagnostics;
using Advent._2019.Week4;
namespace Advent._2019;

class Program
{
    static void Main(string[] args)
    {
        var s = new Stopwatch();
        s.Start();

        Day24.Execute();

        s.Stop();
        Console.WriteLine($"\n\nTime Elapsed: {s.ElapsedMilliseconds}ms");
    }
}