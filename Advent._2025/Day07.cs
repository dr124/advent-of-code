using Xunit.Internal;

namespace Advent._2025;

[AocData("Day07.txt", part1: 1619, part2: null)]
[AocData("Day07Example.txt", part1: 21, part2: null)]
public class Day07 : Day
{
	public override (object? PartA, object? PartB) Run(string[] lines)
	{
		lines = lines.Where((_, i) => i % 2 == 0).ToArray(); // Skip every second line (empty lines)
		Vec2 start = (lines[0].IndexOf('S'), 0);

		var splitCount = 0;
		Dictionary<Vec2, long> beams = new() { { start, 1 } };
		foreach (var _ in lines)
		{
			Dictionary<Vec2, long> beams2 = [];

			foreach (var beam in beams)
			{
				if (beam.Key.On(lines) == '^')
				{
					UpdateVec(beams2, beam.Key + Vec2.Down + Vec2.Left, beam.Value);
					UpdateVec(beams2, beam.Key + Vec2.Down + Vec2.Right, beam.Value);
					splitCount++;
				}
				else
				{
					UpdateVec(beams2, beam.Key + Vec2.Down, beam.Value);
				}
			}

			beams = beams2;
		}

		return (splitCount, beams.Values.Sum());
	}

	private static void UpdateVec(Dictionary<Vec2, long> beams2, Vec2 pos, long beam)
	{
		if (!beams2.TryAdd(pos, beam))
		{
			beams2[pos] += beam;
		}
	}
}