using System.Collections.Immutable;
using System.Text;

namespace Advent._2024.Week2;

public class Day14(string[] input) : IDay
{
    private readonly Vec2 _space = input.Length > 50 ? (101, 103) : (11, 7);
    private readonly Robot[] _robots = input.Select(ParseRobot).ToArray();

    public object Part1() => _robots
        .Select(x => SimulateRobot(x, 100))
        .GroupBy(GetQuadrant)
        .Where(x => x.Key > 0)
        .Aggregate(1, (a, b) => a * b.Count());

    public object Part2() => Enumerable.Range(10, 10000 - 10)
        .Select(t => (t, set: _robots.Select(r => SimulateRobot(r, t)).ToHashSet()))
        .Where(x => ContainsStructure(x.set, 10))
        .Select(x => x.t)
        .First();
    
    private Vec2 SimulateRobot(Robot robot, int seconds)
    {
        var pos = robot.Position + robot.Velocity * seconds;
        var x = (pos.X % _space.X + _space.X) % _space.X;
        var y = (pos.Y % _space.Y + _space.Y) % _space.Y;
        return (x, y);
    }

    private int GetQuadrant(Vec2 pos)
    {
        var midX = _space.X / 2;
        var midY = _space.Y / 2;

        if (pos.X < midX && pos.Y < midY) return 1;
        if (pos.X < midX && pos.Y > midY) return 2;
        if (pos.X > midX && pos.Y < midY) return 3;
        if (pos.X > midX && pos.Y > midY) return 4;

        return 0;
    }

    private bool ContainsStructure(HashSet<Vec2> points, int threshold)
    {
        foreach (var point in points)
        {
            var count = 1;

            for (var yOffset = 1; yOffset < threshold; yOffset++)
            {
                var nextPoint = new Vec2(point.X, point.Y + yOffset);
                if (points.Contains(nextPoint))
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            if (count >= threshold)
            {
                return true;
            }
        }

        return false;
    }
    
    private static Robot ParseRobot(string line)
    {
        var split = line.Split(['=', ',', ' ']);
        var pos = (int.Parse(split[1]), int.Parse(split[2]));
        var vel = (int.Parse(split[4]), int.Parse(split[5]));
        return new Robot(pos, vel);
    }

    private record Robot(Vec2 Position, Vec2 Velocity);
}