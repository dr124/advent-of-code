using Advent.Core;
namespace Advent._2021.Week3;
using Point = Vec3;

internal class Day19 : IReadInputDay
{
    private List<Point> scannerPosition;
    
    class Map
    {
        public List<Point> _mapCopy;
        public List<Point> _map;
        public Point[,] Diffs;
        public int rotation = 0;
        public HashSet<Point> distances = new();
        public Map(List<Point> list)
        {
            _mapCopy = list;
            Restore();
            CalculateDiffs();
        }

        public void RotateAll()
        {
            Restore();
            rotation = ( rotation + 1 ) % 24;
            for (int i = 0; i < _map.Count; i++)
                _map[i] = Rotate(_map[i], rotation);
            CalculateDiffs();
        }

        public void Restore()
        {
            _map = _mapCopy.Select(x => (Point)(x.X, x.Y, x.Z)).ToList();
        }

        public void CalculateDiffs()
        {
            Diffs = new Point[_map.Count, _map.Count];
            distances.Clear();;
            for (int i = 0; i < _map.Count; i++)
            for (int j = 0; j <= i; j++)
                if (i != j)
                {
                    Diffs[i, j] = (_map[i] - _map[j]);//.Abs();
                    distances.Add(Diffs[i, j]);
                }
        }

        public (Point p1, Point p2) GetPointFromDiff(Point diff)
        {
            for (int i = 0; i < _map.Count; i++)
            for (int j = 0; j <= i; j++)
                if (Diffs[i, j] == diff)
                    return (_map[i], _map[j]);
            return (null, null)!;
        }

        public void TranslateAll(Point vec)
        {
            for (int i = 0; i < _map.Count; i++)
                _map[i] += vec;
        }

        public void MergeMap(Map m)
        {
            var h = _map.ToHashSet();
            h.UnionWith(m._map);
            _map = h.ToList();
            CalculateDiffs();
        }
    }

    private List<Map> Maps;

    public void ReadData()
    {
        var lines = File.ReadAllLines("Week3/Day19.txt");
        List<Map> scanners = new();
        List<Point> list = new();
        foreach (var line in lines)
            if (line.Contains("---")) // --- scanner X ---
                list = new();
            else if (string.IsNullOrWhiteSpace(line))
                scanners.Add(new Map(list));
            else
                list.Add(Point.FromString(line));
        scanners.Add(new Map(list));
        Maps = scanners;
    }

    public object TaskA()
    {
        var scanner1 = Maps[0];
        var MapsLeft = Maps.Skip(1).ToList();


        scannerPosition = new() { (0, 0, 0) };
        for(int i = 0; i < MapsLeft.Count; i++)
        {
            var scanner2 = MapsLeft[i];
            for (int n = 0; n< 50; n++)
            {
                scanner2.RotateAll();
                var same = scanner2.distances.Where(x => scanner1.distances.Contains(x)).ToList();
                if (same.Count >= 3)
                {
                    MergeMaps(scanner1, scanner2, same, out var distance);
                    scannerPosition.Add(distance);
                    MapsLeft.RemoveAt(i);
                    i = -1;
                    break;
                }
            }
        }

        return null;
    }

    void MergeMaps(Map m1, Map m2, List<Point> distancesInBoth, out Point scannerPos)
    {
        var d1 = distancesInBoth[0];
        var d1m1 = m1.GetPointFromDiff(d1);
        var d1m2 = m2.GetPointFromDiff(d1);
        var diff1 = d1m1.p1 - d1m2.p1;
        var diff2 = d1m1.p2 - d1m2.p2;
        m2.TranslateAll(diff1);
        d1m2 = m2.GetPointFromDiff(d1);
        m1.MergeMap(m2);
        scannerPos = diff1;
    }


    public object TaskB() => (
            from p1 in scannerPosition
            from p2 in scannerPosition
            where p1 != p2
            select (p1 - p2).ManhattanDistance())
        .Max();

    static Point Rotate(Point v, int n)
    {
        (int x, int y, int z) = v;
        return n switch
        {
            0 => (x, y, z),
            1 => (x, -y, -z),
            2 => (-x, y, -z),
            3 => (-x, -y, z),
            4 => (x, z, -y),
            5 => (x, -z, y),
            6 => (-x, z, y),
            7 => (-x, -z, -y),
            8 => (y, z, x),
            9 => (y, -z, -x),
            10 => (-y, z, -x),
            11 => (-y, -z, x),
            12 => (y, x, -z),
            13 => (y, -x, z),
            14 => (-y, x, z),
            15 => (-y, -x, -z),
            16 => (z, x, y),
            17 => (z, -x, -y),
            18 => (-z, x, -y),
            19 => (-z, -x, y),
            20 => (z, y, -x),
            21 => (z, -y, x),
            22 => (-z, y, x),
            23 => (-z, -y, -x),
        };
    }
}