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
                return bingo.SumUnmarked() * number;

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
                    return last.SumUnmarked() * number;
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


    public class Bingo2
    {
        private const int ZERO = 2137;
        public int[,] board = new int[5, 5];
        public bool IsWinner = false;
        public Bingo2(string[] strValues)
        {
            for (int i = 0; i < 5; i++)
            {
                var values = strValues[i].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                for (int j = 0; j < 5; j++)
                {
                    board[i, j] = int.Parse(values[j]);
                    if (board[i, j] == 0)
                        board[i, j] = ZERO;
                }
            }
        }

        public void Mark(int number)
        {
            if (number == 0)
                number = ZERO;

            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    if (board[i, j] == number)
                    {
                        board[i, j] *= (-1);
                        return;
                    }
        }

        public bool IsWinning()
        {
            for (int i = 0; i < 5; i++)
            {
                int bingoY = 0, bingoX = 0;
                for (int j = 0; j < 5; j++)
                {
                    if (board[i, j] < 0)
                        bingoY++;

                    if (board[j, i] < 0)
                        bingoX++;

                    if (bingoY == 5 || bingoX == 5)
                        return true;
                }
            }

            return false;
        }

        public int UnmarkedSum() => (
            from int val
            in board 
            where val >= 0 && val != ZERO select val).Sum();
    }
}