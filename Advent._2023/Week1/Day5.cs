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

        List<Rule> seedToSoilRules = [];
        List<Rule> soilToFertilizerRules = [];
        List<Rule> fertilizerToWaterRules = [];
        List<Rule> waterToLightRules = [];
        List<Rule> lightToTemperatureRules = [];
        List<Rule> temperatureToHumidityRules = [];
        List<Rule> humidityToLocationRules = [];
        
        uint line = 3;
        while (!string.IsNullOrWhiteSpace(lines[line]))
        {
            var rule = Rule.ParseRule(lines[line]);
            seedToSoilRules.Add(rule);
            line++;
        }

        line += 2;
        while (!string.IsNullOrWhiteSpace(lines[line]))
        {
            var rule = Rule.ParseRule(lines[line]);
            soilToFertilizerRules.Add(rule);
            line++;
        }

        line += 2;
        while (!string.IsNullOrWhiteSpace(lines[line]))
        {
            var rule = Rule.ParseRule(lines[line]);
            fertilizerToWaterRules.Add(rule);
            line++;
        }

        line += 2;
        while (!string.IsNullOrWhiteSpace(lines[line]))
        {
            var rule = Rule.ParseRule(lines[line]);
            waterToLightRules.Add(rule);
            line++;
        }

        line += 2;
        while (!string.IsNullOrWhiteSpace(lines[line]))
        {
            var rule = Rule.ParseRule(lines[line]);
            lightToTemperatureRules.Add(rule);
            line++;
        }

        line += 2;
        while (!string.IsNullOrWhiteSpace(lines[line]))
        {
            var rule = Rule.ParseRule(lines[line]);
            temperatureToHumidityRules.Add(rule);
            line++;
        }

        line += 2;
        while (line < lines.Length)
        {
            var rule = Rule.ParseRule(lines[line]);
            humidityToLocationRules.Add(rule);
            line++;
        }

        return new Almanac
        {
            Seeds = seeds,
            SeedToSoilRules = [..seedToSoilRules.OrderBy(x => x.SourceFrom)],
            SoilToFertilizerRules = [..soilToFertilizerRules.OrderBy(x => x.SourceFrom)],
            FertilizerToWaterRules = [..fertilizerToWaterRules.OrderBy(x => x.SourceFrom)],
            WaterToLightRules = [..waterToLightRules.OrderBy(x => x.SourceFrom)],
            LightToTemperatureRules = [..lightToTemperatureRules.OrderBy(x => x.SourceFrom)],
            TemperatureToHumidityRules = [..temperatureToHumidityRules.OrderBy(x => x.SourceFrom)],
            HumidityToLocationRules = [..humidityToLocationRules.OrderBy(x => x.SourceFrom)]
        };
    }

    [DebuggerDisplay("{SourceFrom}..{SourceTo} -> {DestinationFrom}..{DestinationTo}")]
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

        public uint SourceFrom { get; } = source;
        public uint SourceTo { get; } = source + length;

        public uint DestinationFrom { get; } = destination;
        public uint DestinationTo { get; } = destination + length;
        
        public int CompareTo(uint other)
        {
            if (other < SourceFrom) return 1;
            if (other > SourceTo) return -1;   
            return 0;                   
        }

        public uint Apply(uint value)
        {
            return value - SourceFrom + DestinationFrom;
        }
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
