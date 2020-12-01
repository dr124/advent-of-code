using System;
using System.IO;
using System.Linq;

namespace Advent._2020.Week1
{
    public class Day1
    {
        public static void Execute()
        {
            var input = File.ReadAllLines("Week1/input1.txt")
                .Select(int.Parse)
                .ToArray();

            var taskA = TaskA(input);
            var taskB = TaskB(input);
            Console.WriteLine($"A: {taskA}, B: {taskB}");
        }

        public static int TaskA(int[] input)
        {
            foreach (var i in input)
            foreach (var j in input)
                if (i + j == 2020)
                    return i * j;
            return -1;
        }

        public static int TaskB(int[] input)
        {
            foreach (var i in input)
            foreach (var j in input)
            foreach (var k in input)
                if (i + j + k == 2020)
                    return i * j * k;
            return -1;
        }
    }
}