using System;
using System.IO;
using System.Linq;
using Advent.Core;

namespace Advent._2020.Week1
{
    public class Day5 : Day<Day5.Seat[], int>
    {
        protected override Seat[] ReadData()
        {
            return File.ReadAllLines("Week1/input5.txt")
                .Select(Seat.Create)
                .ToArray();
        }

        protected override int TaskA()
        {
            return Input.Max(x => x.SeatId);
        }

        protected override int TaskB()
        {
            var seats = Input.OrderBy(x => x.SeatId).ToList();
            for (var i = 1; i < seats.Count; i++)
                if (seats[i].SeatId - seats[i - 1].SeatId != 1)
                    return seats[i - 1].SeatId + 1;

            return -1;
        }

        public record Seat(int Row, int Col)
        {
            public int SeatId => Row * 8 + Col;

            public static Seat Create(string str)
            {
                return new (
                    BinarySearch(str[..7], 'F', 'B'),
                    BinarySearch(str[^3..], 'L', 'R'));

                static int BinarySearch(string str, char l, char r)
                {
                    var from = 0;
                    var to = (1 << str.Length) - 1;
                    foreach (var t in str)
                    {
                        var mid = (to + from) / 2;
                        if (t == l)
                            to = mid;
                        else if (t == r)
                            from = mid + 1;
                    }

                    return to;
                }
            }
        }
    }
}