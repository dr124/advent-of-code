using System.IO;
using System.Linq;
using Advent.Core_2019_2020;

namespace Advent._2020.Week2;

public class Day12 : Day<Day12.Movement[], int>
{
    protected override Movement[] ReadData()
    {
        return File.ReadAllLines("Week2/input12.txt")
            .Select(x =>
            {
                var dir = (Direction) x[0];
                var val = int.Parse(x[1..]);
                return new Movement(dir, val);
            }).ToArray();
    }

    protected override int TaskA()
    {
        Vec2 direction = (1, 0);
        Vec2 position = (0, 0);
        foreach (var (dir, val) in Input)
            switch (dir)
            {
                case Direction.North:
                case Direction.South:
                case Direction.East:
                case Direction.West:
                    position += DirToVec(dir).Scale(val);
                    break;
                case Direction.Left:
                case Direction.Right:
                    for (var a = val; a > 0; a -= 90)
                        direction = direction.Rotate(DirToRot(dir));
                    break;
                case Direction.Forward:
                    position += direction.Scale(val);
                    break;
            }

        return position.ManhattanDistance;
    }

    protected override int TaskB()
    {
        Vec2 position = (0, 0);
        Vec2 waypoint = (10, 1);

        foreach (var (dir, val) in Input)
            switch (dir)
            {
                case Direction.North:
                case Direction.South:
                case Direction.East:
                case Direction.West:
                    waypoint += DirToVec(dir).Scale(val);
                    break;
                case Direction.Left:
                case Direction.Right:
                    for (var a = val; a > 0; a -= 90)
                        waypoint = waypoint.Rotate(DirToRot(dir));
                    break;
                case Direction.Forward:
                    position += waypoint.Scale(val);
                    break;
            }

        return position.ManhattanDistance;
    }

    public enum Direction
    {
        North = 'N',
        South = 'S',
        East = 'E',
        West = 'W',
        Left = 'L',
        Right = 'R',
        Forward = 'F'
    }

    public record Movement(Direction dir, int val);

    public Vec2 DirToVec(Direction dir) =>
        dir switch
        {
            Direction.North => (0, 1),
            Direction.South => (0, -1),
            Direction.East => (1, 0),
            Direction.West => (-1, 0)
        };

    private RotateDirection DirToRot(Direction dir) =>
        dir is Direction.Left
            ? RotateDirection.Left
            : RotateDirection.Right;
}