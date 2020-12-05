using System.IO;
using System.Linq;
using Advent.Core;

namespace Advent._2020.Week1
{
    public class Day5 : Day<int[], int>
    {
        protected override int[] ReadData()
        {
            return File.ReadAllLines("Week1/input5.txt")
                .Select(str => 8 * BinarySearch(str[..7], 'F', 'B')
                               + BinarySearch(str[^3..], 'L', 'R'))
                .ToArray();
        }

        protected override int TaskA()
        {
            return Input.Max();
        }

        protected override int TaskB()
        {
            var seats = Input.OrderBy(x => x).ToList();
            for (var i = 1; i < seats.Count; i++)
                if (seats[i] - seats[i - 1] != 1)
                    return seats[i - 1] + 1;

            return -1;
        }

        // ========================================

        private static int BinarySearch(string str, char l, char r)
        {
            int from = 0, to = (1 << str.Length) - 1;
            foreach (var t in str)
            {
                var mid = (to + from) / 2;
                if (t == l) 
                    to = mid;
                else if (t == r) 
                    from = mid + 1;
            }

            return to;
        }
    }
}