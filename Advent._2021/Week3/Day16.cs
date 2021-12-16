using Advent.Core;

namespace Advent._2021.Week3;

internal class Day16 : IReadInputDay
{
    private string Input;
    private Packet _packet;

    public void ReadData()
    {
        Input = File.ReadAllText("Week3/Day16.txt");

        var bytes = Convert.FromHexString(Input);
        var xd = bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')).ToArray();
        var bin = xd.SelectMany(c => c).Select(c => c - '0').ToArray();

        _packet = Packet.ParseNumber(bin, 1, out _)[0];
    }

    public object TaskA() => _packet.VersionSum();

    public object TaskB() => _packet.Op();

    public class Packet
    {
        public List<Packet>? Packets { get; set; }
        public long Value { get; set; }
        public int Version { get; set; }
        public int Type { get; set; }

        public static List<Packet> ParseBits(int[] bin, int bits)
        {
            List<Packet> parsed = new();
            var i = 0;
            while (true)
            {
                if (i >= bits-5) break;

                Packet p = new()
                {
                    Version = bin[i..(i += 3)].ToBitMap(),
                    Type = bin[i..(i += 3)].ToBitMap()
                };
                if (p.Type == 4)
                {
                    List<int> value = new();
                    var keepReading = true;
                    for (; keepReading; i += 5)
                    {
                        keepReading = bin[i] == 1;
                        var x = bin[(i + 1)..(i + 5)];
                        value.AddRange(x);
                    }

                    p.Value = value.ToBitMapLong();
                }
                else
                {
                    var I = bin[i..(i += 1)].ToBitMap();
                    if (I == 0)
                    {
                        var l = 15;
                        var length = bin[i..(i += l)].ToBitMap();
                        p.Packets = Packet.ParseBits(bin[i..(i += length)], length);
                    }
                    else
                    {
                        var l = 11;
                        var number = bin[i..(i += l)].ToBitMap();
                        p.Packets = Packet.ParseNumber(bin[i..], number, out var idx);
                        i += idx;
                    }
                }

                parsed.Add(p);
            }

            return parsed;
        }

        public static List<Packet> ParseNumber(int[] bin, int number, out int idx)
        {
            List<Packet> parsed = new();
            var i = 0;
            while (parsed.Count < number)
            {
                Packet p = new()
                {
                    Version = bin[i..(i += 3)].ToBitMap(),
                    Type = bin[i..(i += 3)].ToBitMap()
                };
                if (p.Type == 4)
                {
                    List<int> value = new();
                    var keepReading = true;
                    for (; keepReading; i += 5)
                    {
                        keepReading = bin[i] == 1;
                        var x = bin[(i + 1)..(i + 5)];
                        value.AddRange(x);
                    }

                    p.Value = value.ToBitMapLong();
                }
                else
                {
                    var I = bin[i..(i += 1)].ToBitMap();
                    if (I == 0)
                    {
                        var l = 15;
                        var length = bin[i..(i += l)].ToBitMap();
                        p.Packets = Packet.ParseBits(bin[i..(i += length)], length);
                    }
                    else
                    {
                        var l = 11;
                        var n = bin[i..(i += l)].ToBitMap();
                        p.Packets = Packet.ParseNumber(bin[i..], n, out var addIdx);
                        i += addIdx;
                    }
                }

                parsed.Add(p);
            }

            idx = i;
            return parsed;
        }

        public int VersionSum() => Version + (Packets?.Sum(x => x.VersionSum()) ?? 0);

        public long Op() =>
            Type switch
            {
                0 => Packets!.Select(x => x.Op()).Sum(),
                1 => Packets!.Select(x => x.Op()).Product(),
                2 => Packets!.Select(x => x.Op()).Min(),
                3 => Packets!.Select(x => x.Op()).Max(),
                4 => Value,
                5 => Packets![0].Op() > Packets[1].Op() ? 1 : 0,
                6 => Packets![0].Op() < Packets[1].Op() ? 1 : 0,
                7 => Packets![0].Op() == Packets[1].Op() ? 1 : 0,
                _ => throw new ArgumentException("XD")
            };
    }
}

internal static class Ext
{
    public static int ToBitMap(this IEnumerable<int> arr)
    {
        int i = 0;
        int sum = 0;
        foreach (var a in arr.Reverse())
        {
            if (a != 0)
                sum += 1 << i;
            i++;
        }

        return sum;
    }

    public static long ToBitMapLong(this IEnumerable<int> arr)
    {
        var i = 0;
        var sum = 0L;
        foreach (var a in arr.Reverse())
        {
            if (a != 0)
                sum += 1L << i;
            i++;
        }

        return sum;
    }

    public static long Product(this IEnumerable<long> arr) => arr.Aggregate(1L, (current, el) => current * el);
}
