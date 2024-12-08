namespace Advent._2024.Week2;

public class Day8(string[] input) : IDay
{
    private readonly Antenna[] _antennas = Extensions.ReadInput(input, c => c != '.')
        .Select(vec => new Antenna(vec, vec.On(input)))
        .ToArray();
        
    public object Part1()
    {
        HashSet<Vec2> antinodes = [];
        foreach (var (a1, a2) in EnumeratePairs())
        {
            var diff = a2 - a1;
            
            var an1 = a1 - diff;
            if(an1.IsInBounds(input))
            {
                antinodes.Add(an1);
            }

            var an2 = a2 + diff;
            if(an2.IsInBounds(input))
            {
                antinodes.Add(an2);
            }
        }

        return antinodes.Count;
    }

    public object Part2()
    {
        HashSet<Vec2> antinodes = [];
        foreach (var (a1, a2) in EnumeratePairs())
        {
            antinodes.Add(a1);
            antinodes.Add(a2);
            
            var diff = a2 - a1;
            foreach (var antiNode in GetAntiNodes(a1, -diff))
            {
                antinodes.Add(antiNode);
            }

            foreach (var antiNode in GetAntiNodes(a2, diff))
            {
                antinodes.Add(antiNode);
            }
        }

        
        return antinodes.Count;
    }

    private IEnumerable<Vec2> GetAntiNodes(Vec2 start, Vec2 step)
    {
        var v = start + step;
        while (v.IsInBounds(input))
        {
            yield return v;
            v += step;
        }
    } 
    
    private IEnumerable<(Vec2 a1, Vec2 a2)> EnumeratePairs() =>
        from gr in _antennas.GroupBy(x => x.Frequency)
        where gr.Count() > 1
        from a1 in gr
        from a2 in gr
        where a1 != a2
        select (a1.Position, a2.Position);

    private record Antenna(Vec2 Position, char Frequency);
}