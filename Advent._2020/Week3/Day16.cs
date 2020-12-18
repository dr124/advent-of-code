﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Advent.Core;
using MoreLinq;

namespace Advent._2020.Week3
{
    public class Day16 : Day<int, int>
    {
        private record Field(string name, Range r1, Range r2);

        private readonly List<Field> _fields = new();
        private int[] _myTicket;
        private readonly List<int[]> _otherTickets = new();
        private readonly List<int[]> _validTickets = new();

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

        protected override int TaskA()
        {
            var sum = 0;
            
            foreach (var ticket in _otherTickets)
                if (IsTicketValid(ticket, out var error))
                    _validTickets.Add(ticket);
                else
                    sum += error;

            return sum;
        }

        protected override int TaskB()
        {
            var columns = _fields.Select((f, j) => (j, t: _validTickets.Select(x => x[j]).ToArray())).ToArray();

            var n = _fields.Count;
            var matchingColumns = new int[n, n];

            
            for (int i = 0; i < n; i++)
            {
                var fit = columns.Where(x => x.t.All(y => y.IsInRange(_fields[i].r1) || y.IsInRange(_fields[i].r2)));
                foreach (var f in fit)
                {
                    matchingColumns[i, f.j] = 1;
                }
            }

            for (int i = 0; i < n * n * 100; i++)
            {
                var ii = i % n;

                var row = matchingColumns.GetRowSpan(ii);
                if (row.ToArray().Sum() == 1)
                {
                    var j = row.IndexOf(1);
                    for (int k = 0; k < n; k++)
                        matchingColumns[k, j] = 0;
                    matchingColumns[ii, j] = 1;
                }
            }
            
            DebugDraw(n, matchingColumns);

            return _fields.Select((field, i) => (field,i))
                    .Where(x => x.field.name.StartsWith("departure"))
                    .Select(x =>
                    {
                        var nth = matchingColumns.GetRowSpan(x.i);
                        var ind = nth.IndexOf(1);
                        return _myTicket[ind];
                    })
                    .Product()
                ;
        }

        private void DebugDraw(int n, int[,] matchingColumns)
        {
            Console.WriteLine("                  nth column: ... ");
            for (int i = 0; i < n; i++)
            {
                Console.Write($"{_fields[i].name}:");
                Console.SetCursorPosition(30, Console.GetCursorPosition().Top);
                Console.WriteLine($"{string.Join(" ", matchingColumns.GetRow(i).Select(x => x == 1 ? 'X' : '_'))}");
            }
        }

        // =============================

        private bool IsTicketValid(int[] ticket, out int error)
        {
            error = ticket
                .Where(field => !_fields.Any(x => field.IsInRange(x.r1) || field.IsInRange(x.r2)))
                .Sum();
            return error == 0 && ticket.Length == _fields.Count;
        }
    }
}