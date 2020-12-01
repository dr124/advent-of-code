using System;
using System.IO;
using System.Linq;
using Advent.Core;

namespace Advent._2020.Week1
{
    public class Day1 : Day<int[], int>
    {
        protected override int[] ReadData()
        {
            return File.ReadAllLines("Week1/input1.txt")
                .Select(int.Parse)
                .ToArray();
        }

        protected override int TaskA()
        {
            foreach (var i in Input)
            foreach (var j in Input)
                if (i + j == 2020)
                    return i * j;
            return -1;
        }

        protected override int TaskB()
        {
            foreach (var i in Input)
            foreach (var j in Input)
            foreach (var k in Input)
                if (i + j + k == 2020)
                    return i * j * k;
            return -1;
        }
    }
}