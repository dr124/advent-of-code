using System.Security.Cryptography.X509Certificates;
using Advent.Core.Extensions;

namespace Advent._2022.Week3;

public class Day15 : IReadInputDay
{
    private readonly List<Area> _areas = new();
    private HashSet<Vec2> _beacons = new();

    public void ReadData()
    {
        var lines = File.ReadAllLines("Week3/Day15.txt")
            .Select(x => x.Split(new[] { "=", ",", ":" }, SplitOptions.Clear))
            .To2dArray();

        foreach (var line in lines)
            _areas.Add(new Area(
                (int.Parse(line[1]), int.Parse(line[3])),
                (int.Parse(line[5]), int.Parse(line[7]))));
        _beacons = _areas.Select(x => x.Beacon).ToHashSet();
    }

    public object TaskA()
    {
        const int y = 2000000;
        var sum = 0;

        var from = _areas.Min(x => x.Sensor.X - x.Distance);
        var to = _areas.Max(x => x.Sensor.X + x.Distance);

        Parallel.For(from, to + 1, x =>
        {
            if (_beacons.Contains((x, y)))
            {

            } 
            else if (GetInRange((x, y)) is not null)
            {
                sum++;
            }
        });

        return sum;
    }

    public object TaskB()
    {
        var to = 4_000_000;

        long result = -1;
        Parallel.For(0, to+1, (y, pls) =>
        {
            for (int x = 0; x <= to; x++)
            {
                var area = GetInRange((x, y));
                if (area is not null)
                {
                    x = area.GetP((x, y));
                }
                else
                {
                    Console.WriteLine($"kuzwa tu: {x},{y}");
                    result = (long)x * to + y;
                    pls.Stop();
                }
            }
        });

        return result;
    }

    private Area GetInRange(Vec2 p)
    {
        return _areas.FirstOrDefault(xd => xd.Contains(p));
    }


    private record Area(Vec2 Sensor, Vec2 Beacon)
    {
        private int? _distance;
        public int Distance => _distance ??= (Beacon - Sensor).ManhattanLength();

        public bool Contains(Vec2 point) => (Sensor - point).ManhattanLength() <= Distance;
        public int GetP(Vec2 c) => Distance + Sensor.X - Math.Abs(Sensor.Y - c.Y);
    }
}