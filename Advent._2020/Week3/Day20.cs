using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent.Core_2019_2020;
using Advent.Core_2019_2020.Graphs;

namespace Advent._2020.Week3;

public class Day20 : Day<List<Day20.MapPiece>, long>
{
    private List<List<MapPiece>> _finalmap;

    private int _n;

    protected override List<MapPiece> ReadData()
    {
        var file = File.ReadAllLines("Week3/Input20.txt");

        var map = new List<MapPiece>();
            
        var tileNum = 0;
        List<string> rows = new();
        foreach (var line in file)
            if (line is "")
                map.Add(new MapPiece(tileNum, rows.ToArray()));
            else if (line.StartsWith("Tile"))
            {
                tileNum = int.Parse(line.Remove(@"\D"));
                rows = new List<string>();
            }
            else
                rows.Add(line);

        map.Add(new MapPiece(tileNum, rows.ToArray()));

        return map;
    }

    protected override long TaskA()
    {
        _finalmap = BuildMap();
        return (long) _finalmap[0][0].Id
               * _finalmap.Last()[0].Id
               * _finalmap[0].Last().Id
               * _finalmap.Last().Last().Id;
    }

    protected override long TaskB()
    {
        var tile_size = _finalmap[0][0].N - 2;
        var tiles = _n;
        var n = tile_size * tiles;

        var allMap = CreateMatrixFromMapPieces();
        var monsterMap = new int[n, n];

        var monster = new[,]
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
            {1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 1},
            {0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0}
        };
        var mw = 20;
        var mh = 3;
        var mon = 0;

        //try all rotations
        for (var i = 0; i < 2; i++, allMap.FlipLR())
        for (var j = 0; j < 2; j++, allMap.FlipUD())
        for (var k = 0; k < 4; k++, allMap.Rotate())
            //iterate all map
        for (var y = 0; y < n - mh; y++)
        for (var x = 0; x < n - mw; x++)
        {
            //try to find monster from current (y,x)
            var isMonster = true;
            for (var my = 0; isMonster && my < mh; my++)
            for (var mx = 0; isMonster && mx < mw; mx++)
            {
                var mapp = allMap[y + my, x + mx];
                var monn = monster[my, mx];
                if (monn == 1 && mapp != 1)
                    isMonster = false;
            }

            //mark monster on map
            if (isMonster)
            {
                mon++;
                for (var my = 0; my < mh; my++)
                for (var mx = 0; mx < mw; mx++)
                    if (monster[my, mx] == 1)
                        monsterMap[y + my, x + mx] = 1;
            }
        }

        //replace 0 <=> 1 for matrix multiplication
        for (var y = 0; y < n; y++)
        for (var x = 0; x < n; x++)
            monsterMap[y, x] = 1 - monsterMap[y, x];

