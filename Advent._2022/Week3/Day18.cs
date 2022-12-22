using Advent.Core;

namespace Advent._2022.Week3;

public class Day18 : IReadInputDay
{
    private CubeType[,,] _input;
    private const int N = 22; // input values ate in range 0..19, offset it by 1
    
    public void ReadData()
    {
        var particles = File.ReadLines("Week3/Day18.txt")
            .Select(x => x.Split(",").Select(int.Parse).ToArray())
            .Select(x => (X: x[0], Y: x[1], Z:x[2]))
            .ToArray();

        _input = new CubeType[N, N, N];

        foreach (var (x,y,z) in particles)
        {
            _input[x + 1, y + 1, z + 1] = CubeType.Obsidian;
        }
    }

    public object? TaskA() => SurfaceArea(CubeType.Unknown);

    public object? TaskB()
    {
        FloodFill();

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                for (int k = 0; k < N; k++)
                {
                    int x = k, y = j, z = i;
                    var p = _input[x, y, z];
                    if (p is CubeType.Water)
                        Console.BackgroundColor = ConsoleColor.Blue;
                    if (p is CubeType.Unknown)
                        Console.BackgroundColor = ConsoleColor.Black;
                    if (p is CubeType.Obsidian)
                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    if (p is CubeType.Air)
                        Console.BackgroundColor = ConsoleColor.White;
                    Console.Write("..");
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();
            }

            Console.ReadKey();
            Console.Clear();
        }

        return SurfaceArea(CubeType.Water);
    }

    private enum CubeType
    {
        Unknown = 0,
        Air,
        Obsidian,
        Water
    }

    private int SurfaceArea(CubeType boundaryType)
    {
        var iterate = (
            from x in Enumerable.Range(1, N - 1)
            from y in Enumerable.Range(1, N - 1)
            from z in Enumerable.Range(1, N - 1)
            select (x, y, z));

        int s = 0;
        foreach (var (x, y, z) in iterate)
        {
            if (_input[x, y, z] == CubeType.Obsidian)
            {
                if (_input[x - 1, y, z] == boundaryType) s++;
                if (_input[x + 1, y, z] == boundaryType) s++;
                if (_input[x, y - 1, z] == boundaryType) s++;
                if (_input[x, y + 1, z] == boundaryType) s++;
                if (_input[x, y, z - 1] == boundaryType) s++;
                if (_input[x, y, z + 1] == boundaryType) s++;
            }
        }

        return s;
    }

    private void FloodFill()
    {
        var iterate = (
           from x in Enumerable.Range(1, N - 2)
           from y in Enumerable.Range(1, N - 2)
           from z in Enumerable.Range(1, N - 2)
           select (x, y, z));

        _input[1,1,1] = CubeType.Water;
        _input[0,0,0] = CubeType.Water;
        _input[0,0,1] = CubeType.Water;

        var t = 0;
        bool changed = false;
        do
        {
            foreach (var (x, y, z) in iterate)
            {
                if (_input[x, y, z] == CubeType.Unknown)
                {
                    if (_input[x + 1, y, z] == CubeType.Water) _input[x,y,z] = CubeType.Water;
                    if (_input[x - 1, y, z] == CubeType.Water) _input[x,y,z] = CubeType.Water;
                    if (_input[x, y - 1, z] == CubeType.Water) _input[x,y,z] = CubeType.Water;
                    if (_input[x, y + 1, z] == CubeType.Water) _input[x,y,z] = CubeType.Water;
                    if (_input[x, y, z - 1] == CubeType.Water) _input[x,y,z] = CubeType.Water;
                    if (_input[x, y, z + 1] == CubeType.Water) _input[x,y,z] = CubeType.Water;
                }
            }
        } while (t++ < 10000);

    }
}

// 3323 too high
// should be 2074
