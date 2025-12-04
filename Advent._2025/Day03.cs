namespace Advent._2025;

[AocData("Day03.txt", part1: 17430L, part2: 171975854269367L)]
[AocData("Day03Example.txt", part1: 357L, part2: 3121910778619L)]
public class Day03 : Day
{
	public override (object? PartA, object? PartB) Run(string[] lines) => (
		lines.Select(str => FindMaxJoltage(str, 2)).Sum(),
		lines.Select(str => FindMaxJoltage(str, 12)).Sum());

	private static long FindMaxJoltage(ReadOnlySpan<char> str, int size)
	{
		long value = 0;
		for (var i = 0; i < size; i++)
		{
			var maxChar = '0';
			var maxIndex = -1;
			for (var j = 0; j <= str.Length - size + i; j++)
			{
				if (str[j] > maxChar)
				{
					maxChar = str[j];
					maxIndex = j;
				}
			}

			value = value * 10 + maxChar - '0';
			str = str[(maxIndex + 1)..];
		}

		return value;
	}

	[Theory]
	[InlineData("987654321111111", 2, 98L)]
	[InlineData("811111111111119", 2, 89L)]
	[InlineData("234234234234278", 2, 78L)]
	[InlineData("818181911112111", 2, 92L)]
	public void FindMaxJoltageTest(string str, int size, long expected)
	{
		var result = FindMaxJoltage(str, size);
		result.ShouldBe(expected);
	}
}