        //return amount of '#'s without monsters
        return MatrixUtils.MultiplyElements(monsterMap, allMap).Flatten().Sum();
    }


    private List<List<MapPiece>> BuildMap()
    {
        List<List<MapPiece>> map = new();
        List<MapPiece> used = new();

        var corner = FindCorner();
        used.Add(corner);

        var n = (int) Math.Sqrt(Input.Count);
        _n = n;
        //first line
        {
            var row = new List<MapPiece> {corner};

            for (var i = 1; i < n; i++)
            {
                var left = row.Last().EdgeR;
                foreach (var m in Input.Except(used))
                    if (m.HasEdge(left))
                    {
                        Fit(m, left);
                        used.Add(m);
                        if (!Input.Except(used).Any(x => x.HasEdge(m.EdgeD)))
                            m.FlipUD();
                        row.Add(m);
                        break;
                    }
            }

            map.Add(row);
        }

        //other lines
        {
            for (var j = 1; j < n; j++)
            {
                var topRow = map.Last();
                int? left = null;
                List<MapPiece> row = new();
                for (var i = 0; i < n; i++)
                {
                    var top = topRow[i].EdgeD;
                    foreach (var m in Input.Except(used))
                        if (m.HasEdge(top) && (i == 0 || m.HasEdge(left.Value)))
                        {
                            Fit(m, left, up: top);
                            used.Add(m);
                            row.Add(m);
                            left = m.EdgeR;
                            break;
                        }
                }

                map.Add(row);
            }
        }

        return map;
    }

    private MapPiece FindCorner()
    {
        return Input.First(x =>
        {
            for (var i = 0; i < 2; i++, x.FlipLR())
            for (var j = 0; j < 2; j++, x.FlipUD())
            for (var k = 0; k < 4; k++, x.Rotate())
            {
                var exists_r = Input.Any(y => y != x && y.HasEdge(x.EdgeR));
                var exists_d = Input.Any(y => y != x && y.HasEdge(x.EdgeD));
                var exists_l = Input.Any(y => y != x && y.HasEdge(x.EdgeL));
                var exists_u = Input.Any(y => y != x && y.HasEdge(x.EdgeU));
                var isCorner = exists_r && exists_d && !exists_l && !exists_u;

                if (isCorner) return true;
            }

            return false;
        });
    }

    private static void Fit(MapPiece map, int? left = null, int? right = null, int? up = null, int? down = null)
    {
        for (var i = 0; i < 2; i++, map.FlipLR())
        for (var j = 0; j < 2; j++, map.FlipUD())
        for (var k = 0; k < 4; k++, map.Rotate())
            if ((left == null || map[0] == left)
                && (right == null || map[2] == right)
                && (up == null || map[1] == up)
                && (down == null || map[3] == down))
                return;
    }

    private int[,] CreateMatrixFromMapPieces()
    {
        var tile_size = _finalmap[0][0].N - 2;
        var tiles = _n;
        var n = tile_size * tiles;

        var allMap = new int[n, n];

        for (var y = 0; y < tiles; y++)
        for (var x = 0; x < tiles; x++)
        for (var yy = 0; yy < tile_size; yy++)
        for (var xx = 0; xx < tile_size; xx++)
            allMap[y * tile_size + yy, x * tile_size + xx] = _finalmap[y][x].mapp[yy + 1, xx + 1] ? 1 : 0;
        return allMap;
    }

    public class MapPiece
    {
        public int Id;
        public string[] map;
        public bool[,] mapp;
        public int N;

        public MapPiece(int id, string[] data)
        {
            map = data;
            Id = id;
            N = map.Length;
            mapp = new bool[N, N];
            for (var y = 0; y < data.Length; y++)
            for (var x = 0; x < data.Length; x++)
                mapp[y, x] = data[y][x] == '#';
        }

        public int EdgeL => mapp.GetColumn(0).ToBitMap();
        public int EdgeR => mapp.GetColumn(N - 1).ToBitMap();
        public int EdgeU => mapp.GetRow(0).ToBitMap();
        public int EdgeD => mapp.GetRow(N - 1).ToBitMap();

        public int ReverseEdgeL => mapp.GetColumn(0).Reverse().ToBitMap();
        public int ReverseEdgeR => mapp.GetColumn(N - 1).Reverse().ToBitMap();
        public int ReverseEdgeU => mapp.GetRow(0).Reverse().ToBitMap();
        public int ReverseEdgeD => mapp.GetRow(N - 1).Reverse().ToBitMap();

        public IEnumerable<int> allEdges => new List<int>
        {
            EdgeL, EdgeR, EdgeU, EdgeD,
            ReverseEdgeL, ReverseEdgeR, ReverseEdgeU, ReverseEdgeD
        };

        /// <summary>
        ///     0 => L, 1 => U, 2 => R, 3 => D
        /// </summary>
        public int this[int i] =>
            i switch
            {
                0 => EdgeL,
                1 => EdgeU,
                2 => EdgeR,
                3 => EdgeD
            };

        public bool HasEdge(int edge) => allEdges.Contains(edge);
        public void Rotate() => mapp = mapp.Rotate();
        public void FlipUD() => mapp = mapp.FlipUD();
        public void FlipLR() => mapp = mapp.FlipLR();

        public override string ToString() => $"Map: {Id}";
    }
}