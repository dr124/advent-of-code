using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Advent.Core;
using MoreLinq;

namespace Advent._2020.Week3
{
    public class Day17 : Day<Day17.Map, int>
    {
        protected override Map ReadData()
        {
            var lines = File.ReadAllLines("Week3/Input17.txt");

            var map = new Map();
            List<(int x, int y, int z)> v = new();
            for (var y = 0; y < lines.Length; y++)
            for (var x = 0; x < lines[y].Length; x++)
                if (lines[y][x] == '#')
                    map[(x, y, 0)] = 1;
            return map;
        }

        protected override int TaskA()
        {
            var map = new Map(Input);

            for(int i = 0; i < 6; i++)
                map.Mutate();

            return -1;
        }


        private int Neighbours(Map map, (int x, int y, int z) pt)
        {
            return map[(pt.x + 1, pt.y, pt.z)]
                   + map[(pt.x - 1, pt.y, pt.z)]
                   + map[(pt.x, pt.y + 1, pt.z)]
                   + map[(pt.x, pt.y - 1, pt.z)]
                   + map[(pt.x, pt.y, pt.z + 1)]
                   + map[(pt.x, pt.y, pt.z - 1)];
        }

        protected override int TaskB()
        {
            return -1;
        }

        public class Map
        {
            public Map()
            {
                N = 100;
                Z = 100;
                
            }

            public Map(Map otherMap) : base()
            {
                HashMap = otherMap.HashMap;
                Z = otherMap.Z;
                N = otherMap.N;
            }

            private HashSet<(int x, int y, int z)> HashMap { get; init; } = new();
            public int N { get; set; } = 3;
            public int Z { get; set; } = 1;
            
            public int this[(int x, int y, int z) pt]
            {
                get => HashMap.Contains(pt) ? 1 : 0;
                set
                {
                    if (HashMap.Contains(pt))
                    {
                        if(value == 0)
                            HashMap.Remove(pt);
                    }
                    else
                    {
                        if (value == 1)
                            HashMap.Add(pt);
                    }

                }
            }

            public bool this[int x, int y, int z]
            {
                get => this[(x, y, z)] == 1;
                set => this[(x, y, z)] = value ? 1 : 0;
            }


            public void Mutate(bool expand = true)
            {
                var prevMap = new Map {HashMap = HashMap.ToHashSet()};

                foreach (var z in EnumerateZ())
                foreach (var y in EnumerateN())
                foreach (var x in EnumerateN())
                {
                    var isActive = prevMap[x, y, z];
                    var ne = prevMap.Neighbours(x, y, z);
                    if (isActive)
                    {
                        this[x, y, z] = ne is 2 or 3;
                    }
                    else
                    {
                        this[x, y, z] = ne is 3;
                    }
                }
                
                Console.WriteLine(HashMap.Count);
            }


            private IEnumerable<int> EnumerateZ()
            {
                for (var z = Z; z <= Z; z++)
                    yield return z;
            }

            private IEnumerable<int> EnumerateN()
            {
                for (var n = 1 - N; n <= 1 + N ; n++)
                    yield return n;
            }

            private int Neighbours(int x, int y, int z)
            {
                var sum = 0;
                for (var i = -1; i <= 1; i++)
                for (var j = -1; j <= 1; j++)
                for (var k = -1; k <= 1; k++)
                    if (i == 0 && j == 0 && k == 0)
                        continue;
                    else
                        sum += this[(x + i, y + j, z + k)];

                return sum;
            }
            
            public void DebugDraw()
            {
                foreach (var z in EnumerateZ())
                {
                    Console.WriteLine($"Z = {z}: ");
                    foreach (var y in EnumerateN())
                    {
                        foreach (var x in EnumerateN())
                            Console.Write(this[x, y, z] ? "#" : ".");
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}