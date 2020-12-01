using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Advent._2019.Week2
{
    public class Day14
    {
        static Dictionary<string, int> storage = new Dictionary<string, int>();
        static Dictionary<string, ChemicalFactory> Factories = new Dictionary<string, ChemicalFactory>();

        static long totalOres;

        public static void Execute()
        {
            var txt = File.ReadAllLines("Week2/input14.txt")
                .Select(x => x.Replace("=>", ",").Replace(" ", "").Split(","))
                .Select(x => x.Select(y => (
                    name: Regex.Replace(y, "[^A-Z]", ""),
                    count: int.Parse(Regex.Replace(y, "\\D", ""))
                )))
                .Select(x => x.ToArray())
                .ToArray();

            storage["ORE"] = 0;
            foreach (var ch in txt)
                storage[ch[^1].name] = 0;
            foreach (var ch in txt)
                Factories[ch[^1].name] = new ChemicalFactory(ch);

            Factories["FUEL"].Produce();
            var taskA = totalOres;

            /*
             * Task B was calculated in excel
             * loop eg. 1000x Factories["FUEL"].Produce()
             * take totalOre and storage["FUEL"] values 
             * get the mean value (in my case: ~998535.8)
             *
             * or make a loop to 1_000_000_000_000 and wait 🤣
             * one fuel is produced in 20ms (on my machine)
             */

            var taskB = 998536;
            Console.WriteLine($"A: {taskA}, B: {taskB}");
        }

        public class ChemicalFactory
        {
            public ChemicalFactory((string, int)[] ruleset)
            {
                Product = ruleset[^1];
                Recipe = ruleset[..^1];
            }

            public (string name, int count) Product { get; }
            public (string name, int count)[] Recipe { get; }

            private void Log(string str)
            {
#if DEBUG
                Console.WriteLine(str);
                Debug.WriteLine(str);
#endif
            }

            public virtual void Produce(int count = 1)
            {
                var productsReady = Recipe.All(x => storage[x.name] >= x.count * count);
                if (!productsReady)
                    Log($"{Product.name}: waiting for {count}x products");

                while (!AllProductsReady(count))
                    for (var i = 0; i < Recipe.Length; i++)
                    {
                        var recipeName = Recipe[i].name;
                        var recipeCount = Recipe[i].count * count;
                        if (recipeName == "ORE")
                        {
                            totalOres += recipeCount;
                            storage["ORE"] += recipeCount;
                        }
                        else if (storage[recipeName] < recipeCount)
                        {
                            var needed = recipeCount - storage[recipeName];
                            var units = (int) Math.Ceiling(needed / (double) Factories[recipeName].Product.count);
                            Factories[recipeName].Produce(units);
                        }
                    }

                for (var i = 0; i < Recipe.Length; i++)
                    storage[Recipe[i].name] -= Recipe[i].count * count;
                storage[Product.name] += Product.count * count;

                Log($"{Product.name}: Created {Product.count * count}x");
            }

            private bool AllProductsReady(int count)
            {
                var recipe = Factories[Product.name].Recipe;
                for (var i = 0; i < recipe.Length; i++)
                    if (storage[recipe[i].name] < recipe[i].count * count)
                        return false;
                return true;
            }
        }
    }
}