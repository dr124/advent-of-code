using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent.Core;
using Map = System.Collections.Generic.HashSet<Advent.Core.Vec2>;
    
namespace Advent._2020.Week4
{
    public class Day24 : Day<List<Vec2>, int>
    {
        protected override List<Vec2> ReadData()
        {
            var lines = File.ReadAllLines("Week4/Input24.txt")
                .Select(x => SeparateTiles(SeparateTiles(x)).SplitClear("_"));

            var tileSteps = lines.Select(x => x.Select(y => (Vec2) (y switch
            {
                "e" => (1, 0),
                "w" => (-1, 0),
                "se" => (1, -1),
                "sw" => (0, -1),
                "ne" => (0, 1),
                "nw" => (-1, 1)
            })));
            
            var tiles = tileSteps.Select(x => x.Aggregate((v1, v2) => v1 + v2));
            
            var blackTiles = tiles
                .GroupBy(x => x)
                .Where(x => x.Count() % 2 == 1)
                .Select(x => x.Key);

            return blackTiles.ToList();
        }

        protected override int TaskA()
        {
            return Input.Count;
        }

        protected override int TaskB()
        {
            const int days = 100;
            var map = Input.ToHashSet();
            for (var i = 0; i < days; i++)
                map = MutateMap(map);
            return map.Count;
        }

        private Map MutateMap(Map map)
        {
            var neighbours = new[] {(1, 0), (-1, 0), (1, -1), (0, -1), (0, 1), (-1, 1)};

            var toVisit = new Map(map);
            foreach (var v in map)
            foreach (var n in neighbours)
                toVisit.Add(v + n);

            var newMap = new Map();

            foreach (var v in toVisit)
            {
                var isBlack = map.Contains(v);
                if (isBlack)
                    newMap.Add(v);

                var neigh = neighbours.Count(n => map.Contains(v + n));

                if (isBlack && neigh is 0 or > 2)
                    newMap.Remove(v);
                else if (!isBlack && neigh is 2)
                    newMap.Add(v);
            }

            return newMap;
        }

        private static string SeparateTiles(string x)
        {
            return x
                .Replace("se", "_se_").Replace("ne", "_ne_").Replace("sw", "_sw_").Replace("nw", "_nw_")
                .Replace("ww", "w_w").Replace("ew", "e_w").Replace("we", "w_e").Replace("ee", "e_e");
        }
    }
}