using Advent.Core;

namespace Advent._2021.Week1
{
    internal class Day1 : IReadInputDay<int[]>,IDay 
    {
        public int[] Input { get; set; }
        public void ReadData()
        {
            Input = File.ReadAllText("Week1/Day1.txt").Select(x => (int)x).ToArray();
        }

        public object TaskA()
        {
            Func<int, int> fib = null;
            fib = (a => a < 2 ? a : fib(a - 1) + fib(a - 2));

            return fib(30);
        }

        public object TaskB()
        {
            Func<int, int> fib = null;
            fib = (a => a < 2 ? a : fib(a - 1) + fib(a - 2));

            return fib(35);
        }
    }
}
