using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent.Core_2019_2020;
using Node = System.Collections.Generic.LinkedListNode<int>;

namespace Advent._2020.Week4;

internal static class LinkedListExtensions
{
    public static LinkedListNode<T> Next<T>(this LinkedListNode<T> node)
    {
        return node.Next ?? node.List?.First;
    }
}

public class Day23 : Day<List<int>, string>
{
    private int max = int.MaxValue;

    protected override List<int> ReadData()
    {
        return File.ReadAllText("Week4/Input23.txt")
            .Select(x => int.Parse(x.ToString()))
            .ToList();
    }

    protected override string TaskA()
    {
        var gameResult = PlayWithCrab(new LinkedList<int>(Input), new Dictionary<int, Node>(), 100);

        var x = gameResult.Concat(gameResult)
            .Skip(gameResult.ToList().IndexOf(1))
            .Skip(1)
            .Take(gameResult.Count - 1);
        return string.Join("", x);
    }

    protected override string TaskB()
    {
        var values = new Dictionary<int, Node>();
        var list = new LinkedList<int>(Input);
        for (var i = list.Max() + 1; i <= 1_000_000; i++)
            list.AddLast(i);

        _ = PlayWithCrab(list, values, 10_000_000);
        var result = values[1].Next().Value * (long)values[1].Next().Next().Value;
        return result.ToString();
    }

    private LinkedList<int> PlayWithCrab(LinkedList<int> data, Dictionary<int, Node> values, int moves)
    {
        for (var node = data.First; node != null; node = node.Next)
            values.Add(node.Value, node);

        max = data.Max();

        var move = 1;
        for (var current = data.First; move <= moves; move++)
        {
            var next1 = current.Next(); 
            var next2 = next1.Next(); 
            var next3 = next2.Next();

            var destination = current.Value;
                
            do destination = destination > 1 ? destination - 1 : max;
            while (next1.Value == destination 
                   || next2.Value == destination 
                   || next3.Value == destination);

            data.Remove(next1); 
            data.Remove(next2); 
            data.Remove(next3);
                
            var destIdx = values[destination];
                
            data.AddAfter(destIdx, next3); 
            data.AddAfter(destIdx, next2); 
            data.AddAfter(destIdx, next1);
                
            current = current.Next();
        }

        return data;
    }
}