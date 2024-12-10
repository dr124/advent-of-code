namespace Advent._2024.Week2;

public class Day10(string[] input) : IDay
{
    private readonly Vec2[] _trailheads = Extensions.ReadInput(input, '0').ToArray();

    public object Part1() => _trailheads
        .Select(x => Traverse(x, '0').Distinct())
        .Sum(x => x.Count());

    public object Part2() => _trailheads
        .Select(x => Traverse(x, '0'))
        .Sum(x => x.Count());

    private IEnumerable<Vec2> Traverse(Vec2 position, int value)
    {
        if (position.On(input) == '9')
        {
            yield return position;
            yield break;
        }
        
        foreach (var side in Vec2.Sides)
        {
            var pos = position + side;
            if (pos.IsInBounds(input) && pos.On(input) == value + 1)
            {
                foreach (var found in Traverse(pos, value + 1))
                {
                    yield return found;
                }
            }
        }
    }
}