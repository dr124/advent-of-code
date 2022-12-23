using Advent.Core.Extensions;
using System.Text.RegularExpressions;

namespace Advent._2022.Week4;

public class Day22 : IReadInputDay
{
    private char[,] _map;
    private string[] _key;
    private int _width = 0;

    public void ReadData()
    {
        var lines = File.ReadAllLines("Week4/Day22.txt");

        var maxWidth = lines[..^2].Max(x => x.Length);
        _map = lines[..^2].Select(x=>x.PadRight(maxWidth, ' ')).To2dArray().ToMatrix();
        _key = lines[^1].Replace("L", " L ").Replace("R", " R ").Split(" ", SplitOptions.Clear);
        _width = lines[0].Count(x => x != ' ')/2;
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
            }
            else if (operation == "R")
            {
                direction = direction.Rotate(RotateDirection.Right);
            }
            else
            {
                var n = int.Parse(operation);
                for (int i = 0; i < n; i++)
                {
                    var nextPosition = GetNextPosition(position, direction).pos;

                    if (_map.At(nextPosition) == '#')
                        break;

                    (position, direction) = GetNextPosition(position, direction);
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

    private (Vec2 pos, Vec2 dir) FindNextPos(Vec2 position, Vec2 direction)
    {
        var p = position + 0; // copy :D 
        var d = (-direction.X, -direction.Y);
        while (IsInBounds(p) && _map.At(p) != ' ')
        {
            p += d;
        }

        p -= d;
        return (p, direction);
    }

    private (Vec2 pos, Vec2 dir) FindNextPos2(Vec2 position, Vec2 direction)
    {
        var chunkOld = GetChunk(position);
        var chunkNew = GetChunk(position + direction);
        var chunkPos = position % _width;

        Vec2 chunk = (-100, -100);
        Vec2 newPosition = Vec2.Zero;
        Vec2 newDirection = Vec2.Zero;

        Action action = (chunkOld, chunkNew) switch
        {
            ((0,3), (0,4)) => () => FlipLR((2,0)),
            ((2,0), (2,-1)) => () => FlipLR((0,3)),
            
            ((0,2), (0,1)) => () => RotateCW((1,1)),
            ((1,2), (1,3)) => () => RotateCW((0,3)),
            ((2,0), (2,1)) => () => RotateCW((1,1)),
            ((1,1), (2,1)) => () => RotateCW((2,0)),
            ((0,3), (-1,3)) => () => RotateCW((1,0)),
            
            ((2, 0), (3, 0)) => () => FlipUD((1, 2)),
            ((1, 2), (2, 2)) => () => FlipUD((2, 0)),
            ((1, 0), (0, 0)) => () => FlipUD((0, 2)),
            ((0, 2), (-1, 2)) => () => FlipUD((1, 0)),
            
            ((1,0), (1,-1)) => () => RotateCCW((1,0)),
            ((1, 1), (0, 1)) => () => RotateCCW((0, 2)),
            ((0, 3), (1, 3)) => () => RotateCCW((1, 2)),
        };

        action();
        
        void RotateCCW(Vec2 c)
        {
            chunk = c;
            newPosition = chunk * _width + (chunkPos.Y, chunkPos.X);
            newDirection = (direction.Y, direction.X);
        }

        void RotateCW(Vec2 c)
        {
            chunk = c;
            newPosition = chunk * _width + (chunkPos.Y, chunkPos.X);
            newDirection = (-direction.Y, direction.X);
        }

        void FlipUD(Vec2 c)
        {
            chunk = c;
            newPosition = chunk * _width + (chunkPos.X, _width - 1 - chunkPos.Y);
            newDirection = (-direction.X, direction.Y);
        }

        void FlipLR(Vec2 c)
        {
            chunk = c;
            newPosition = chunk * _width + (chunkPos.X, _width - 1 - chunkPos.Y);
            newDirection = (direction.X, -direction.Y);
        }

        return (newPosition, newDirection);
    }

    private Vec2 GetChunk(Vec2 position)
    {
        var chunk = position / _width;
        if(position.Y < 0)
            chunk.Y--;
        if (position.X < 0)
            chunk.X--;
        return chunk;
    }

    private (Vec2 pos, Vec2 dir) GetNextPosition(Vec2 position, Vec2 direction)
    {
        Vec2 dp = (direction.X, -direction.Y);
        if (IsInBounds(position + dp) && _map.At(position + dp) is not ' ')
            return (position + dp, direction);

        return FindNextPos2(position, dp);  // B
        //return (FindNextPos(position, dp).pos, direction); // A
    }

    private bool IsInBounds(Vec2 position) =>
        position.X >= 0
        && position.Y >= 0
        && position.X < _map.GetLength(1) 
        && position.Y < _map.GetLength(0);

    public object? TaskB()
    {
        return null;
    }
}