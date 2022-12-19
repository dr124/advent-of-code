using Advent.Core;
using Advent.Core.Extensions;
using Microsoft.Diagnostics.Tracing.Parsers.AspNet;
using Random = System.Random;

namespace Advent._2022.Week3;

public class Day19 : IReadInputDay
{
    private Blueprint[] _input;

    private record Blueprint(int Id, int OreRobotOreCost, int ClayRobotOreCost, int ObsidianRobotOreCost, int ObsidianRobotClayCost, int GeodeRobotOreCost, int GeodeRobotObsidianCost)
    {

        public static Blueprint Create(string line)
        {
            var parts = line.ExtractNumbers();
            return new Blueprint(parts[0], parts[1], parts[2], parts[3], parts[4], parts[5], parts[6]);
        }
    }

    public void ReadData()
    {
        _input = File.ReadLines("Week3/Day19.txt")
            .Select(Blueprint.Create)
            .ToArray();

    }

    public object? TaskA()
    {
        var xd = _input.Take(3).Select(CalculateMaximumProduction).Product();

        return xd;
    }

    private int CalculateMaximumProduction(Blueprint blueprint)
    {
        var max = Enumerable.Range(0, 1000000).AsParallel()
            .Select(i => Production(new Random(i), blueprint)).Max();
        Console.WriteLine(max);
        return max;
    }

    const int max = 32;

    private int Production(Random rnd, Blueprint bp)
    {
        var minute = 0;
        int oreR = 1, clayR = 0, obsR = 0, geoR = 0;
        int ore = 0, clay = 0, obs = 0, geo = 0;
        
        while (true)
        {
            if (minute >= max)
            {
                return geo;
            }

            var wait = rnd.NextSingle() < 0.2f;
            var skipRobot1 = rnd.NextSingle() < 0.2f;
            var skipRobot2 = rnd.NextSingle() < 0.2f;
            var skipRobot3 = rnd.NextSingle() < 0.2f;

            // if can make geo robot
            if (!wait && obs >= bp.GeodeRobotObsidianCost && ore >= bp.GeodeRobotOreCost && !skipRobot1)
            {
                obs -= bp.GeodeRobotObsidianCost;
                ore -= bp.GeodeRobotOreCost;
                ore += oreR;
                clay += clayR;
                obs += obsR;
                geo += geoR;
                
                geoR += 1;
            }
            //if can make obs robot
            else if (!wait && clay >= bp.ObsidianRobotClayCost && ore >= bp.ObsidianRobotOreCost && !skipRobot2)
            {
                clay -= bp.ObsidianRobotClayCost;
                ore -= bp.ObsidianRobotOreCost;
                ore += oreR;
                clay += clayR;
                obs += obsR;
                geo += geoR;
                
                obsR += 1;
            }
            // if can make clay robot
            else if (!wait && ore >= bp.ClayRobotOreCost && !skipRobot3)
            {
                ore -= bp.ClayRobotOreCost;
                ore += oreR;
                clay += clayR;
                obs += obsR;
                geo += geoR;
                clayR += 1;
            }
            // if can make ore robot
            else if (!wait && ore >= bp.OreRobotOreCost)
            {
                ore -= bp.OreRobotOreCost;
                ore += oreR;
                clay += clayR;
                obs += obsR;
                geo += geoR;
                oreR += 1;
            }
            else
            {
                ore += oreR;
                clay += clayR;
                obs += obsR;
                geo += geoR;
            }

            minute += 1;
        }
    }

    public object? TaskB()
    {
        return null;
    }

    private record Robot(Mineral Type);

    private enum Mineral
    {
        Ore,
        Clay,
        Obsidian,
        Geode
    }
}