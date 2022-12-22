using System.Diagnostics;
using Advent.Core;
using BenchmarkDotNet.Characteristics;

namespace Advent._2022.Week3;

public class Day20 : IReadInputDay
{
    private long[] _input;
    
    public void ReadData()
    {
        _input = File.ReadLines("Week3/Day20.txt")
            .Select(int.Parse)
            //.Select(x=> 811589153L * x)
            .Select(x=> 1L * x)
            .ToArray();

        var x = new Solution();
        var y = x.PartOne(File.ReadAllText("Week3/Day20.txt"));
        var z = x.PartTwo(File.ReadAllText("Week3/Day20.txt"));
    }

    public object? TaskA()
    {
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

        foreach (var node in nodes)
        {
            node.Move(node.Value);
        }

        var node0 = nodes.First(x => x.Value == 0);

        var sum = 0L;
        var x = node0;
        for(int i = 0; i < 1000;i++)
            x = x.Right;
        sum += x.Value;
        Console.WriteLine(x.Value);
        for (int i = 0; i < 1000; i++)
            x = x.Right;
        sum += x.Value;
        Console.WriteLine(x.Value);
        for (int i = 0; i < 1000; i++)
            x = x.Right;
        sum += x.Value;
        Console.WriteLine(x.Value);

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

class Solution
{

    public object PartOne(string input) => GetGrooveCoordinates(1, 1, input);
    public object PartTwo(string input) => GetGrooveCoordinates(1, 811589153L, input);

    long GetGrooveCoordinates(int iterations, long multiplier, string input)
    {
        var nums = input
            .Split("\n")
            .Select(line => multiplier * long.Parse(line))
            .ToArray();

        // permuted array position -> numbers array position
        // e.g. perm[5] means: 
        //    element 5 of the perm array can be found at perm[5] in the nums array
        var perm = Enumerable.Range(0, nums.Length).ToArray();

        for (var i = 0; i < iterations; i++)
        {
            for (var inum = 0; inum < nums.Length; inum++)
            {
                var num = nums[inum];

                if (num < 0)
                {
                    // negative numbers can be thought of as positive numbers
                    // we don't want to implement moving in both directions
                    // so let's convert it to a positive number here 
                    num = -num % (nums.Length - 1);
                    num = (nums.Length - 1) - num;
                }
                else
                {
                    num = num % (nums.Length - 1);
                }

                // position of inum in the permuted array:
                var iperm = Array.IndexOf(perm, inum);

                // should be moved to the right by 'num' steps with wrap around:
                for (var j = 0; j < num; j++)
                {
                    var ipermNext = (iperm + 1) % nums.Length;
                    (perm[ipermNext], perm[iperm]) = (perm[iperm], perm[ipermNext]);
                    iperm = ipermNext;
                }
            }
        }

        nums = perm.Select(inum => nums[inum]).ToArray();
        var idx0 = Array.IndexOf(nums, 0);

        var a = nums[(idx0 + 1000) % nums.Length];
        var b = nums[(idx0 + 2000) % nums.Length];
        var c = nums[(idx0 + 3000) % nums.Length];
        Console.WriteLine($"{a} {b} {c} = {a+b+c}");

        return (
            nums[(idx0 + 1000) % nums.Length] +
            nums[(idx0 + 2000) % nums.Length] +
            nums[(idx0 + 3000) % nums.Length]
        );
    }
}