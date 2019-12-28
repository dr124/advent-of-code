using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace advent2019.Week3
{
    public static class Day20
    {
        public static void Execute()
        {
            var txt = File.ReadAllLines("Week3/input20.txt")
                .Select(x => x.Select(y => (int)y).ToArray())
                .ToArray();
            var maze = new Maze(txt);
            maze.DrawMaze();

            var zzpos = maze.FindPortals("ZZ").First();
            int steps = 0;
            do
            {
                maze.ProcessMaze();
                //    steps = -maze[zzpos]; maze.DrawMaze();
                maze.DrawMaze();

            } while (steps <= 0);
            
            maze.DrawMaze();
            Console.WriteLine($"FOUND ZZ IN {steps-1} steps");
            
        }

        public class Maze
        {
            private readonly int[][] maze;
  
            public Maze(int[][] data)
            {
                maze = data;
                this[FindPortals("AA").First()] = -1;
            }

            public ref int this[int x, int y] => ref maze[y][x];
            public ref int this[(int x, int y) p] => ref maze[p.y][p.x];

            public void ProcessMaze()
            {
                var dir = new (int x, int y)[] { (1, 0), (-1, 0), (0, 1), (0, -1) };

                for (var y = 1; y < maze.Length - 1; y++)
                    for (var x = 1; x < maze[y].Length - 1; x++)
                        if (this[x, y] < 0)
                            foreach (var (dx, dy) in dir)
                            {
                                int xx = x + dx, yy = y + dy;
                                if (this[xx, yy] == '.' || this[xx, yy] < 0
                                    && this[x, y] - 1 > this[xx, yy])
                                    this[xx, yy] = this[x, y] - 1;

                                if (IsPortal(xx, yy))
                                {
                                    ProcessPortal(xx, yy);
                                }
                            }
            }

            private bool IsPortal(int x, int y) => this[x, y] >= 'A' && this[x, y] <= 'Z';

            private string GetPortalName(int x, int y)
            {
                var horizontalPortal = new[] { (x - 1, y), (x, y), (x + 1, y) }
                    .Select(p => (char)this[p])
                    .ToArray();
                if (!horizontalPortal[1].IsChar())
                    return null;
                var horizontalPortalName = string.Join("", horizontalPortal.Where(Utils.IsChar));
                if (horizontalPortalName.Length == 2)
                    return horizontalPortalName;

                var verticalPortal = new[] {(x, y - 1), (x, y), (x, y + 1)}
                    .Select(p => (char) this[p])
                    .ToArray();
                if (!verticalPortal[1].IsChar())
                    return null;
                var verticalPortalName = string.Join("", verticalPortal.Where(Utils.IsChar));
                if (verticalPortalName.Length == 2)
                    return verticalPortalName;

                return null;
            }

            public IEnumerable<(int x, int y)> FindPortals(string name)
            {
                var pts = new (int x, int y)[]
                {
                    (0, 0),
                    (0, 1), (0, -1), (1, 0), (-1, 0),
                    (0, 2), (0, -2), (2, 0), (-2, 0)
                };

                for (var y = 1; y < maze.Length - 1; y++)
                    for (var x = 1; x < maze[y].Length - 1; x++)
                    {
                        var portalName = GetPortalName(x, y);
                        if (portalName == name)
                        {
                            foreach (var p in pts)
                            {
                                int px = x + p.x, py = y + p.y;
                                if (py >= 0 && py < maze.Length && px >= 0 && px < maze[py].Length)
                                    if (this[px, py] == '.' || this[px,py] < 0)
                                    {
                                        yield return (px, py);
                                        break;
                                    }
                            }
                        }
                    }

                yield break;
            }


            private void ProcessPortal(int x, int y)
            {
                var portalName = GetPortalName(x, y);
                var portals = FindPortals(portalName).Distinct().ToArray();
                if (portals.Length == 2)
                {
                    if (this[portals[0]] < 0)
                        this[portals[1]] = this[portals[0]] - 1;
                    else if (this[portals[1]] < 0)
                        this[portals[0]] = this[portals[1]] - 1;
                }
                else
                {

                }
            }

            public void DrawMaze()
            {


                Console.SetCursorPosition(0, 0);
                Console.WriteLine(string.Join("\n",
                        maze
                            .Take(70)
                            .Select(x =>
                            string.Join("",
                                x
                                    .Select(p =>
                                    p switch
                                    {
                                        '#' => "██",
                                        '.' => "..",
                                        ' ' => "  ",
                                        var c when c < 0 => (-c%100).ToString("00"),
                                        var c when c >= 'A' && c <= 'Z' => $"{(char)c}{(char)c}",
                                        _ => null
                                    } ?? p.ToString("00")
                                )
                            )
                        )
                    )
                );

                var zzpos = FindPortals("ZZ").First();
                var steps = -this[zzpos];
                Console.WriteLine($"ZZ: {steps-1}");
            }
        }
    }
}