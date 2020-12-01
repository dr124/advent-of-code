using System;
using System.IO;
using System.Linq;
using MoreLinq;

namespace Advent._2019.Week2
{
    public static class Day8
    {
        public static void Execute()
        {
            var pixels = File.ReadAllBytes(@"Week2\input8.txt")
                .Select(x => x - 48)
                .Reverse()
                .ToArray();


            var a = Process(pixels);
            Console.WriteLine($"A: {a}, B: from image: \"UGCUH\"");
        }

        public static int Process(int[] pixels)
        {
            var w = 25;
            var h = 6;
            var area = w * h;
            var n = pixels.Length / area;
            var layers = new int[n][,];

            for (var i = 0; i < n; i++)
                layers[i] = new int[w, h];
            for (var i = 0; i < pixels.Length; i++)
            {
                var _w = i % w;
                var _h = i % area / w;
                var _l = i / area;
                layers[_l][_w, _h] = pixels[i];
            }

            var flattened = layers.Select(x => x.Flatten().Cast<int>());
            var layer = flattened.MinBy(x => x.Count(y => y == 0)).Take(1).First();
            var taskA = layer.Count(x => x == 1) * layer.Count(x => x == 2);


            // 0 is black, 1 is white, and 2 is transparent.
            var tab = new string[w, h];

            for (var y = 0; y < h; y++)
            for (var x = 0; x < w; x++)
            for (var k = n - 1; k >= 0; k--)
            {
                var pixel = layers[k][x, y];
                if (pixel == 2) // transparent
                    continue;
                if (pixel == 1) // white
                {
                    tab[x, y] = "█";
                    break;
                }
                if (pixel == 0) // black
                {
                    tab[x, y] = " ";
                    break;
                }
            }


            for (var y = h - 1; y >= 0; y--)
            {
                for (var x = w - 1; x >= 0; x--)
                    Console.Write(tab[x, y]);
                Console.WriteLine();
            }

            return taskA;
        }
    }
}