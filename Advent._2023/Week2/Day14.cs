using System.Buffers;
using System.Text;

namespace Advent._2023.Week2;

public class Day14(string[] input) : IDay
{
    private readonly char[][] _map = input.Select(x => x.ToCharArray()).ToArray();
    private int N = input.Length;

    private const char None = '.';
    private const char Ball = 'O';
    private const char Cube = '#';

    public object Part1()
    {
        SlideNorth(_map);
        return GetLoad(_map);
    }

    public object Part2()
    {
        var cycles = 1_000_000_000;

        // do cycles until repeat
        var seen = new Dictionary<string, int>();
        var map = _map;
        for (var i = 0; i < cycles; i++)
        {
            var key = GetKey(map);
            if (seen.ContainsKey(key))
            {
                var cycleLength = i - seen[key];
                var remainingCycles = cycles - i;
                var remainingCyclesMod = remainingCycles % cycleLength;
                for (var j = 0; j < remainingCyclesMod; j++)
                {
                    Cycle(map);
                }

                return GetLoad(map);
            }

            seen[key] = i;
            Cycle(map);
        }

        return null;
    }
    
    private int GetLoad(char[][] map)
    {
        var sum = 0;
        for (var y = 0; y < N; y++)
        for (var x = 0; x < N; x++)
        {
            if (map[^(y + 1)][x] == Ball)
            {
                sum += y + 1;
            }
        }

        return sum;
    }

    private static string GetKey(char[][] map) => string.Join(Environment.NewLine, map.Select(chars => new string(chars)));

    private void Cycle(char[][] i)
    {
        SlideNorth(i);
        SlideWest(i);
        
        // south
        i.AsSpan().Reverse();
        SlideNorth(i);

        // east
        foreach (var obj in i)
        {
            obj.AsSpan().Reverse();
        }
        SlideWest(i); 
        
        // return back to normal
        i.AsSpan().Reverse();
        foreach (var obj in i)
        {
            obj.AsSpan().Reverse();
        }
    }
    
    private void SlideNorth(char[][] map) => Slide((x, y) => map[y][x], (x, y, o) => map[y][x] = o);
    
    private void SlideWest(char[][] map) => Slide((y, x) => map[y][x], (y, x, o) => map[y][x] = o);
    
    private void Slide(Func<int, int, char> getObject, Action<int, int, char> setObject)
    {
        for (var axis = 0; axis < N; axis++)
        {
            var indexOfEmpty = 0;
            for (var lineIndex = 0; lineIndex < N; lineIndex++)
            {
                var o = getObject(axis, lineIndex);
                if (o is Cube)
                {
                    setObject(axis, lineIndex, Cube);
                    indexOfEmpty = lineIndex + 1;
                }
                else if (o is Ball)
                {
                    setObject(axis, lineIndex, None);
                    setObject(axis, indexOfEmpty, Ball);
                    indexOfEmpty++;
                }
            }
        }
    }
}