using System.Diagnostics;
using Advent.Core;
using Advent.Core.Extensions;
using Microsoft.Diagnostics.Tracing.Parsers.Symbol;

namespace Advent._2022.Week3;

public class Day16 : IReadInputDay
{
    private Valve[] _input;
    private Dictionary<string, Valve> _valves;
    
    public void ReadData()
    {
        _input = File.ReadAllLines("Week3/Day16.txt")
            .Select(Valve.Create)
            .ToArray();

        _valves = _input.ToDictionary(v => v.Name);

        foreach (var valve in _input)
        {
            valve.ConnectedTo = valve.ConnectedToNames
                .Select(name => _valves[name])
                .ToArray();
        }
    }

    public object? TaskA()
    {
        var acts = new List<Act>()
        {
            Act.Move("DD"),
            Act.Open(),
            Act.Move("CC"),
            Act.Move("BB"),
            Act.Open(),
            Act.Move("AA"),
            Act.Move("II"),
            Act.Move("JJ"),
            Act.Open(),
            Act.Move("II"),
            
            Act.Move("AA"),
            Act.Move("DD"),
            Act.Move("EE"),
            Act.Move("FF"),
            Act.Move("GG"),
            Act.Move("HH"),
            Act.Open(),
            Act.Move("GG"),
            Act.Move("FF"),
            Act.Move("EE"),
            
            Act.Open(),
            Act.Move("DD"),
            Act.Move("CC"),
            Act.Open(),
            null,
            null,
            null,
            null,
            null,
        };
        return EvalActs(acts);
    }

    public object? TaskB()
    {
        return null;
    }

    public int EvalActs(List<Act> acts)
    {
        int sum = 0;
        int openPress = 0;
        var v = _valves["AA"];
        for (int minute = 1; minute < 30; minute++)
        {
            var act = acts[minute-1];
            if (act is not null)
            {
                if (act.IsOpen) // valve opening
                {
                    Console.WriteLine($"You open valve {v.Name}");
                    openPress += v.FlowRate;
                }
                else // movement
                {
                    Console.WriteLine($"You move to valve {act.MoveTo}");
                    v = _valves[act.MoveTo];
                }
            }

            Console.WriteLine($"releasing {openPress} pressure");
            sum += openPress;
        }

        return sum;
    }

    public record Act(bool IsOpen, string MoveTo)
    {
        public static Act Move(string name) => new(false, name);
        public static Act Open() => new(true, null);
        
    }

    public class Valve
    {
        public string Name { get; init; }
        public int FlowRate { get; set; }
        public string[] ConnectedToNames { get; init; }
        public Valve[] ConnectedTo { get; set; }
        
        public static Valve Create(string line)
        {
            // Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
            var parts = line.Split(new [] {' ', '=', ';', ','}, SplitOptions.Clear);
            var name = parts[1];
            var flowRate = int.Parse(parts[5]);
            var connectedToNames = parts.Skip(10).ToArray();

            return new Valve
            {
                Name = name,
                FlowRate = flowRate,
                ConnectedToNames = connectedToNames
            };
        }

        public override string ToString() => $"{Name}:{FlowRate} --> {string.Join(", ", ConnectedToNames)}";
    }
}