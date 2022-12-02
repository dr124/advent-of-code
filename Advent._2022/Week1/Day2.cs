using Advent.Core;

namespace Advent._2022.Week1;

public class Day2 : IReadInputDay
{
    private List<(Shape opponent, Shape me)> _input;
    public void ReadData()
    {
        _input = File.ReadAllLines("Week1/Day2.txt")
            .Select(x => ((Shape)(x[0]-'A'), (Shape)(x[2]-'X')))
            .ToList();
    }
    
    public object? TaskA() => _input
            .Select(x => RoundResult(x.opponent, x.me) + ShapeScore(x.me))
            .Sum();

    public object? TaskB() => _input
            .Select(x => (x.opponent, me: FromRoundResult(x.opponent, (Result)x.me)))
            .Select(x => RoundResult(x.opponent, x.me) + ShapeScore(x.me))
            .Sum();
    
    
    private static int RoundResult(Shape opponent, Shape me)
    {
        return (opponent, me) switch
            {
                (Shape.Rock, Shape.Paper) => Result.Win,
                (Shape.Rock, Shape.Scissors) => Result.Loss,
                (Shape.Paper, Shape.Rock) => Result.Loss,
                (Shape.Paper, Shape.Scissors) => Result.Win,
                (Shape.Scissors, Shape.Rock) => Result.Win,
                (Shape.Scissors, Shape.Paper) => Result.Loss,
                (_, _) x when x.opponent == x.me => Result.Draw,
            } switch
            {
                Result.Win => 6,
                Result.Draw => 3,
                Result.Loss => 0,
            };
    }
    
    private static Shape FromRoundResult(Shape opponent, Result result)
    {
        return (opponent, result) switch
        {
            (Shape.Rock, Result.Win) => Shape.Paper,
            (Shape.Rock, Result.Loss) => Shape.Scissors,
            (Shape.Paper, Result.Loss) => Shape.Rock,
            (Shape.Paper, Result.Win) => Shape.Scissors,
            (Shape.Scissors, Result.Win) => Shape.Rock,
            (Shape.Scissors, Result.Loss) => Shape.Paper,
            (_, Result.Draw) x => opponent
        };
    }

    private static int ShapeScore(Shape me) => (int)me + 1;

    private enum Shape
    {
        Rock = 0,
        Paper = 1,
        Scissors = 2,
    }

    private enum Result
    {
        Loss = 0,
        Draw = 1,
        Win = 2
    }
}