using Advent.Core.Extensions;

namespace Advent._2022.Week3;

public class Day19 : IReadInputDay
{
    private Blueprint[] _input;

    public void ReadData()
    {
        _input = File.ReadLines("Week3/Day19.txt")
            .Select(Blueprint.Create)
            .ToArray();
    }

    public object? TaskA() => null;//_input.Take(3).Select(CalculateMaximumProduction).Product();

    public object? TaskB() => _input.Take(3).Select(CalculateMaximumProduction).Product();

    private int CalculateMaximumProduction(Blueprint blueprint)
    {
        var max = Enumerable.Range(0, 2000000).AsParallel()
            .Select(i => Production(new Random(i), blueprint, 32)).Max();
        Console.WriteLine(max);
        return max;
    }


    private int Production(Random rnd, Blueprint bp, int max)
    {
        var minute = 0;
        int oreR = 1, clayR = 0, obsR = 0, geoR = 0;
        int ore = 0, clay = 0, obs = 0, geo = 0;

        while (true)
        {
            if (minute >= max) return geo;
            var wait = rnd.NextSingle() < 0.1f;
            var skipGeoRobot = rnd.NextSingle() < 0.1f;
            var skipObsRobot = rnd.NextSingle() < 0.1f;
            var skipClayRobot = rnd.NextSingle() < 0.1f;

            ore += oreR;
            clay += clayR;
            obs += obsR;
            geo += geoR;

            // if can make geo robot
            if (!wait && !skipGeoRobot && obs - obsR >= bp.GeodeRobotObsidianCost && ore - oreR >= bp.GeodeRobotOreCost)
            {
                obs -= bp.GeodeRobotObsidianCost;
                ore -= bp.GeodeRobotOreCost;
                geoR += 1;
            }
            //if can make obs robot
            else if (!wait && !skipObsRobot && clay - clayR >= bp.ObsidianRobotClayCost &&
                     ore - oreR >= bp.ObsidianRobotOreCost)
            {
                clay -= bp.ObsidianRobotClayCost;
                ore -= bp.ObsidianRobotOreCost;
                obsR += 1;
            }
            // if can make clay robot
            else if (!wait && !skipClayRobot && ore - oreR >= bp.ClayRobotOreCost)
            {
                ore -= bp.ClayRobotOreCost;
                clayR += 1;
            }
            // if can make ore robot
            else if (!wait && ore - oreR >= bp.OreRobotOreCost)
            {
                ore -= bp.OreRobotOreCost;
                oreR += 1;
            }

            minute += 1;
        }
    }

    private record Blueprint(int Id, int OreRobotOreCost, int ClayRobotOreCost, int ObsidianRobotOreCost,
        int ObsidianRobotClayCost, int GeodeRobotOreCost, int GeodeRobotObsidianCost)
    {
        public static Blueprint Create(string line)
        {
            var parts = line.ExtractNumbers();
            return new Blueprint(parts[0], parts[1], parts[2], parts[3], parts[4], parts[5], parts[6]);
        }
    }
}