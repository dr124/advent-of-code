using System.Diagnostics;
using Advent.Core;
using BenchmarkDotNet.Characteristics;

namespace Advent._2022.Week3;
// 7592416526315 too high

public class Day20 : IReadInputDay
{
    private static long[] _input;
    
    public void ReadData()
    {
        _input = File.ReadLines("Week3/Day20.txt")
            .Select(int.Parse)
            .Select(x=> 811589153L * x)
            //.Select(x=> 1L * x)
            .ToArray();
    }

    public object? TaskA()
    {
        int times = 10;
        var n = _input.Length;
        var nodes = _input.Select((x,i) => new Node(x,i)).ToArray();
        for (int i = 1; i < n-1; i++)
        {
            nodes[i].Left = nodes[i - 1];
            nodes[i].Right = nodes[i + 1];
        }

        nodes[0].Left = nodes[^1];
        nodes[0].Right = nodes[1];

        nodes[^1].Left = nodes[^2];
        nodes[^1].Right = nodes[0];

        for (int i = 0; i < times; i++)
        {
            foreach (var node in nodes)
            {
                node.Move(node.Value);
            }
        }

        var node0 = nodes.First(x => x.Value == 0);

        var sum = 0L;
        var x = node0;
        for(int i = 0; i < 1000;i++)
            x = x.Right;
        sum += x.Value;
        Console.Write(x.Value + " ");
        for (int i = 0; i < 1000; i++)
            x = x.Right;
        sum += x.Value;
        Console.Write(x.Value + " ");
        for (int i = 0; i < 1000; i++)
            x = x.Right;
        sum += x.Value;
        Console.Write(x.Value + " ");
        Console.WriteLine($"= {sum}");
        
        return sum;
    }

    public object? TaskB()
    {
        return null;
    }

    [DebuggerDisplay("{Value}")]
    private record Node(long Value, int Id)
    {
        public Node Left { get; set; }
        public Node Right { get; set; }

        public void Move(long n)
        {
            var nn = Math.Abs(n);
            nn %= (_input.Length - 1);
            var other = this;
            
            if (n > 0) // to right
            {
                for (int i = 0; i < nn; i++)
                {
                    other = other.Right;
                    if (other == this)
                        other = other.Right;
                }   
                var a = Left;
                var b = this;
                var c = Right;
                var d = other;
                var e = d.Right;

                a.Right = c;
                c.Left = a;

                b.Left = d;
                d.Right = b;

                b.Right = e;
                e.Left = b;
            }
            else if(n < 0) // to left
            {
                for (int i = 0; i < nn; i++)
                {
                    other = other.Left;
                    if (other == this)
                        other = other.Left;
                }
                var e = Right;
                var d = this;
                var c = Left;
                var b = other;
                var a = other.Left;

                c.Right = e;
                e.Left = c;

                b.Left = d;
                d.Right = b;

                a.Right = d;
                d.Left = a;
            }

        }
    }
}