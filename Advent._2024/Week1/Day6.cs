namespace Advent._2024.Week1;

public class Day6(string[] input) : IDay
{
    private readonly Vec2 _startingDirection = Vec2.Up;
    private readonly Vec2 _startingPosition = FindPosition(input, '^').First();
    private readonly HashSet<Vec2> _obstacles = FindPosition(input, '#').ToHashSet();
    private readonly HashSet<Vec2> _visited = [];

    public object Part1()
    {
        var position = _startingPosition;
        var direction = _startingDirection;
        
        do
        {
            _visited.Add(position);
            (position, direction) = GetDirection(position, direction, _obstacles);
        } while (position.IsInBounds(input));
        
        return _visited.Count;
    }

    public object Part2()
    {
        var sum = 0;

        Parallel.ForEach(_visited.Where(x => x != _startingPosition), v =>
        {
            var position = _startingPosition;
            var direction = _startingDirection;
            var obstacles = _obstacles.Append(v).ToHashSet();
            HashSet<(Vec2 position, Vec2 direction)> visitedInLoop = [];

            while (position.IsInBounds(input))
            {
                visitedInLoop.Add((position, direction));
                (position, direction) = GetDirection(position, direction, obstacles);

                if (visitedInLoop.Contains((position, direction)))
                {
                    Interlocked.Increment(ref sum);
                    break;
                }
            }
        });

        return sum;
    }
    
    private static (Vec2 position, Vec2 direction) GetDirection(Vec2 position, Vec2 direction, HashSet<Vec2> obstacles)
    {
        var nextPosition = position + direction;
        while (obstacles.Contains(nextPosition))
        {
            direction = direction.Rotate(Rotation.Clockwise);
            nextPosition = position + direction;
        }

        return (nextPosition, direction);
    }

    private static IEnumerable<Vec2> FindPosition(string[] input, int character) =>
        from y in Enumerable.Range(0, input.Length)
        from x in Enumerable.Range(0, input[0].Length)
        where input[y][x] == character
        select new Vec2(x, y);
}