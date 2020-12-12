using System.IO;
using System.Linq;
using Advent.Core;

namespace Advent._2020.Week2
{
    public class Day12 : Day<Day12.Movement[], int>
    {
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

            foreach (var mov in Input)
                switch (mov.dir)
                {
                    case Direction.North:
                        position.Y += mov.val;
                        break;
                    case Direction.East:
                        position.X += mov.val;
                        break;
                    case Direction.West:
                        position.X -= mov.val;
                        break;
                    case Direction.South:
                        position.Y -= mov.val;
                        break;
                    case Direction.Forward:
                        position += direction.Scale(mov.val);
                        break;
                    case Direction.Left:
                        for (var a = mov.val; a != 0; a -= 90)
                            (direction.X, direction.Y) = (-direction.Y, direction.X);
                        break;
                    case Direction.Right:
                        for (var a = mov.val; a != 0; a -= 90)
                            (direction.X, direction.Y) = (direction.Y, -direction.X);
                        break;
                }

            return position.ManhattanDistance;
        }

        protected override int TaskB()
        {
            Vec2 position = (0, 0);
            Vec2 waypoint = (10, 1);

            foreach (var mov in Input)
                switch (mov.dir)
                {
                    case Direction.North:
                        waypoint.Y += mov.val;
                        break;
                    case Direction.East:
                        waypoint.X += mov.val;
                        break;
                    case Direction.West:
                        waypoint.X -= mov.val;
                        break;
                    case Direction.South:
                        waypoint.Y -= mov.val;
                        break;
                    case Direction.Forward:
                        position += waypoint.Scale(mov.val);
                        break;
                    case Direction.Left:
                        for (var a = mov.val; a != 0; a -= 90)
                            waypoint = waypoint.RotateLeft();
                        break;
                    case Direction.Right:
                        for (var a = mov.val; a != 0; a -= 90)
                            waypoint = waypoint.RotateRight();
                        break;
                }

            return position.ManhattanDistance;
        }

    }
}