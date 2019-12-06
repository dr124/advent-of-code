using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace advent.Week1
{
    public static class Day6
    {
        public static void Execute()
        {
            var orbits = File.ReadAllLines(@"Week1\input6.txt")
                .Where(x => !string.IsNullOrEmpty(x))
                .ToArray();

            var (a, b) = Process(orbits);
            Console.WriteLine($"A: {a}, B: {b}");
        }

        public static (int, int) Process(string[] orbits)
        {
            var planets = new Dictionary<string, Planet>();
            foreach (var orbit in orbits)
            {
                var p = orbit.Split(")");

                planets.TryAdd(p[0], new Planet(p[0]));
                planets.TryAdd(p[1], new Planet(p[1]));

                var p1 = planets[p[0]];
                var p2 = planets[p[1]];

                p2.Parent = p1;
                p1.Children.Add(p2);
            }

            var taskA = planets.Sum(x => x.Value.OrbitCount);

            var taskB = ShortestPath(planets["YOU"], planets["SAN"]) - 2;

            return (taskA, taskB);
        }

        public static int ShortestPath(Planet src, Planet dest)
        {
            var visited = new HashSet<Planet>();
            var queue = new Queue<Planet>();
            var nextQueue = new Queue<Planet>();
            var dist = 0;

            queue.Enqueue(src);
            while (true)
                if (queue.Count > 0)
                {
                    var planet = queue.Dequeue();
                    if (!visited.Add(planet))
                        continue;
                    if (planet == dest)
                        return dist;
                    if (planet.Parent != null)
                        nextQueue.Enqueue(planet.Parent);
                    foreach (var plx in planet.Children)
                        nextQueue.Enqueue(plx);
                }
                else if (nextQueue.Count > 0)
                {
                    queue = nextQueue;
                    nextQueue = new Queue<Planet>();
                    dist++;
                }
                else
                {
                    return dist;
                }
        }


        public class Planet
        {
            public string Name;
            public Planet Parent;
            public List<Planet> Children;

            public Planet(string name)
            {
                Name = name;
                Children = new List<Planet>();
            }

            public int OrbitCount
            {
                get
                {
                    var orbits = 0;
                    var planet = this;
                    while (planet.Parent != null)
                    {
                        planet = planet.Parent;
                        orbits += 1;
                    }

                    return orbits;
                }
            }
        }
    }
}