using System.Numerics;
using System.Runtime.Intrinsics;
using Advent.Core;
using Microsoft.Diagnostics.Tracing.Parsers.Clr;

namespace Advent._2021.Week1;

internal class Day6 : IReadInputDay
{
    public long[] Input { get; set; }

    public void ReadData()
    {
        var times = File.ReadAllText("Week1/Day6.txt")
            .Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(int.Parse);

        Input = new long[9];
        foreach (var x in times)
            Input[x] += 1;
    }

    public object TaskA() => LiveForNDays2(Input, 80);
                                         
    public object TaskB() => LiveForNDays2(Input, 256);

    long LiveForNDays2(long[] fish, int days)
    {
        unchecked
        {
            var nodes = new Node[fish.Length];
            for (var i = 0; i < nodes.Length; i++)
                nodes[i] = new Node
                {
                    V = fish[i],
                };

            for (var i = 0; i < nodes.Length; i++)
            {
                nodes[i].Prev = nodes[(i + 8) % 9];
                nodes[i].Next = nodes[(i + 1) % 9];
            }

            var head = nodes[0];
            for (var day = 0; day < days; day++, head = head.Next)
                head.Prev.Prev.V += head.V;

            var sum = 0l;
            foreach (var t in nodes)
                sum += t.V;
            return sum;
        }
    }

    class Node
    {
        public long V;
        public Node Next;
        public Node Prev;
    }
}