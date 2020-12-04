using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Advent.Core;

namespace Advent._2020.Week1
{
    public class Day4 : Day<Dictionary<string, string>[], int>
    {
        protected override Dictionary<string, string>[] ReadData()
        {
            var str = File.ReadAllText("Week1/input4.txt");

            var str_split = str.Replace("\r", "")
                .Replace("\n\n", "@")
                .Replace("\n", " ")
                .Replace("  ", " ")
                .Split("@", StringSplitOptions.RemoveEmptyEntries);

            var dict = str_split
                .Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries))
                .Select(x => x
                    .Select(field => field.Split(":"))
                    .ToDictionary(
                        f => f[0].ToLower(),
                        f => f[1].ToLower()))
                .ToArray();

            return dict;
        }

        protected override int TaskA()
        {
            return Input.Count(IsValidA);
        }

        protected override int TaskB()
        {
            return Input.Count(IsValidB);
        }

        // ========================================

        private bool IsValidA(IDictionary<string, string> dict)
        {
            var requiredFields = new[] {"byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"};

            return requiredFields.All(dict.ContainsKey);
        }

        private bool IsValidB(IDictionary<string, string> dict)
        {
            var colors = new[] {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"};

            return IsValidA(dict)
                   && IsIntBetween(dict["byr"], 1920, 2002)
                   && IsIntBetween(dict["iyr"], 2010, 2020)
                   && IsIntBetween(dict["eyr"], 2020, 2030)
                   && Regex.IsMatch(dict["hcl"], "^#[a-f0-9]{6}$")
                   && Regex.IsMatch(dict["pid"], "^[0-9]{9}$")
                   && colors.Contains(dict["ecl"])
                   && IsHeightValid(dict["hgt"]);
        }

        private bool IsHeightValid(string hgt)
        {
            if (hgt.Contains("cm"))
                return IsIntBetween(hgt.Replace("cm", ""), 150, 193);
            if (hgt.Contains("in"))
                return IsIntBetween(hgt.Replace("in", ""), 59, 76);
            return false;
        }

        private bool IsIntBetween(string str, int min, int max)
        {
            return int.TryParse(str, out var val) && val >= min && val <= max;
        }
    }
}