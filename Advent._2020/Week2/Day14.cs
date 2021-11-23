using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent.Core_2019_2020;

namespace Advent._2020.Week2;

public class Day14 : Day<Day14.MemoryOperation[], long>
{
    public record MemoryOperation(long address, long value, string mask);

    protected override MemoryOperation[] ReadData()
    {
        var lines = File.ReadAllLines("Week2/input14.txt");

        List<MemoryOperation> ops = new();
        var mask = "";
        foreach (var line in lines)
            if (line.StartsWith("mask"))
            {
                mask = line.Split("=", StringSplitOptions.TrimEntries)[1];
            }
            else
            {
                var split = line.Split("=", StringSplitOptions.TrimEntries);
                var addr = split[0].Remove(@"\D");
                var val = split[1].Remove(@"\D");
                ops.Add(new MemoryOperation(long.Parse(addr), long.Parse(val), mask));
            }

        return ops.ToArray();
    }

    protected override long TaskA()
    {
        var memory = new Dictionary<long, long>();

        foreach (var op in Input)
            memory[op.address] = ApplyMask1(op);

        return memory.Values.Sum();
    }

    protected override long TaskB()
    {
        var memory = new Dictionary<long, long>();

        foreach (var op in Input.SelectMany(GetAllOperations))
            memory[op.address] = op.value;

        return memory.Values.Sum();
    }

    //============================================

    private long ApplyMask1(MemoryOperation op)
    {
        var m1 = Convert.ToInt64(op.mask.Replace("X", "0"), 2);
        var m0 = Convert.ToInt64(op.mask.Replace("X", "1"), 2);

        var val = (op.value | m1) & m0;

        return val;
    }

    private IEnumerable<MemoryOperation> GetAllOperations(MemoryOperation op)
    {
        var val = Convert.ToString(op.address, 2)
            .PadLeft(op.mask.Length, '0')
            .ToCharArray();
            
        for (var i = 0; i < op.mask.Length; i++)
            if (op.mask[i] is '1' or 'X')
                val[i] = op.mask[i];

        foreach (var addr in GenerateAddresses(new string(val)))
            yield return op with { address = addr };
    }

    private IEnumerable<long> GenerateAddresses(string pat)
    {
        Queue<string> toParse = new();
        toParse.Enqueue(pat);
        do
        {
            var p = toParse.Dequeue();
            var x = p.IndexOf('X');
            if (x < 0)
            {
                yield return Convert.ToInt64(p, 2);
                continue;
            }

            var a = p[..x];
            var b = p[(x + 1)..];

            toParse.Enqueue(a + '0' + b);
            toParse.Enqueue(a + '1' + b);
        } while (toParse.Count != 0);
    }
}