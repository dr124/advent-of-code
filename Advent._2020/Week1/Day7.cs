using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent.Core_2019_2020;

// ReSharper disable StringIndexOfIsCultureSpecific.1
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Advent._2020.Week1;

public class Day7 : Day<Dictionary<string, Day7.Bag>, int>
{
    private const string ShinyGoldBag = "shiny gold bag";

    protected override Dictionary<string, Day7.Bag> ReadData() =>
        File.ReadAllLines("Week1/input7.txt")
            .Select(Bag.Create)
            .ToDictionary(x => x.Name, x => x);

    protected override int TaskA() => Input.Count(x => ContainsGold(x.Value));

    protected override int TaskB() => CountBags(Input[ShinyGoldBag]);

    public int CountBags(Bag bag) =>
        bag.Content
            .Where(x => x.name != ShinyGoldBag)
            .Sum(x => x.count * (1 + CountBags(Input[x.name])));

    public bool ContainsGold(Bag bag) =>
        bag.Content.Any(bagg => bagg.name == ShinyGoldBag)
        || bag.Content.Any(x => ContainsGold(Input[x.name]));

    public class Bag
    {
        public string Name { get; set; }
        public List<(string name, int count)> Content { get; set; }

        public static Bag Create(string text)
        {
            var t = text.Replace("bags", "bag")
                .Remove("\\.")
                .Split("contain");

            return new Bag
            {
                Name = t[0].Trim(),
                Content = GetContents(t[1].Trim())
            };
        }

        private static List<(string name, int count)> GetContents(string str)
        {
            if (str == "no other bag")
                return new();

            return str.Split(",")
                .Select(x =>
                {
                    var text = x.Trim();
                    var space = text.IndexOf(" ");
                    var count = int.Parse(text[..space]);
                    var name = text[(space + 1)..];
                    return (name, count);
                }).ToList();
        }
    }
}