using Advent.Core;
using Advent.Core.Extensions;

namespace Advent._2021.Week4;

public class Day22 : IReadInputDay<List<Day22.Cube>>
{
    public List<Cube> Input { get; set; }

    public void ReadData() =>
        Input = (from line in File.ReadAllLines("Week4/Day22.txt")
                 let split = line.Split(new[] { "=", "..", "," }, SplitOptions.Clear)
                 let fr = (int.Parse(split[1]), int.Parse(split[4]), int.Parse(split[7]))
                 let to = (int.Parse(split[2]), int.Parse(split[5]), int.Parse(split[8]))
                 select new Cube(fr, to, line.StartsWith("on")))
            .ToList();


    public object TaskA() => Calculate(Input.Where(IsSmall).ToList());

    public object TaskB() => Calculate(Input);

    private long Calculate(List<Cube> cubes)
    {
        List<Cube> existing = new();
        foreach (var cube in cubes)
        {
            existing.AddRange(existing.Where(cube.Intersects).ToList().Select(x => x.GetIntersection(cube, !x.Lit)));
            if (cube.Lit)
                existing.Add(cube);
        }

        return existing.Sum(x => x.Volume);
    }

    private bool IsSmall(Cube c) => c.From.X.IsBetween(-50, 50)
                                    && c.From.Y.IsBetween(-50, 50)
                                    && c.From.Z.IsBetween(-50, 50)
                                    && c.To.X.IsBetween(-50, 50)
                                    && c.To.Y.IsBetween(-50, 50)
                                    && c.To.Z.IsBetween(-50, 50);

    public record Cube(Vec3 From, Vec3 To, bool Lit)
    {
        public long Volume => (Lit ? 1L : -1L)
                              * (To.X - From.X + 1L)
                              * (To.Y - From.Y + 1L)
                              * (To.Z - From.Z + 1L);

        public bool Intersects(Cube cube) => From.X <= cube.To.X
                                              && From.Y <= cube.To.Y
                                              && From.Z <= cube.To.Z
                                              && To.X >= cube.From.X
                                              && To.Y >= cube.From.Y
                                              && To.Z >= cube.From.Z;

        public Cube GetIntersection(Cube other, bool lit) => new(
            (Math.Max(From.X, other.From.X),
                Math.Max(From.Y, other.From.Y),
                Math.Max(From.Z, other.From.Z)),
            (Math.Min(To.X, other.To.X),
                Math.Min(To.Y, other.To.Y),
                Math.Min(To.Z, other.To.Z)),
            lit);
    }
}