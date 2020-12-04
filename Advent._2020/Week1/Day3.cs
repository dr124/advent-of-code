using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent.Core;

namespace Advent._2020.Week1
{
    public class Day3 : Day<string[], int>
    {
        private readonly (int x, int y)[] Slopes =
        {
            (x: 1, y: 1),
            (x: 3, y: 1),
            (x: 5, y: 1),
            (x: 7, y: 1),
            (x: 1, y: 2)
        };

        protected override string[] ReadData() => File.ReadAllLines("Week1/input3.txt");

        protected override int TaskA() => TreesInSlope((3, 1));
        protected override int TaskB() => Slopes.Select(TreesInSlope).Product();

        // ========================================

        private int TreesInSlope((int x, int y) slope) => EnumerateSlope(slope).Count(IsDżewo);
        private bool IsDżewo((int x, int y) pos) => Input[pos.y % Input.Length][pos.x % Input[0].Length] == '#';

        private IEnumerable<(int x, int y)> EnumerateSlope((int x, int y) slope)
        {
            for (int y = 0, x = 0; y < Input.Length; y += slope.y, x += slope.x)
                yield return (x, y);
        }
    }
}