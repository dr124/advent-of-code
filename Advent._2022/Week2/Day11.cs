using System.Globalization;
using System.Numerics;
using Advent.Core;
using Advent.Core.Extensions;

namespace Advent._2022.Week2;

public class Day11 : IReadInputDay
{
    public class Monkey
    {
        public Queue<long> Queue { get; } = new();
        
        public int ItemsProcessed { get; private set; } = 0;
        private Func<long, long> _operation;
        public int _test;
        public int _throwTrue;
        public int _throwFalse;
        
        public Monkey(string[] str)
        {
            var startingItems = str[1][18..]
                .Split(", ")
                .Select(long.Parse)
                .ToList();
            
            foreach (var t in startingItems)
            {
                Queue.Enqueue(t);
            }

            var op = str[2][18..].Split(' ', SplitOptions.Clear);
            _operation = (op[1], op[2]) switch
            {
                ("*", "old") => (old) => old * old,
                ("+", "old") => (old) => old + old,
                ("*", _) s => (old) => old * int.Parse(s.Item2),
                ("+", _) s => (old) => old + int.Parse(s.Item2)
            };

            _test = int.Parse(str[3][21..]);
            _throwTrue = int.Parse(str[4][29..]);
            _throwFalse = int.Parse(str[5][30..]);
        }

        public void ProcessItems(int div, List<Monkey> queue)
        {
            while (Queue.TryDequeue(out var item))
            {
                ItemsProcessed += 1;
                
                var worry = (_operation(item) / div) % XD;
                var m = worry % _test == 0 
                    ? _throwTrue
                    : _throwFalse;
                queue[m].Queue.Enqueue(worry);
            }
        }
    }

    private static int XD = 0;
    private IEnumerable<Monkey> _monkeFactory;

    public void ReadData()
    {
        _monkeFactory = File.ReadLines("Week2/Day11.txt")
            .Chunk(7)
            .Select(x => new Monkey(x));

        XD = _monkeFactory
            .Select(x => x._test)
            .Aggregate(1, (x, d) => x * d);
    }

    public object? TaskA() => Process(20, 3);

    public object? TaskB() => Process(10000, 1);

    public long Process(int rounds, int div)
    {
        var monkeys = _monkeFactory.ToList();
        for (int round = 1; round <= rounds; round++)
        {
            foreach (var monkey in monkeys)
            {
                monkey.ProcessItems(div, monkeys);
            }
        }

        return monkeys
            .Select(x => x.ItemsProcessed)
            .OrderDescending()
            .Take(2)
            .Aggregate(1L, (product, monkey) => product * monkey);
    }
}