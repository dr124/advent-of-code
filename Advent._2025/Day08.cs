using Xunit.Internal;

namespace Advent._2025;

[AocData("Day08.txt", part1: null, part2: null)]
[AocData("Day08Example.txt", part1: 40, part2: null)]
public class Day08 : Day
{
	public override (object? PartA, object? PartB) Run(string[] lines)
	{
		var points = lines
			.Select(x => x.Split(','))
			.Select(x => new Vec3(int.Parse(x[0]), int.Parse(x[1]), int.Parse(x[2])))
			.ToList();

		var distSquareMap = new double[points.Count, points.Count];

		for (int i = 0; i < points.Count; i++)
		{
			for (int j = 0; j < points.Count; j++)
			{
				if (i == j) continue;
				var dx = (double)points[i].X - (double)points[j].X;
				var dy = (double)points[i].Y - (double)points[j].Y;
				var dz = (double)points[i].Z - (double)points[j].Z;
				distSquareMap[i, j] = dx * dx + dy * dy + dz * dz;
			}
		}

		List<HashSet<Vec3>> strings = [];
		for(int i = 0; i < (points.Count < 100 ? 10 : 1000); i++)
		{
			var closest = FindClosestPair(distSquareMap);
			var pointA = points[closest.A];
			var pointB = points[closest.B];
			// set it as infinite distance
			distSquareMap[closest.A, closest.B] = double.MaxValue;
			distSquareMap[closest.B, closest.A] = double.MaxValue;

			var existingA = strings.FirstOrDefault(x => x.Contains(pointA));
			var existingB = strings.FirstOrDefault(x => x.Contains(pointB));

			if (existingA != null && existingB != null && existingA != existingB)
			{
				strings.Remove(existingB);
				existingA.AddRange(existingB);
			}
			else if (existingA == null && existingB == null)
			{
				existingA = [];
				strings.Add(existingA);
			}
			else
			{
				existingA ??= existingB!;
			}

			existingA.Add(pointA);
			existingA.Add(pointB);
		}

		return (strings.Select(x => x.Count).OrderDescending().Take(3).Aggregate(1L, (acc, x) => acc * x), null);
	}

	private (int A, int B) FindClosestPair(double[,] distSquareMap)
	{
		double minDist = long.MaxValue;
		(int, int) closestPair = (-1, -1);

		for (int i = 0; i < distSquareMap.GetLength(0); i++)
		{
			for (int j = 0; j < distSquareMap.GetLength(1); j++)
			{
				if (i != j && distSquareMap[i, j] < minDist)
				{
					minDist = distSquareMap[i, j];
					closestPair = (i, j);
				}
			}
		}

		return closestPair;
	}
}