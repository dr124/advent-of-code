using Shouldly;
using Xunit;

namespace Advent._2025;

[AocData("Day02.txt", part1: 29940924880L, part2: 48631958998L)]
[AocData("Day02Example.txt", part1: 1227775554L, part2: 4174379265L)]
public class Day02 : Day
{
	public override (object? PartA, object? PartB) Run(string[] lines)
	{
		var ranges = lines[0].Split([',', '-'])
			.Select(long.Parse)
			.Chunk(2)
			.Select(x => (from: x[0], to: x[1]))
			.ToList();

		var invalid = 0L;
		var extraInvalid = 0L;
		
		foreach (var (from, to) in ranges)
		{
			for (var i = from; i <= to; i++)
			{
				var number = i.ToString();
				if (IsInvalid(number))
				{
					invalid += i;
				}

				if (IsExtraInvalid(number))
				{
					extraInvalid += i;
				}
			}
		}

		return (invalid, extraInvalid);
	}

	private static bool IsInvalid(ReadOnlySpan<char> number)
	{
		var len = number.Length;
		if (len > 2 && len % 2 != 0)
		{
			return false;
		}

		var left = number[..(len / 2)];
		var right = number[(len / 2)..];

		return left.SequenceCompareTo(right) == 0;
	}

	private static bool IsExtraInvalid(ReadOnlySpan<char> number)
	{
		for (var i = number.Length / 2; i >= 1; i--)
		{
			if (DoesRepeat(number[..i], number[i..]))
			{
				return true;
			}
		}

		return false;
	}

	private static bool DoesRepeat(ReadOnlySpan<char> left, ReadOnlySpan<char> right)
	{
		while (true)
		{
			var lenL = left.Length;
			var lenR = right.Length;

			if (lenL > lenR || lenR % lenL != 0)
			{
				return false;
			}

			if (lenL == lenR)
			{
				return left.SequenceCompareTo(right) == 0;
			}

			if (!right.StartsWith(left))
			{
				return false;
			}

			right = right[lenL..];
		}
	}

	[Theory]
	[InlineData("12", "12121212", true)]
	[InlineData("12", "12121213", false)]
	[InlineData("1", "111111", true)]
	[InlineData("1", "111112", false)]
	[InlineData("1234", "1234", true)]
	[InlineData("1234", "12341234", true)]
	[InlineData("1234", "12341235", false)]
	public void TestDoesRepeat(string left, string right, bool expected) => DoesRepeat(left, right).ShouldBe(expected);

	[Theory]
	[InlineData("11", true)]
	[InlineData("12", false)]
	[InlineData("22", true)]
	[InlineData("101", false)]
	[InlineData("111", true)]
	[InlineData("999", true)]
	[InlineData("1001", false)]
	[InlineData("1010", true)]
	[InlineData("1011", false)]
	[InlineData("1188511885", true)]
	[InlineData("1188511886", false)]
	[InlineData("11885118851", false)]
	[InlineData("222222", true)]
	[InlineData("2222223", false)]
	[InlineData("446446446446", true)]
	[InlineData("38593859", true)]
	[InlineData("565656", true)]
	[InlineData("565655", false)]
	[InlineData("824824824", true)]
	[InlineData("824824826", false)]
	[InlineData("2121212121", true)]
	[InlineData("2121212122", false)]
	[InlineData("123412341234", true)]
	[InlineData("123412341233", false)]
	public void IsExtraInvalidTests(string value, bool expected) => IsExtraInvalid(value).ShouldBe(expected);
}