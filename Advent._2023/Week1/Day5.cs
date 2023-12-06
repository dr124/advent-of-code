using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Advent._2023.Week1;

public class Day5(string[] input) : IDay
{
    private Almanac _almanac = ParseInput(input);

    public object Part1()
    {
        var result = _almanac.Seeds.Select(_almanac.Process).ToArray();

        return result.Min();
    }

    public object Part2()
    {
        var seeds = _almanac.Seeds;
        var min = uint.MaxValue;
        var chunks = seeds.Chunk(2).OrderBy(_ => Guid.NewGuid()).ToArray();

        var sw = Stopwatch.StartNew();

        Parallel.ForEach(chunks, x =>
        {
            Parallel.For(0, x[1], i =>
            {
                var result = _almanac.Process((uint)(x[0] + i));
                if (result < min)
                {
                    min = result;
                    Console.WriteLine(min);
                }
            });
        });

        var elapsed = sw.Elapsed;


        Console.WriteLine("#####");
        Console.WriteLine("Finished in {0}ms", elapsed.TotalMilliseconds);
        Console.WriteLine("Result is {0}", min);
        return min;
    }

    // omg this is ugly
    private static Almanac ParseInput(string[] lines)
    {
        var seeds = lines[0]
            .Split(':', StringSplitOptions.TrimEntries)[1]
            .Split(' ')
            .Select(uint.Parse)
            .ToArray();

        var line = 3;
        return new Almanac
        {
            Seeds = seeds,
            SeedToSoilRules = ReadInput(lines, ref line),
            SoilToFertilizerRules = ReadInput(lines, ref line),
            FertilizerToWaterRules = ReadInput(lines, ref line),
            WaterToLightRules = ReadInput(lines, ref line),
            LightToTemperatureRules = ReadInput(lines, ref line),
            TemperatureToHumidityRules = ReadInput(lines, ref line),
            HumidityToLocationRules = ReadInput(lines, ref line)
        };
    }

    private static Rule[] ReadInput(string[] lines, ref int lineIndex)
    {
        var rules = new List<Rule>();
        while (lineIndex < lines.Length && !string.IsNullOrWhiteSpace(lines[lineIndex]))
        {
            rules.Add(Rule.ParseRule(lines[lineIndex++]));
        }

        lineIndex += 2;
        return rules.OrderBy(x => x.From).ToArray();
    }

    private class Rule(uint destination, uint source, uint length) : IComparable<uint>
    {
        public static Rule ParseRule(string rule)
        {
            var parts = rule.Split(' ');
            var destination = uint.Parse(parts[0]);
            var source = uint.Parse(parts[1]);
            var length = uint.Parse(parts[2])-1;
            return new Rule(destination, source, length);
        }

        public int CompareTo(uint other)
        {
            if (other < source) return 1;
            if (other > source + length) return -1;   
            return 0;                   
        }

        public uint From { get; } = source;

        public uint Apply(uint value) => value - source + destination;
    }

    private class Almanac
    {
        public uint[] Seeds { get; init; } = [];
        public Rule[] SeedToSoilRules { get; init; } = [];
        public Rule[] SoilToFertilizerRules { get; init; } = [];
        public Rule[] FertilizerToWaterRules { get; init; } = [];
        public Rule[] WaterToLightRules { get; init; } = [];
        public Rule[] LightToTemperatureRules { get; init; } = [];
        public Rule[] TemperatureToHumidityRules { get; init; } = [];
        public Rule[] HumidityToLocationRules { get; init; } = [];

        public uint Process(uint seed)
        {
            var soil = UseRule(seed, SeedToSoilRules);
            var fert = UseRule(soil, SoilToFertilizerRules);
            var water = UseRule(fert, FertilizerToWaterRules);
            var light = UseRule(water, WaterToLightRules);
            var temp = UseRule(light, LightToTemperatureRules);
            var hum = UseRule(temp, TemperatureToHumidityRules);
            var loc = UseRule(hum, HumidityToLocationRules);
            return loc;
        }

        private uint UseRule(uint value, Rule[] rules)
        {
            // binary search for rule
            int low = 0, high = rules.Length - 1;
            while (low <= high)
            {
                var mid = (low + high) / 2;
                var comparison = rules[mid].CompareTo(value);

                if (comparison == 0)
                {
                    return rules[mid].Apply(value); 
                }
                else if (comparison > 0)
                {
                    high = mid - 1;
                }
                else
                {
                    low = mid + 1;
                }
            }
            
            // not found
            return value;
        }
    }
}
