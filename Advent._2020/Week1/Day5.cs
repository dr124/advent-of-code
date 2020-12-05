using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Advent.Core;

namespace Advent._2020.Week1
{
    public class Day5 : Day<int[], int>
    {
        protected override int[] ReadData()
        {
            return File.ReadAllLines("Week1/input5.txt")
                .Select(str =>
                {
                    var bin = str
                        .Replace('L', '0').Replace('R', '1')
                        .Replace('F', '0').Replace('B', '1');
                    return Convert.ToInt32(bin, 2);
                })
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
    }
}