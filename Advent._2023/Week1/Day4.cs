using System.Diagnostics;

namespace Advent._2023.Week1;

public class Day4(string[] input) : IDay
{
    private readonly Card[] _cards = input.Select(ParseCard).ToArray();

    public object Part1() => _cards.Sum(CalculatePoints);

    public object Part2()
    {
        var count = new int[_cards.Length];
        for (var i = 0; i < _cards.Length; i++)
        {
            count[i]++; // initial card count
            for (var j = 0; j < _cards[i].Found; j++) //  no need to check for upper cap (&& i + j < _cards.Length)
            {
                count[i + j + 1] += count[i];
            }
        }
        return count.Sum();
    }

    private static int CalculatePoints(Card card) => card.Found == 0 ? 0 : 1 << (card.Found - 1);

    private static Card ParseCard(string line)
    {
        var parts = line.Split(':', '|'); // id is not needed
        var winning = parts[1].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var having = parts[2].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        return new Card([.. winning.Select(int.Parse)],[.. having.Select(int.Parse)]);
    }

    private record Card(int[] Winning, int[] Having)
    {
        public int Found { get; } = Winning.Count(Having.Contains);
    }
}