﻿using System;
using System.Diagnostics;
using advent2019.Week1;
using advent2019.Week2;
using advent2019.Week3;
using advent2019.Week4;

namespace advent2019
{
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
}
