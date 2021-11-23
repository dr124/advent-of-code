using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Advent.Core_2019_2020;

namespace Advent._2020.Week1;

public class Day2 : Day<Day2.PasswordPolicy[], int>
{
    public record PasswordPolicy(int From, int To, char Letter, string Password);

    protected override PasswordPolicy[] ReadData()
    {
        return File.ReadAllLines("Week1/input2.txt")
            .Select(str =>
            {
                var elements = Regex.Split(str, @"-|\s|:\s");
                return new PasswordPolicy(
                    int.Parse(elements[0]),
                    int.Parse(elements[1]),
                    elements[2][0],
                    elements[3]);
            })
            .ToArray();
    }

    protected override int TaskA()
    {
        return this.Input.Count(p =>
        {
            var sum = p.Password.Count(x => x == p.Letter);
            return sum >= p.From && sum <= p.To;
        });
    }

    protected override int TaskB()
    {
        return this.Input.Count(p => (p.Password[p.From - 1] == p.Letter) ^ (p.Password[p.To - 1] == p.Letter));
    }

}