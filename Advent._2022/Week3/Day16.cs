using System.Globalization;
using Advent.Core.Extensions;
using Microsoft.Diagnostics.Tracing.Parsers.Clr;

namespace Advent._2022.Week3;

public class SomeUtils
{
    // get shortest path between two points in graph
    public static List<T> GetShortestPath<T>(T start, T end, Func<T, IEnumerable<T>> getNeighbors)
    {
        var queue = new Queue<(T node, List<T> path)>();
        queue.Enqueue((start, new List<T> { start }));

        while (queue.Count > 0)
        {
            var (node, path) = queue.Dequeue();

            if (node.Equals(end))
                return path;

            foreach (var neighbor in getNeighbors(node))
            {
                if (path.Contains(neighbor))
                    continue;

                var newPath = path.ToList();
                newPath.Add(neighbor);
                queue.Enqueue((neighbor, newPath));
            }
        }

        return new List<T>();
    }

    public static IEnumerable<IEnumerable<T>> GetVariationsWithoutDuplicates<T>(IList<T> items, int length)
    {
        if (length == 0 || !items.Any()) return new List<List<T>> { new List<T>() };
        return from item in items.Distinct()
               from permutation in GetVariationsWithoutDuplicates(items.Where(i => !EqualityComparer<T>.Default.Equals(i, item)).ToList(), length - 1)
               select Prepend(item, permutation);
    }

    public static IEnumerable<T> Prepend<T>(T first, IEnumerable<T> rest)
    {
        yield return first;
        foreach (var item in rest)
            yield return item;
    }
}

public class Day16 : IReadInputDay
{
    const int maxTime = 30;

    private static Valve[] _input;
    private static Dictionary<string, Valve> _valves;
    private static List<Valve>[,] _paths;
    private static Valve[,] _nextNodes;

    public void ReadData()
    {
        _input = File.ReadAllLines("Week3/Day16.txt")
            .Select(Valve.Create)
            .ToArray();

        _valves = _input.ToDictionary(v => v.Name);

        foreach (var valve in _input)
            valve.ConnectedTo = valve.ConnectedToNames
                .Select(name => _valves[name])
                .ToArray();

        _paths = new List<Valve>[_input.Length, _input.Length];
        for (var i = 0; i < _input.Length; i++)
            for (var j = 0; j < _input.Length; j++)
                _paths[i, j] = SomeUtils.GetShortestPath(_input[i], _input[j], v => v.ConnectedTo);

        _nextNodes = new Valve[_input.Length, _input.Length];
        for (var i = 0; i < _input.Length; i++)
            for (var j = 0; j < _input.Length; j++)
                if (_paths[i, j].Count > 1)
                    _nextNodes[i, j] = _paths[i, j][1];
    }

    public object? TaskA()
    {
        var start = _valves["AA"];
        var allToVisit = _valves.Values.Where(v => v.FlowRate > 0).ToList();
        var a = new Agent
        {
            Current = start,
            Goal = null
        };

        var agents = new List<Agent> { a };

        int pressure = 0;
        for (int i = 0; i < maxTime; i++)
        {
            foreach (var agent in agents)
            {
                pressure += agent.Process(allToVisit);
            }
        }

        return -1;
    }

    public object? TaskB()
    {
        return null;
    }

    public static int MaxPressureLeft(Valve valve, int currentMinute)
    {
        if (currentMinute > maxTime)
        {
            return -10000;
        }
        return valve.FlowRate * (maxTime - currentMinute);
    }


    public class Valve
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public int FlowRate { get; set; }
        public string[] ConnectedToNames { get; init; }
        public Valve[] ConnectedTo { get; set; }

        public static Valve Create(string line, int i)
        {
            // Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
            var parts = line.Split(new[] { ' ', '=', ';', ',' }, SplitOptions.Clear);
            var name = parts[1];
            var flowRate = int.Parse(parts[5]);
            var connectedToNames = parts.Skip(10).ToArray();

            return new Valve
            {
                Id = i,
                Name = name,
                FlowRate = flowRate,
                ConnectedToNames = connectedToNames
            };
        }

        public override string ToString() => $"{Id}:{Name} ({FlowRate}/min) --> {string.Join(", ", ConnectedToNames)}";
    }

    public struct Agent
    {
        private void Log(string s)
        {
            Console.WriteLine(s);
        }
        
        public Valve Goal;
        public Valve Current;
        public int Minute = 0;

        public Agent()
        {
            Goal = null;
            Current = null;
        }

        public int Process(List<Valve> notOpenGoals)
        {
            int pressure = 0;
            Minute++;
            var log = $"t={Minute}";

            if (Current == Goal) // you are in valve, open
            {
                Goal = null;
                var up = MaxPressureLeft(Current, Minute);
                log += $", OPEN {Current.Name} +{up}";
                pressure += up;
            }
            else if(Goal != null)
            {
                Current = _nextNodes[Current.Id, Goal.Id];
                log += $", MOVING TO {Current.Name}";
            }

            if (Goal == null) // find new goal
            {
                Goal = notOpenGoals.First();
                log += $", NEW GOAL {Goal.Name}";
            }

            Log(log);
            return pressure;
        }
    }
}