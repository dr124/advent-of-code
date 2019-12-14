using System;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
using advent2019.Week1;
using advent2019.Week2;
using advent2019.Week3;

namespace advent2019
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Stopwatch();
            s.Start();

            Day15.Execute();

            s.Stop();
            Console.WriteLine($"\n\nTime Elapsed: {s.ElapsedMilliseconds}ms");
        }
    }
}
