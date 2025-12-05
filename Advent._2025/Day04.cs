namespace Advent._2025;

[AocData("Day04.txt", part1: 1527, part2: 8690)]
[AocData("Day04Example.txt", part1: 13, part2: 43)]
public class Day04 : Day
{
	public override (object? PartA, object? PartB) Run(string[] lines)
	{
		var points = Extensions.ReadInput(lines, c => c == '@').ToHashSet();
		var startCount = points.Count;
		var firstStageRemoved = RemoveRolls(points);

		while (true)
		{
			var removed = RemoveRolls(points);

			if (removed == 0)
				break;
		}

		var totalRemoved = startCount - points.Count;
		return (firstStageRemoved, totalRemoved);
	}

	private static int RemoveRolls(HashSet<Vec2> points)
	{
		var toRemove = points.Where(point => IsAccessible(points, point)).ToList();
		foreach (var point in toRemove)
		{
			points.Remove(point);
		}

		return toRemove.Count;
	}

	private static bool IsAccessible(HashSet<Vec2> points, Vec2 point)
	{
		return point.Adjacent().Count(points.Contains) < 4;
	}
}