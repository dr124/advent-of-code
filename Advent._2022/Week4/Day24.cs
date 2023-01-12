using System.Diagnostics;
using Advent.Core;
using Advent.Core.Extensions;

namespace Advent._2022.Week4;

public class Day24 : IReadInputDay
{
    private Blizzard[] _blizzards;
    private Vec2 _mapSize;

    public void ReadData()
    {
        var map = File.ReadAllLines("Week4/Day24.txt")
            .Select(x => x[1..^1])
            .ToArray()[1..^1]
            .To2dArray();

        _blizzards = map
            .Select((x, y) => x.Select((c, x) => (c, x, y)))
            .SelectMany(x => x)
            .Where(x => x.c is '>' or 'v' or '^' or '<')
            .Select(x => new Blizzard
            {
                Position = (x.x, x.y),
                Direction = x.c switch
                {
                    '>' => (1, 0),
                    'v' => (0, 1),
                    '^' => (0, -1),
                    '<' => (-1, 0),
                    _ => throw new Exception()
                }
            })
            .ToArray();

        _mapSize = (map[0].Length, map.Length);
    }

    [DebuggerDisplay("{Position}, dir: {Direction}")]
    private class Blizzard
    {
        public Vec2 Position { get; set; }
        public Vec2 Direction { get; set; }
    }

    public object? TaskA()
    {
        int[,] map = new int[_mapSize.Y, _mapSize.X];
        foreach (var v in map.EnumerateVec())
        {
            map.At(v) = int.MaxValue;
        }
    
        for (int i = 1; i < 100000; i++)
        {
            map[0, 0] = i;

            Console.SetCursorPosition(0,0);
            //DrawAll(map);
            //Thread.Sleep(100);

            Go(map);
            ProcessBlizzards(map);

            var last = map[_mapSize.Y - 1, _mapSize.X - 1];
            if (last != int.MaxValue)
            {
                Console.SetCursorPosition(0, 0);
                DrawMap(map);
                return last;
            }
        }

        return null;
    }

    // 283 too low

    private void Go(int[,] map)
    {
        var m = map.Copy();
        
        for (int y = 0; y < _mapSize.Y; y++)
            for (int x = 0; x < _mapSize.X; x++)
            {
                var p = m[y, x];
                if (p == int.MaxValue)
                    continue;

                if (x > 0) // to left
                {
                    var p2 = m[y, x - 1];
                    if (p < p2)
                        map[y, x - 1] = p + 1;
                }

                if (x < _mapSize.X - 1) // to right
                {
                    var p2 = m[y, x + 1];
                    if (p < p2)
                        map[y, x + 1] = p + 1;
                }

                if (y > 0) // to top
                {
                    var p2 = m[y - 1, x];
                    if (p < p2)
                        map[y - 1, x] = p + 1;
                }

                if (y < _mapSize.Y - 1) // to bottom
                {
                    var p2 = m[y + 1, x];
                    if (p < p2)
                        map[y + 1, x] = p + 1;
                }
            }

        for (int y = 0; y < _mapSize.Y; y++)
        for (int x = 0; x < _mapSize.X; x++)
            if (m[y, x] != int.MaxValue) 
                map[y, x]++;
    }

    private bool HasBlizzard(Vec2 v)
    {
        return _blizzards.Any(x => x.Position == v);
    }

    private void ProcessBlizzards(int[,] map)
    {
        foreach (var blizzard in _blizzards)
        {
            blizzard.Position = (blizzard.Position + blizzard.Direction + _mapSize) % _mapSize;
            map.At(blizzard.Position) = int.MaxValue;
        }
    }

    public void Draw()
    {
        var map = new char[_mapSize.Y, _mapSize.X];

        Array.ForEach(map.EnumerateVec().ToArray(), x => map.At(x) = '.');

        foreach (var blizzard in _blizzards)
        {
            map.At(blizzard.Position) = blizzard.Direction switch
            {
                (1, 0) => '>',
                (0, 1) => 'v',
                (0, -1) => '^',
                (-1, 0) => '<',
                _ => throw new Exception()
            };
        }
        Console.WriteLine(map.ToMatrixString(""));
    }

    public void DrawMap(int[,] map)
    {
        var map2 = new string[_mapSize.Y, _mapSize.X];

        Array.ForEach(map.EnumerateVec().ToArray(), x => map2.At(x) = "  ");

        for (int y = 0; y < _mapSize.Y; y++)
            for (int x = 0; x < _mapSize.X; x++)
            {
                var p = map[y, x];
                if (p == int.MaxValue)
                    map2[y, x] = ".";
                else
                    map2[y, x] = (p%10).ToString("D1");
            }

        Console.WriteLine(map2.ToMatrixString(""));
    }

    public void DrawAll(int[,] map)
    {
        var map2 = new char[_mapSize.Y, _mapSize.X];

        Array.ForEach(map.EnumerateVec().ToArray(), x => map.At(x) = '.');

        foreach (var blizzard in _blizzards)
        {
            map2.At(blizzard.Position) = blizzard.Direction switch
            {
                (1, 0) => '>',
                (0, 1) => 'v',
                (0, -1) => '^',
                (-1, 0) => '<',
                _ => throw new Exception()
            };
        }

        for (int y = 0; y < _mapSize.Y; y++)
        for (int x = 0; x < _mapSize.X; x++)
        {
            var p = map[y, x];
            if (p == int.MaxValue)
                map2[y, x] = '.';
            else
                map2[y, x] = (p % 10).ToString()[0];
        }

        Console.WriteLine(map2.ToMatrixString(""));
    }

    public object? TaskB()
    {
        return null;
    }
}