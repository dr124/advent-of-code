using Advent.Core.Extensions;

namespace Advent._2022.Week3;

public class Day19 : IReadInputDay
{
    private Blueprint[] _input;
    private const int N = 1000000;
    private const float Probability = 0.1f;

    public void ReadData()
    {
        _input = File.ReadLines("Week3/Day19.txt")
            .Select(StringUtils.ExtractNumbers)
            .Select(x => new Blueprint(x[0], x[1], x[2], x[3], x[4], x[5], x[6]))
            .ToArray();
    }

    public object TaskA() => _input.Select(CalculateBlueprintA).Sum();
    public object TaskB() => _input.Take(3).Select(CalculateBlueprintB).Product();

    private int CalculateBlueprintA(Blueprint bp) => bp.Id * CalculateMaximumProduction(bp, 24);
    private int CalculateBlueprintB(Blueprint bp) => CalculateMaximumProduction(bp, 32);

    private int CalculateMaximumProduction(Blueprint bp, int t) =>
        Enumerable.Range(0, N)
            .AsParallel()
            .Select(i => Production(new Random(i), bp, t))
            .Max();

    private static int Production(Random rnd, Blueprint bp, int max)
    {
        int oreR = 1, clayR = 0, obsR = 0, geoR = 0;
        int ore = 0, clay = 0, obs = 0, geo = 0;

        for (var minute = 0; minute < max; minute++)
        {
            var wait = rnd.NextSingle() < Probability;
            var skipGeoRobot = rnd.NextSingle() < Probability;
            var skipObsRobot = rnd.NextSingle() < Probability;
            var skipClayRobot = rnd.NextSingle() < Probability;

            ore += oreR;
            clay += clayR;
            obs += obsR;
            geo += geoR;

            if (wait)
                continue;

            // if can make geo robot
            if (!skipGeoRobot && obs - obsR >= bp.GeodeRobotObsidianCost && ore - oreR >= bp.GeodeRobotOreCost)
            {
                obs -= bp.GeodeRobotObsidianCost;
                ore -= bp.GeodeRobotOreCost;
                geoR += 1;
            }
            //if can make obs robot
            else if (!skipObsRobot && clay - clayR >= bp.ObsidianRobotClayCost && ore - oreR >= bp.ObsidianRobotOreCost)
            {
                clay -= bp.ObsidianRobotClayCost;
                ore -= bp.ObsidianRobotOreCost;
                obsR += 1;
            }
            // if can make clay robot
            else if (!skipClayRobot && ore - oreR >= bp.ClayRobotOreCost)
            {
                ore -= bp.ClayRobotOreCost;
                clayR += 1;
            }
            // if can make ore robot
            else if (ore - oreR >= bp.OreRobotOreCost)
            {
                ore -= bp.OreRobotOreCost;
                oreR += 1;
            }
        }

        return geo;
    }

    private record Blueprint(int Id, int OreRobotOreCost, int ClayRobotOreCost, int ObsidianRobotOreCost,
        int ObsidianRobotClayCost, int GeodeRobotOreCost, int GeodeRobotObsidianCost);
}