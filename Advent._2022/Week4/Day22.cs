using Advent.Core;
using Advent.Core.Extensions;

namespace Advent._2022.Week4;

public class Day22 : IReadInputDay
{
    private char[,] _map;
    private string[] _key;

    public void ReadData()
    {
        var lines = File.ReadAllLines("Week4/Day22.txt");

        var maxWidth = lines[..^2].Max(x => x.Length);
        _map = lines[..^2].Select(x=>x.PadRight(maxWidth, ' ')).To2dArray().ToMatrix();
        _key = lines[^1].Replace("L", " L ").Replace("R", " R ").Split(" ", SplitOptions.Clear);
    }

    public object? TaskA()
    {
        var position = Vec2.Zero;
        for(int i = 0; i < _map.GetLength(1); i++)
            if (_map.At(i, 0) == '.')
            {
                position.X += i;
                break;
            }
        
        var direction = new Vec2(1, 0);


        foreach (var operation in _key)
        {
            if (operation == "L")
            {
                direction = direction.Rotate(RotateDirection.Left);
                Console.WriteLine($"rotate L : {direction}");
            }
            else if (operation == "R")
            {
                direction = direction.Rotate(RotateDirection.Right);
                Console.WriteLine($"rotate R : {direction}");
            }
            else
            {
                var n = int.Parse(operation);
                Console.WriteLine($"Go {n}, {position} -> {position + n * direction}");
                for (int i = 0; i < n; i++)
                {
                    //Console.Clear();
                    //Console.WriteLine("-------------------");
                    //Console.WriteLine(_map.ToMatrixString(""));
                    //Console.ReadKey();

                    var nextPosition = GetNextPosition(position, direction);

                    if (_map.At(nextPosition) == '#')
                        break;

                    position = nextPosition;

                    //if (_map.At(position) == '.')
                    //    _map.At(position) = '@';
                }
            }
        }

        var facing = direction switch
        {
            (1, 0) => 0,
            (0, -1) => 1,
            (-1, 0) => 2,
            (0, 1) => 3
        };


        return 1000 * (position.Y + 1) + 4 * (position.X + 1) + facing;
    }

    private Vec2 FindNextPos(Vec2 position, Vec2 direction)
    {
        var p = position + 0; // copy :D 
        var d = (-direction.X, -direction.Y);
        while (IsInBounds(p) && _map.At(p) != ' ')
        {
            p += d;
        }

        p -= d;
        return p;
    }

    private Vec2 GetNextPosition(Vec2 position, Vec2 direction)
    {
        var dp = (direction.X, -direction.Y);
        if (IsInBounds(position + dp) && _map.At(position + dp) is not ' ')
            return position + dp;

        return FindNextPos(position, dp);
        //else throw new Exception("whaa");
    }

    private bool IsInBounds(Vec2 position)
    {
        return position.X >= 0
               && position.X < _map.GetLength(1) 
               && position.Y >= 0
               && position.Y < _map.GetLength(0);
    }

    public object? TaskB()
    {
        return null;
    }
}