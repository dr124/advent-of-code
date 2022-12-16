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

        _distances = new int[_input.Length, _input.Length];
        for (int i = 0; i < _input.Length; i++)
        {
            for (int j = 0; j < _input.Length; j++)
            {
                _distances[i, j] = int.MaxValue;
                if (i == j)
                    _distances[i, j] = 0;
            }
        }

        foreach (var valve in _input)
        {
            var i = _valves[valve.Name].Id;
            foreach (var connected in valve.ConnectedTo)
            {
                var j = _valves[connected.Name].Id;
                _distances[i, j] = 1;
            }
        }

        for (int k = 0; k < _input.Length; k++)
        {
            for (int i = 0; i < _input.Length; i++)
            {
                for (int j = 0; j < _input.Length; j++)
                {
                    if (_distances[i, k] != int.MaxValue && _distances[k, j] != int.MaxValue)
                    {
                        _distances[i, j] = Math.Min(_distances[i, j], _distances[i, k] + _distances[k, j]);
                    }
                }
            }
        }
    }

    public object? TaskA()
    {
        var start = _valves["AA"];
        var nop = _input.Where(x => x.FlowRate > 0).Select(x=>x.Id).ToList();
        var acts2 = GenerateActs(new List<Act>(), start, nop, -1);

        return acts2
            .AsParallel()
            .Select(EvalActs)
            .Max();
    }

    public IEnumerable<IEnumerable<Act>> GenerateActs(List<Act> acts, Valve currentValve, List<int> notOpenedYet, int goal)
    {
        //if current is goal, open,
        //enqueue every other not opened yet as goals
        if (goal == currentValve.Id)
        {
            acts.Add(Act.Open());
            // open
        }

        if (notOpenedYet.Count == 0)
        {
            yield return acts;
        }

        // open all not opened
        foreach (var notOpened in notOpenedYet)
        {
            var newNotOpened = notOpenedYet.Where(x => x != notOpened).ToList();
            var newActs = new List<Act>(acts) { Act.Move(notOpened) };
            foreach (var act in GenerateActs(newActs, _input[notOpened], newNotOpened, notOpened))
            {
                yield return act;
            }
        }
    }

    public object? TaskB()
    {
        return null;
    }

    public int MaxPressureLeft(int flowRate, int currentMinute)
    {
        return flowRate * (maxMin - currentMinute);

    }

    const int maxMin = 30;

    public int EvalActs(IEnumerable<Act> acts)
    {
        int sum = 0;
        var v = _valves["AA"];
        int minute = 1;

        foreach (var act in acts)
        {
            if (act is not null)
            {
                if (act.IsOpen) // valve opening
                {
                    //Console.WriteLine($"You open valve {currentValve.Name}");
                    sum += MaxPressureLeft(v.FlowRate, minute);
                    minute++;
                }
                else // movement
                {
                    //Console.WriteLine($"You move to valve {act.MoveTo}");
                    var cost = _distances[v.Id, act.MoveTo];
                    v = _input[act.MoveTo];
                    minute += cost;
                }
            }
            else
            {
                minute++;
            }
            if (minute > maxMin)
                return -1;

        }

        return sum;
    }

    public record Act(bool IsOpen, int MoveTo)
    {
        public static Act Move(int id) => new(false, id);
        public static Act Open() => new(true, -1);

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

        public override string ToString() => $"{Name}:{FlowRate} --> {string.Join(", ", ConnectedToNames)}";
    }
}