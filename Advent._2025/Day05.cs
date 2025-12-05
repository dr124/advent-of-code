using Microsoft.Extensions.Primitives;

namespace Advent._2025;

[AocData("Day05.txt", part1: 698, part2: 352807801032167L)]
[AocData("Day05Example.txt", part1: 3, part2: 14)]
public class Day05 : Day
{
	public override (object? PartA, object? PartB) Run(string[] lines)
	{
		var separator = lines.IndexOf(string.Empty);
		var freshIngredientsRanges = lines[..separator]
			.Select(Range.Parse)
			.OrderBy(x => x.From)
			.ThenByDescending(x => x.To)
			.ToList();
		var availableIngredients = lines[(separator + 1)..]
			.Select(long.Parse)
			.ToList();

		MergeRanges(freshIngredientsRanges);

		var freshIngredientCount = availableIngredients.Count(i => IsIngredientFresh(i, freshIngredientsRanges));
		var totalFreshIngredientCount = freshIngredientsRanges.Sum(x => x.Length);

		return (freshIngredientCount, totalFreshIngredientCount);
	}

	private static bool IsIngredientFresh(long ingredient, List<Range> ranges)
	{
		// FYI: 70 microseconds with this solution
		// it could be improved with a binary search since ranges are sorted - 40 microseconds
		// but for this input size it's not necessary
		return ranges.Any(range => ingredient >= range.From && ingredient <= range.To);
	}

	private static void MergeRanges(List<Range> freshIngredientsRanges)
	{
		for (var i = 0; i < freshIngredientsRanges.Count - 1; i++)
		{
			var current = freshIngredientsRanges[i];
			var next = freshIngredientsRanges[i + 1];
			if (current.To + 1 >= next.From)
			{
				var merged = new Range(current.From, Math.Max(current.To, next.To));
				freshIngredientsRanges[i] = merged;
				freshIngredientsRanges.RemoveAt(i + 1);
				i--;
			}
		}
	}

	private record Range(long From, long To)
	{
		public long Length => To - From + 1;

		// Some benchmarking just for my curiosity
		// |  Method        | Mean     | Error    | StdDev   | Gen0    | Gen1   | Allocated |
		// | --------------:|---------:|---------:|---------:|--------:|-------:|----------:|
		// | Split          | 72.16 us | 1.394 us | 1.659 us | 17.8223 | 0.7324 | 146.06 KB |
		// | IndexOf        | 71.21 us | 1.390 us | 3.277 us | 16.9678 | 0.7324 | 138.95 KB |
		// | IndexOf + Span |          |          |          | 14.5264 | 0.6104 | 119.08 KB |
		public static Range Parse(string line)
		{
			var separator = line.IndexOf('-');
			var from = long.Parse(line[..separator]);
			var to = long.Parse(line[(separator + 1)..]);
			return new Range(from, to);
		}

		public override string ToString() => $"{From:N}-{To:N}";
	}
}