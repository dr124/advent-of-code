using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent.Core_2019_2020;

namespace Advent._2020.Week4;

public class Day22 : Day<(int[] p1, int[] p2), int>
{
    protected override (int[] p1, int[] p2) ReadData()
    {
        var players = File.ReadAllText("Week4/Input22.txt")
            .Split(Environment.NewLine + Environment.NewLine)
            .Select(p => p.Replace(Environment.NewLine, " ").SplitClear(" ")[2..])
            .ToList();

        var p1 = players[0].Select(int.Parse).ToArray();;
        var p2 = players[1].Select(int.Parse).ToArray();;

        return (p1, p2);
    }

    protected override int TaskA()
    {
        var p1 = new Queue<int>(Input.p1);
        var p2 = new Queue<int>(Input.p2);
        _ = Play(p1, p2, false);

        return p1.Concat(p2)
            .Reverse()
            .Select((x, i) => x * (i + 1))
            .Sum();
    }

    protected override int TaskB()
    {
        var p1 = new Queue<int>(Input.p1);
        var p2 = new Queue<int>(Input.p2);
        _ = Play(p1, p2, true);

        return p1.Concat(p2)
            .Reverse()
            .Select((x, i) => x * (i + 1))
            .Sum();
    }

    private int Play(Queue<int> p1, Queue<int> p2, bool isRecurvise)
    {
        HashSet<string> played = new();
        while (p1.Count * p2.Count != 0)
        {
            var str = Serialize(p1, p2);
            if (played.Contains(str))
                return 1;
            played.Add(str);

            var p1c = p1.Dequeue();
            var p2c = p2.Dequeue();

            var winner = p1c > p2c ? 1 : 2;
            if (isRecurvise && p1.Count >= p1c && p2.Count >= p2c)
                winner = Play(
                    new Queue<int>(p1.Take(p1c)), 
                    new Queue<int>(p2.Take(p2c)),
                    true);

            if (winner == 1)
            {
                p1.Enqueue(p1c);
                p1.Enqueue(p2c);
            }
            else if (winner == 2)
            {
                p2.Enqueue(p2c);
                p2.Enqueue(p1c);
            }
        }

        return p2.Count == 0 ? 1 : 2; 
    }
        
    private string Serialize(IEnumerable<int> p1, IEnumerable<int> p2) =>
        $"{string.Join(",", p1)}_{string.Join(",",p2)}";
}