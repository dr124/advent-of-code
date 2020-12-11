using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent.Core;

namespace Advent._2020.Week2
{
    public class Day11 : Day<Dictionary<Vec2, Day11.State>, int>
    {
        public enum State
        {
            Foor,
            Empty,
            Occupied
        }

        protected override Dictionary<Vec2, State> ReadData()
        {
            return File.ReadAllLines("Week2/input11.txt")
                .Select(x => x.Select(c => c == '.' ? State.Foor : State.Empty))
                .SelectMany((o, y) => o.Select((s, x) => (x, y, s)))
                .ToDictionary(o => new Vec2(o.x, o.y), o => o.s);
        }

        protected override int TaskA()
        {
            var map = Input;
            while (true)
            {
                var newMap = Mutate(map, true, 4);

                if(map.SequenceEqual(newMap))
                    return newMap.Values.Count(x => x == State.Occupied);

                map = newMap;
            }
        }

        protected override int TaskB()
        {
            
            var map = Input;
            while (true)
            {
                var newMap = Mutate(map, false, 5);
               
                if (map.SequenceEqual(newMap))
                    return newMap.Values.Count(x => x == State.Occupied);
               
                map = newMap;
            }
        }

        private Dictionary<Vec2, State> Mutate(Dictionary<Vec2, State> map, bool onlyNear, int minOccupants)
        {
            var copy = map.ToDictionary(x => x.Key, x => x.Value);

            foreach (var pos in map.Keys)
                copy[pos] = map[pos] switch
                {
                    State.Empty when Occupied(map, pos, onlyNear) == 0 => State.Occupied,
                    State.Occupied when Occupied(map, pos, onlyNear) >= minOccupants => State.Empty,
                    _ => copy[pos]
                };

            return copy;
        }

        private int Occupied(Dictionary<Vec2, State> map, Vec2 point, bool onlyNear)
        {
            return isOccupied((-1, -1)) + isOccupied((-1, 0)) + isOccupied((-1, 1)) + isOccupied((0, -1))
                   + isOccupied((0, 1)) + isOccupied((+1, -1)) + isOccupied((+1, 0)) + isOccupied((+1, 1));

            int isOccupied(Vec2 dir)
            {
                for (var i = 1;; i++)
                {
                    var pos = point + dir.Scale(i);
                    if (!map.ContainsKey(pos))
                        return 0;

                    if (map[pos] != State.Foor)
                        return map[pos] == State.Occupied ? 1 : 0;

                    if (onlyNear) //only adjacent
                        return 0;
                }
            }
        }
    }
}