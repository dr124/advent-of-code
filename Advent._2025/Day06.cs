namespace Advent._2025;

[AocData("Day06.txt", part1: 5873191732773L, part2: 11386445308378L)]
[AocData("Day06Example.txt", part1: 4277556L, part2: 3263827L)]
public class Day06 : Day
{
	public override (object? PartA, object? PartB) Run(string[] lines)
	{
		var numberRanges = FindOperationRanges(lines[^1]);
		var problems = numberRanges.Select(range =>
		{
			var numbers = lines[..^1].Select(x => x[range]).ToArray();
			var operation = lines[^1][range.Start];
			return new MathProblem(numbers, operation);
		}).ToList();

		return (
			problems.Sum(x => x.TaskA()),
			problems.Sum(x => x.TaskB())
		);
	}

	private static Range[] FindOperationRanges(string line)
	{
		var indexes = FindOperationStarts(line);
		var numberRanges = new Range[indexes.Count];
		for (var i = 0; i < indexes.Count - 1; i++)
		{
			var current = indexes[i];
			var next = indexes[i + 1];
			numberRanges[i] = current..(next - 1);
		}
		numberRanges[^1] = indexes[^1]..; // to the end

		return numberRanges;
	}

	private static List<int> FindOperationStarts(string line)
	{
		List<int> newOperationsIndexes = [];
		for (var i = 0; i < line.Length; i++)
		{
			if (line[i] != ' ')
			{
				newOperationsIndexes.Add(i);
			}
		}

		return newOperationsIndexes;
	}

	private record MathProblem(string[] Numbers, char Operation)
	{
		public long TaskA() => ExecuteOperation(Numbers.Select(long.Parse).ToArray());

		public long TaskB() => ExecuteOperation(CephalopodTransposition(Numbers));

		private long ExecuteOperation(long[] numbers)
		{
			return Operation switch
			{
				'+' => numbers.Sum(),
				'*' => numbers.Aggregate(1L, (a, b) => a * b),
				_ => throw new InvalidOperationException($"Unknown operation '{Operation}'")
			};
		}
	}

	public static long[] CephalopodTransposition(string[] numbers)
	{
		var maxLength = numbers.Max(x => x.Length);
		var expected = new long[maxLength];

		foreach (var current in numbers)
		{
			for(var j = 0; j < current.Length; j++)
			{
				var c = current[j];
				if (c == ' ')
				{
					continue;
				}

				expected[j] *= 10;
				expected[j] += current[j] - '0';
			}
		}

		return expected;
	}

	[Theory]
	[InlineData("64 ,23 ,314", "4,431,623")]
	[InlineData(" 51,387,215", "175,581,32")]
	[InlineData("328,64 ,98 ", "8,248,369")]
	[InlineData("123, 45,  6", "356,24,1")]
	public void TestUltraTransposition(string rows, string expected)
	{
		var rowsRaw = rows.Split(',');
		var expectedRaw = expected.Split(',').Select(long.Parse).ToArray();

		var result = CephalopodTransposition(rowsRaw).ToList();
		result.ShouldBe(expectedRaw, ignoreOrder: true);
	}
}