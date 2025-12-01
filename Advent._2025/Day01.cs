namespace Advent._2025;

[AocData("Day01.txt", part1: 1043, part2: 5963)]
[AocData("Day01Example.txt", part1: 3, part2: 6)]
public class Day01 : Day
{
	public override (object? PartA, object? PartB) Run(string[] lines)
	{
		var turns = lines
			.Select(x => x[0] switch
			{
				'L' => -int.Parse(x[1..]),
				'R' => int.Parse(x[1..]),
				_ => throw new InvalidOperationException("Invalid direction")
			})
			.ToArray();

		const int max = 100;
		var position = 50;
		var fullStopZeros = 0;
		var allClickedZeros = 0;

		foreach (var t in turns)
		{
			var turn = t;
			var fullTurns = Math.Abs(turn) / max;

			// each full turn clicks zero once
			allClickedZeros += fullTurns;
			turn %= max;

			var prev = position;
			var next = (position + turn + max) % max;

			// clicks zero when passing it
			var clickedLeft = prev != 0 && next != 0 && turn < 0 && prev < next;
			var clickedRight = prev != 0 && next != 0 && turn > 0 && next < prev;
			if (clickedRight || clickedLeft)
			{
				allClickedZeros++;
			}

			// stops at zero
			if (next == 0)
			{
				allClickedZeros++;
				fullStopZeros++;
			}

			position = next;
		}

		return (fullStopZeros, allClickedZeros);
	}
}