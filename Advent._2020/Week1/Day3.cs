using System.IO;
using System.Linq;
using Advent.Core;

namespace Advent._2020.Week1
{
    public class Day3 : Day<bool[][], int>
    {
        protected override bool[][] ReadData()
        {
            return File.ReadAllLines("Week1/input3.txt")
                .Select(y => y.Select(x => x == '#').ToArray())
                .ToArray();
        }

        protected override int TaskA()
        {
            return TreesInSlope((3, 1));
        }

        protected override int TaskB()
        {
            var slopes = new[]
            {
                (x: 1, y: 1),
                (x: 3, y: 1),
                (x: 5, y: 1),
                (x: 7, y: 1),
                (x: 1, y: 2)
            };

            return slopes.Select(TreesInSlope).Product();
        }

        private int TreesInSlope((int x, int y) slope)
        {
            var trees = 0;
            for (int y = 0, x = 0; y < Input.Length; y += slope.y, x += slope.x)
                if (IsTree(y, x))
                    trees++;
            return trees;
        }

        private bool IsTree(int y, int x)
        {
            return Input[y % Input.Length][x % Input[0].Length];
        }
    }
}