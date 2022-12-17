using Advent.Core;
using System.Linq;
using Microsoft.Diagnostics.Tracing;

namespace Advent._2022.Week3;

public class Day17 : IReadInputDay
{
    public char[] _input;
    public HashSet<Vec2>[] _shapes;
    public void ReadData()
    {
        _input = File.ReadAllText("Week3/day17.txt").ToCharArray();
        _shapes = new HashSet<Vec2>[5];
        var shape1 = """
            ####
            """;
        var shape2 = """
            .#.
            ###
            .#. 
            """;
        var shape3 = """
            ###
            ..#
            ..#
            """;
        var shape4 = """
            #
            #
            #
            #
            """;
        var shape5 = """
            ##
            ##
            """;

        _shapes[0] = ConvertShape(shape1);
        _shapes[1] = ConvertShape(shape2);
        _shapes[2] = ConvertShape(shape3);
        _shapes[3] = ConvertShape(shape4);
        _shapes[4] = ConvertShape(shape5);

        static HashSet<Vec2> ConvertShape(string shape)
        {
            return shape.Split(Environment.NewLine)
                .Select((s, y) => s
                    .Select((c, x) => c == '#' ? new Vec2(x, y) : null)
                    .Where(v => v != null).ToArray())
                .SelectMany(v => v)
                .ToHashSet();
        }
    }

    public object? TaskA()
    {
        var grid = new HashSet<Vec2>();
        var width = 7;

        // make floor
        for (var x = 0; x <= width; x++)
        {
            grid.Add(new Vec2(x, 0));
        }

        // tetris simulation
        var i = 0;
        var j = 0;
        long block = 0;
        while (block < 1000000000000L)
        {
            var shape = _shapes[i % 5];
            int maxY = grid.Max(x => x.Y);
            Vec2 offset = (2, maxY + 4);
            while (true)
            {
                //DrawMap(20, 0, grid, shape, offset);

                var direction = _input[j++ % _input.Length];
                if (direction == '>')
                {
                    offset.X++;
                    if (DoesHit(grid, shape, offset))
                    {
                        offset.X--;
                    }
                }
                else if (direction == '<')
                {
                    offset.X--;
                    if (DoesHit(grid, shape, offset))
                    {
                        offset.X++;
                    }
                }

                // fall down
                offset.Y--;
                if (DoesHit(grid, shape, offset))
                {
                    offset.Y++;
                    foreach (var v in shape)
                    {
                        grid.Add(v + offset);
                    }

                    block++;
                    break; // this shape end
                }

                TryReduceMap(grid);
                //DrawMap(20, 0, grid, shape, offset);
            }
            //DrawMap(20, 0, grid, shape, offset);
            i++;
        }

        return grid.Max(x => x.Y);
    }

    private void TryReduceMap(HashSet<Vec2> map)
    {
        if (map.Count > 10000)
        {
            // check if there are duplicate lines
            var lines = map.GroupBy(x => x.Y)
                .Select(x => x.Select(x => x.X))
                .Select(ToInt)
                .ToArray();
        }
    }

    private void DrawMap(int fromY, int toY, HashSet<Vec2> grid, HashSet<Vec2> shape = null, Vec2 shapeOfffset = null)
    {
        int fromX = 0;
        int toX = 6;
        Console.Clear();
        for (var y = fromY; y >= toY; y--)
        {
            for (var x = fromX; x <= toX; x++)
            {
                var v = new Vec2(x, y);
                if (grid.Contains(v))
                {
                    Console.Write('#');
                }
                else if (shape != null && shape.Contains(v - shapeOfffset))
                {
                    Console.Write('X');
                }
                else
                {
                    Console.Write('.');
                }
            }
            Console.WriteLine();
        }
    }

    private bool DoesHit(IReadOnlySet<Vec2> grid, IEnumerable<Vec2> shape, Vec2 offset)
    {
        return shape.Any(v => (v + offset).X is >= 7 or < 0)
               || shape.Any(v => grid.Contains(v + offset));
    }

    public object? TaskB()
    {
        return null;
    }

    int ToInt(IEnumerable<int> tab) => tab.Select((x, i) => (x == 1 ? 1 : 0) * (1 << i)).Sum();
}