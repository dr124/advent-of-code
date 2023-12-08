using System;

namespace Advent._2023.Week1;

public class Day7(string[] input) : IDay
{
    private const char Joker = '#'; // less than 2 in ascii
    
    public object Part1() => input
        .Select(ParseHand)
        .Order()
        .Select((h, i) => h.Bid * (i + 1))
        .Sum();

    public object Part2() => input
        .Select(line => line.Replace('J', Joker)) 
        .Select(ParseHand)
        .Order()
        .Select((h, i) => h.Bid * (i + 1))
        .Sum();

    private static Hand ParseHand(string line)
    {
        var x = line.Split(' ');
        var cards = x[0]
            .Replace('T', 'a')
            .Replace('J', 'b')
            .Replace('Q', 'c')
            .Replace('K', 'd')
            .Replace('A', 'e');
        var bet = int.Parse(x[1]);
        return new Hand(cards, bet);
    }

    private record Hand(string Cards, int Bid) : IComparable<Hand>
    {
        private readonly HandType _type = GetType(Cards);
        
        public int CompareTo(Hand? other)
        {
            var typeComparison = _type.CompareTo(other._type);
            return typeComparison != 0
                ? typeComparison 
                : string.Compare(Cards, other.Cards);
        }

        private static HandType GetType(string cards)
        {
            var dict = cards.GroupBy(c => c).ToDictionary(c => c.Key, c => c.Count());
            dict.Remove(Joker, out var jokers);

            if (dict.Count is 0 || dict.Any(x => x.Value + jokers == 5))
                return HandType.FiveOfAKind;

            if (dict.Values.Any(x => x + jokers == 4))
                return HandType.FourOfAKind;

            var three = dict.FirstOrDefault(x => x.Value + jokers == 3).Key;
            if (three != default)
            {
                return dict.Any(x => x.Value == 2 && x.Key != three)
                    ? HandType.FullHouse
                    : HandType.ThreeOfAKind;
            }

            if (dict.Values.Count(x => x == 2) == 2)
                return HandType.TwoPairs;

            if (dict.Values.Any(x => x + jokers == 2))
                return HandType.OnePair;

            return HandType.HighCard;
        }
    }

    private enum HandType
    {
        HighCard,
        OnePair,
        TwoPairs,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind
    }
}