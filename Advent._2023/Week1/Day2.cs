namespace Advent._2023.Week1;

public class Day2(string[] input) : IDay
{
    private readonly Game[] _games = input.Select(ParseGame).ToArray();

    public object Part1() => _games.Where(IsValidGame).Select(x => x.Id).Sum();
    public object Part2() => _games.Select(MinimumRgbPower).Sum();

    private bool IsValidGame(Game game) =>
        game.Sets.All(x => x is { Red: <= 12, Green: <= 13, Blue: <= 14 });
    
    private static int MinimumRgbPower(Game game) => 
        game.Sets.Max(y => y.Red) * game.Sets.Max(y => y.Green) * game.Sets.Max(y => y.Blue);

    private static Game ParseGame(string line)
    {
        var parts = line.Split(':', StringSplitOptions.TrimEntries);
        var gameId = parts[0].Split(' ', StringSplitOptions.TrimEntries)[^1];
        var sets = parts[1].Split(';', StringSplitOptions.TrimEntries);
        var bagParts = sets
            .Select(set =>
            {
                var bagPart = set.Split(new[] { ',', ' ' },
                    StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToArray();
                int red = 0;
                int green = 0;
                int blue = 0;

                for (int i = 0; i < bagPart.Length; i += 2)
                {
                    if (bagPart[i + 1][0] == 'b')
                        blue = int.Parse(bagPart[i]);
                    else if (bagPart[i + 1][0] == 'r')
                        red = int.Parse(bagPart[i]);
                    else if (bagPart[i + 1][0] == 'g')
                        green = int.Parse(bagPart[i]);
                }

                return new BagPart(red, green, blue);
            })
            .ToArray();

        return new Game(int.Parse(gameId), bagParts);
    }

    private record Game(int Id, BagPart[] Sets);
    private record BagPart(int Red, int Green, int Blue);
}