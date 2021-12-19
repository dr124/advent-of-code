using Advent.Core;
namespace Advent._2021.Week3;
using Point = Vec3;

internal class Day19 : IReadInputDay
{
    private readonly List<Point> ScannerPositions = new() { (0, 0, 0) };
    private readonly List<Map> Maps = new ();

    public void ReadData()
    {
        var lines = File.ReadAllLines("Week3/Day19.txt");
        List<Point> list = new();
        foreach (var line in lines)
            if (line.Contains("---"))
                list = new();
            else if (string.IsNullOrWhiteSpace(line))
                Maps.Add(new Map(list));
            else
                list.Add(Point.FromString(line));
        Maps.Add(new Map(list));
    }

    public object TaskA()
    {
        var mainMap = Maps[0];
        var mapsLeft = Maps.Skip(1).ToList();

        for(int i = 0; mapsLeft.Count > 0; i++)
        {
            var otherMap = mapsLeft[i];
            for (int n = 0; n < 24; n++)
            {
                otherMap.RotateAll();
                var same = otherMap.Distances.Where(x => mainMap.Distances.Contains(x)).ToList();
                if (same.Count >= 3)
                {
                    MergeMaps(mainMap, otherMap, same, out var distance);
                    ScannerPositions.Add(distance);
                    mapsLeft.Remove(otherMap);
                    i = -1;
                    break;
                }
            }
        }
        return mainMap.Points.Count;
    }

    public object TaskB() => (
            from p1 in ScannerPositions
            from p2 in ScannerPositions
            where p1 != p2
            select (p1 - p2).ManhattanDistance())
        .Max();

    void MergeMaps(Map m1, Map m2, List<Point> distancesInBoth, out Point scannerPos)
    {
        var d1 = distancesInBoth[0];
        var d1m1 = m1.GetPointFromDiff(d1);
        var d1m2 = m2.GetPointFromDiff(d1);
        var diff1 = d1m1.p1 - d1m2.p1;
        m2.TranslateAll(diff1);
        m1.MergeMap(m2);
        scannerPos = diff1;
    }

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

    class Map
    {
        private List<Point> _mapCopy;
        private Point[,] Diffs;
        private int Rotation = 0;

        public List<Point> Points;
        public HashSet<Point> Distances = new();

        public Map(List<Point> list)
        {
            _mapCopy = list;
            Restore();
            CalculateDiffs();
        }

        public void RotateAll()
        {
            Restore();
            Rotation = (Rotation + 1) % 24;
            for (int i = 0; i < Points.Count; i++)
                Points[i] = Rotate(Points[i], Rotation);
            CalculateDiffs();
        }

        private void Restore()
        {
            Points = _mapCopy.Select(x => (Point)(x.X, x.Y, x.Z)).ToList();
        }

        private void CalculateDiffs()
        {
            Diffs = new Point[Points.Count, Points.Count];
            Distances.Clear();
            for (int i = 0; i < Points.Count; i++)
            for (int j = 0; j <= i; j++)
                if (i != j)
                {
                    Diffs[i, j] = Points[i] - Points[j];
                    Distances.Add(Diffs[i, j]);
                }
        }

        public (Point p1, Point p2) GetPointFromDiff(Point diff)
        {
            for (int i = 0; i < Points.Count; i++)
            for (int j = 0; j <= i; j++)
                if (Diffs[i, j] == diff)
                    return (Points[i], Points[j]);
            throw new ArgumentOutOfRangeException(nameof(diff));
        }

        public void TranslateAll(Point vec)
        {
            for (int i = 0; i < Points.Count; i++)
                Points[i] += vec;
        }

        public void MergeMap(Map otherMap)
        {
            var h = Points.ToHashSet();
            h.UnionWith(otherMap.Points);
            Points = h.ToList();
            CalculateDiffs();
        }
    }
}