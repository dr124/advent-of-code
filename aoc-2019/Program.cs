using System;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using advent.Week1;

namespace avent
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Stopwatch();
            s.Start();

            Day4.Execute();

            s.Stop();
            Console.WriteLine($"\n\nTime Elapsed: {s.ElapsedMilliseconds}ms");
        }
    }
}
