using Advent.Core;

namespace Advent._2021.Week1;

internal class Day4 : IReadInputDay
{
    public int[] Numbers { get; set; }
    public Bingo[] Bingos { get; set; }

    public void ReadData()
    {
        var file = File.ReadAllLines("Week1/Day4.txt");

        Numbers = file.First()
            .Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(int.Parse)
            .ToArray();

        IEnumerable<Bingo> ReadBingos(string[] lines)
        {
            for (int i = 0; i < lines.Length; i += 6)
                yield return new Bingo(lines[i..(i + 5)]);
        }

        Bingos = ReadBingos(file[2..]).ToArray();
    }

    public object TaskA()
    {
        foreach (var number in Numbers)
        foreach (var bingo in Bingos)
            if (bingo.MarkNumber(number))
                return bingo.SumUnmarked() * number; // 29440

        return null;
    }

    public object TaskB()
    {
        var bingos = Bingos.ToHashSet();
        bingos.RemoveWhere(x => x.IsWinning());

        foreach (var number in Numbers)
        {
            if (bingos.Count == 1)
            {
                var last = bingos.ElementAt(0);
                if (last.MarkNumber(number))
                    return last.SumUnmarked() * number; // 13884
            }

            foreach (var bingo in bingos)
                if (bingo.MarkNumber(number))
                    bingos.Remove(bingo);
        }

        return null;
    }

    public class Bingo
    {
        private const int N = 5;
        private readonly (int p, bool c)[][] _board;
        private readonly Dictionary<int, (int i, int j)> _boardDict;

        public Bingo(string[] board)
        {
            var nums = board
                .Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToArray())
                .ToArray();

            _board = new (int, bool)[N][];
            _boardDict = new();
            for (int i = 0; i < N; i++)
            {
                _board[i] = new (int, bool)[N];
                for (int j = 0; j < N; j++)
                {
                    _board[i][j] = (nums[i][j], false);
                    _boardDict[nums[i][j]] = (i, j);
                }
            }
        }

        public bool MarkNumber(int number)
        {
            if (_boardDict.ContainsKey(number))
            {
                var (i, j) = _boardDict[number];
                if (!_board[i][j].c)
                    _board[i][j].c = true;
            }

            return IsWinning();
        }

        public bool IsWinning()
        {
            for (var i = 0; i < N; i++)
            {
                if (_board[i].All(x => x.c)) return true;
                if (_board.All(x => x[i].c)) return true;
            }
           
            return false;
        }

        public int SumUnmarked() => _board.Sum(x => x.Where(y => !y.c).Sum(y=>y.p));
    }
}