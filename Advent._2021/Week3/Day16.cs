using Advent.Core;

namespace Advent._2021.Week3;

internal class Day16 : IReadInputDay
{
    private Packet _packet;

    public void ReadData()
    {
        var text = File.ReadAllText("Week3/Day16.txt");
        var bytes = Convert.FromHexString(text);
        var xd = bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')).ToArray();
        var bin = xd.SelectMany(c => c).Select(c => c - '0').ToArray();

        _packet = Packet.ParseNumber(bin, 1, out _)[0];
    }

    public object TaskA() => _packet.VersionSum();
    public object TaskB() => _packet.Operation();

    public class Packet
    {
        public List<Packet>? Packets { get; set; }
        public long? Value { get; set; }
        public int Version { get; set; }
        public int Type { get; set; }

        public static List<Packet> ParseNumber(int[] bin, int maxPackets, out int idx)
        {
            var parsed = new List<Packet>();
            var i = 0;
            while (parsed.Count < maxPackets && i < bin.Length - 5)
            {
                var packet = new Packet
                {
                    Version = bin[i..(i += 3)].ToBitMap(),
                    Type = bin[i..(i += 3)].ToBitMap()
                };

                if (packet.Type == 4)
                {
                    var value = new List<int>();
                    var keepReading = true;
                    while(keepReading)
                    {
                        keepReading = bin[i++] == 1;
                        value.AddRange(bin[i..(i += 4)]);
                    }
                    packet.Value = value.ToBitMapLong();
                }
                else
                {
                    var I = bin[i..(i += 1)].ToBitMap();
                    var l = I == 0 ? 15 : 11;
                    if (I == 0)
                    {
                        var length = bin[i..(i += l)].ToBitMap();
                        packet.Packets = ParseNumber(bin[i..(i += length)], int.MaxValue, out _);
                    }
                    else
                    {
                        var n = bin[i..(i += l)].ToBitMap();
                        packet.Packets = ParseNumber(bin[i..], n, out var readPosition);
                        i += readPosition;
                    }
                }

                parsed.Add(packet);
            }

            idx = i;
            return parsed;
        }

        public int VersionSum() => Version + (Packets?.Sum(x => x.VersionSum()) ?? 0);

        public long Operation()
        {
            var xd = 
            Type switch
            {
                0 => Packets!.Select(x => x.Operation()).Sum(),
                1 => Packets!.Select(x => x.Operation()).Product(),
                2 => Packets!.Select(x => x.Operation()).Min(),
                3 => Packets!.Select(x => x.Operation()).Max(),
                4 => Value!.Value,
                5 => Packets![0].Operation() > Packets[1].Operation() ? 1 : 0,
                6 => Packets![0].Operation() < Packets[1].Operation() ? 1 : 0,
                7 => Packets![0].Operation() == Packets[1].Operation() ? 1 : 0,
                _ => throw new ArgumentException("XD")
            };
            Console.WriteLine(xd);
            return xd;
        }
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
