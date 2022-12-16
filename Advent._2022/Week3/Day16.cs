using Advent.Core.Extensions;
using Iced.Intel;

namespace Advent._2022.Week3;

public class Day16 : IReadInputDay
{
    private Valve[] _input;
    private Dictionary<string, Valve> _valves;
    //private Dictionary<string, int> _mapping;
    private int[,] _distances;

    public void ReadData()
    {
        _input = File.ReadAllLines("Week3/Day16.txt")
            .Select((str, i) => Valve.Create(str, i))
            .ToArray();

        _valves = _input.ToDictionary(v => v.Name);

        foreach (var valve in _input)
        {
            valve.ConnectedTo = valve.ConnectedToNames
                .Select(name => _valves[name])
                .ToArray();
        }

        // calculate all distances between valves
        _distances = new int[_input.Length, _input.Length];

        for (var i = 0; i < _input.Length; i++)
        {
            var valve = _input[i];
            var distances = new Dictionary<Valve, int>();
            var visited = new HashSet<Valve>();
            var queue = new Queue<(Valve, int)>();
            queue.Enqueue((valve, 0));

            while (queue.Count > 0)
            {
                var (v, d) = queue.Dequeue();

                if (visited.Contains(v))
                    continue;

                visited.Add(v);
                distances[v] = d;

                foreach (var c in v.ConnectedTo)
                {
                    queue.Enqueue((c, d + 1));
                }
            }

            for (var j = 0; j < _input.Length; j++)
            {
                _distances[i, j] = distances[_input[j]];
            }
        }

    }

    public object? TaskA()
    {
        var start = _valves["AA"];
        var allToVisit = _valves.Values.Where(v => v.FlowRate > 0).ToList();

        var xd = GenerateActs(start, allToVisit, 0, 0)
            .OrderByDescending(x=>x.pressure)
            .ToList();

        return GenerateActs(start, allToVisit, 0, 0).AsParallel().Max(x => x.pressure);
    }
    
    public IEnumerable<(List<string> path, int minute, int pressure)> GenerateActs(Valve currentValve, List<Valve> notOpenedYet, int minute, int pressure)
    {
        if (currentValve.Name == "AA")
        {
            // nothing, starting point
        }
        else // open it!
        {
            minute++;
            pressure += MaxPressureLeft(currentValve, minute);
        }

        if (notOpenedYet.Count == 0 || minute <= 30)
        {
            yield return new (new (), minute, pressure);
        }

        foreach (var valve in notOpenedYet)
        {
            var newNotOpenedYet = notOpenedYet.ToList();
            newNotOpenedYet.Remove(valve);
            
            var newMinute = minute + _distances[currentValve.Id, valve.Id];
            if (newMinute < 30)
            {
                var acts = GenerateActs(valve, newNotOpenedYet, newMinute, pressure);
                foreach (var act in acts)
                {
                    yield return (act.path.Prepend(valve.Name).ToList(), act.minute, act.pressure);
                }
            }
        }
    }

    public object? TaskB()
    {
        return null;
    }

    public int MaxPressureLeft(Valve valve, int currentMinute)
    {
        if (currentMinute > maxMin)
        {
            return -10000;
        }
        return valve.FlowRate * (maxMin - currentMinute);
    }

    const int maxMin = 30;
    
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

        public override string ToString() => $"{Name}:{FlowRate} --> {string.Join(", ", ConnectedToNames)}";
    }
}

// 2247 too low