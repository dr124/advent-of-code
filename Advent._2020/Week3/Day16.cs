using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent.Core_2019_2020;
using Advent.Core_2019_2020.Graphs;

namespace Advent._2020.Week3;

public class Day16 : Day<int, long>
{
    private readonly List<Field> _fields = new();
    private readonly List<int[]> _otherTickets = new();
    private readonly List<int[]> _validTickets = new();
    private int[] _myTicket;

    private readonly Matching _matching = new();

    protected override int ReadData()
    {
        var mode = 0;
        var lines = File.ReadAllLines("Week3/Input16.txt");
        for (var i = 0; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i]))
            {
                mode += 1;
                i += 2;
            }

            switch (mode)
            {
                case 0:
                {
                    var split = lines[i].Split(":", StringSplitOptions.TrimEntries);
                    var name = split[0];
                    var ranges = split[1].Split("or", StringSplitOptions.TrimEntries)
                        .Select(x => x.Split("-", StringSplitOptions.TrimEntries).Select(int.Parse).ToArray())
                        .ToArray();
                    _fields.Add(
                        new Field(name,
                            new Range(ranges[0][0], ranges[0][1]),
                            new Range(ranges[1][0], ranges[1][1])));
                    break;
                }
                case 1:
                    _myTicket = lines[i].Split(",").Select(int.Parse).ToArray();
                    break;
                case 2:
                    _otherTickets.Add(lines[i].Split(",").Select(int.Parse).ToArray());
                    break;
            }
        }

        return -1;
    }

    protected override long TaskA()
    {
        var sum = 0;

        foreach (var ticket in _otherTickets)
            if (IsTicketValid(ticket, out var error))
                _validTickets.Add(ticket);
            else
                sum += error;

        return sum;
    }

    protected override long TaskB()
    {
        var n = _fields.Count;

        //transpose
        var columns = _fields.Select((f, j) => (j, t: _validTickets.Select(x => x[j]).ToArray())).ToArray();

        //create matching columns graph
        var graph = new int[n, n];
        for (var i = 0; i < n; i++)
        {
            var fit = columns.Where(x => x.t.All(y => y.IsInRange(_fields[i].r1) || y.IsInRange(_fields[i].r2)));
            foreach (var f in fit)
                graph[i, f.j] = 1;
        }

        //eliminate matches
        for(int ii = 0; ii < n; ii++)
        for (var i = 0; i < n ; i++)
        {
            var row = graph.GetRowSpan(i % n);
            if (row.ToArray().Sum() == 1)
            {
                var j = row.IndexOf(1);
                for (var k = 0; k < n; k++)
                    graph[k, j] = 0;
                graph[i % n, j] = 1;
            }
        }

        //return values
        return _fields.Select((field, i) => (field, i))
            .Where(x => x.field.name.StartsWith("departure"))
            .Select(x =>
            {
                var nth = graph.GetRowSpan(x.i);
                var ind = nth.IndexOf(1);
                return (long) _myTicket[ind];
            })
            .ProductLong();
    }

    // =============================

    private bool IsTicketValid(int[] ticket, out int error)
    {

        error = ticket
            .Where(field => !_fields.Any(x => field.IsInRange(x.r1) || field.IsInRange(x.r2)))
            .Sum();
        var isNoError = error == 0;
        var isValidLength = ticket.Length == _fields.Count;

        var n = ticket.Length;
        var graph = new int[n, n];
        for (var col = 0; col < n; col++)
        for (var row = 0; row < n; row++)
            graph[row, col] = ticket[col].IsInRange(_fields[row].r1)
                              || ticket[col].IsInRange(_fields[row].r2)
                ? 1
                : 0;

        var matching = _matching.MaxFlow(graph);
        var isMaxMatching = matching == n;

        return isNoError && isValidLength && isMaxMatching;
    }

    private record Field(string name, Range r1, Range r2);
}