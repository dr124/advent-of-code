using System.Diagnostics;
using Advent.Core;

namespace Advent._2022.Week4;

public class Day23 : IReadInputDay
{
    private static readonly Vec2 _N = (0, -1);
    private static readonly Vec2 _S = (0, 1);
    private static readonly Vec2 _E = (1, 0);
    private static readonly Vec2 _W = (-1, 0);
    
    private HashSet<Vec2> _map;
    private List<Entity> _elves;

    public void ReadData()
    {
        _elves = File.ReadAllLines("Week4/Day23.txt")
            .Select((x, y) => x.Select((c, x) => (c, x, y)))
            .SelectMany(x => x)
            .Where(x => x.c == '#')
            .Select(x => new Entity { Position = (x.x, x.y) })
            .ToList();
    }

    [DebuggerDisplay("{Position} -> {Goal}")]
    private class Entity
    {
        public Vec2 Position { get; set; }
        public Vec2? Goal { get; set; }
    }

    public object? TaskA()
    {
        return XD();
    }

    public object? TaskB()
    {
        return null;
    }

    private void ApplyGoals()
    {
        foreach (var entity in _elves)
        {
            if (entity.Goal != null)
            {
                _map.Remove(entity.Position);
                entity.Position = entity.Goal;
                _map.Add(entity.Position);
                entity.Goal = null;
            }
        }
    }

    private void AdjustGoals()
    {
        foreach (var elves in _elves.GroupBy(x => x.Goal).Where(x=>x.Count() > 1))
        {
            foreach (var elf in elves)
            {
                elf.Goal = null;
            }
        }
    }

    private void ProcessMovement(Func<Vec2, bool> isAlone, List<Func<Vec2, Vec2>> directions, int i)
    {
        foreach (var elf in _elves)
        {
            var v = elf.Position;

            if (!isAlone(v))
            {
                for (int j = 0; j < directions.Count; j++)
                {
                    var d = directions[(j + i) % directions.Count](v);
                    if (d != 0)
                    {
                        elf.Goal = v + d;
                        break;
                    }
                }
            }
        }
    }

    private int XD()
    {
        var directions = new List<Func<Vec2, Vec2>>
        {
            v => !_map.Contains(v + _N + _W) && !_map.Contains(v + _N) && !_map.Contains(v + _N + _E) ? _N : 0,
            v => !_map.Contains(v + _S + _W) && !_map.Contains(v + _S) && !_map.Contains(v + _S + _E) ? _S : 0,
            v => !_map.Contains(v + _N + _W) && !_map.Contains(v + _W) && !_map.Contains(v + _S + _W) ? _W : 0,
            v => !_map.Contains(v + _N + _E) && !_map.Contains(v + _E) && !_map.Contains(v + _S + _E) ? _E : 0,
        };

        _map = _elves.Select(x => x.Position).ToHashSet();

        int i = 0;
        for (i = 0; ; i++)
        {
            ProcessMovement(IsAlone, directions, i);

            AdjustGoals();

            // apply goal
            bool finish = _elves.All(x => x.Goal == null);
            if (finish)
            {
                break;
            }

            ApplyGoals();
        }

        return i + 1;
    }

    private bool IsAlone(Vec2 v) => !_map.Contains(v + _N + _W) && !_map.Contains(v + _N) && !_map.Contains(v + _N + _E)
                                    && !_map.Contains(v + _W) && !_map.Contains(v + _E)
                                    && !_map.Contains(v + _S + _W) && !_map.Contains(v + _S) && !_map.Contains(v + _S + _E);

   
}