using Lista = System.Collections.Generic.List<(int val, int depth)>;

namespace Advent._2021.Week3
{
    class Day18
    {
        static string stringify(Lista l) => "v:" + string.Join(" ", l.Select(x => x.val)) + "\nd:" + string.Join(" ", l.Select(x => x.depth));

        public static void Execute()
        {

            var file = File.ReadAllLines("Week3/Day18.txt");
            var xd = ReadList(file[0]);
            var result = ReadList(file[0]);
            Reduce(result);
            foreach (var line in file[1..])
            {
                var toAdd = ReadList(line);
                result = ConnectList(result, toAdd);
                Reduce(result);
            }
            var dupa = CalculateMagnitude(result);
            Console.WriteLine(dupa);

            var przetworzone = file.Select(x =>
                {
                    var list = ReadList(x);
                    Reduce(list);
                    return list;
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            var xdd = new int[100];

            var idx = Enumerable.Range(0, 100)
                .SelectMany(l => Enumerable.Range(0, 100), (l, r) => (l, r))
                .Where(x => x.l != x.r).ToList();

            {
                var ab = ConnectList(przetworzone[63].ToList(), przetworzone[38].ToList());
                Reduce(ab);
                CalculateMagnitude(ab);
            }


            var n = idx
                .OrderBy(x => Guid.NewGuid()).ToList()
                .AsParallel()
                .Select(x =>
                {
                    var ab = ConnectList(przetworzone[x.l].ToList(), przetworzone[x.r].ToList());
                    Reduce(ab);
                    return CalculateMagnitude(ab);
                })
                .ToList();
            var m = n.Max();
            Console.WriteLine(m);
        }

        public static int CalculateMagnitude(Lista lista)
        {
            while (lista.Count > 1)
                for (int i = 0; i < lista.Count - 1; i++)
                    if (lista[i].depth == lista[i + 1].depth)
                    {
                        lista[i] = (lista[i].val * 3 + lista[i + 1].val * 2, lista[i].depth - 1);
                        lista.RemoveAt(i + 1);
                        i=-1;
                    }

            return lista[0].val;
        }

        public static Lista ConnectList(Lista A, Lista B) => A.Concat(B).Select(x => (x.val, x.depth + 1)).ToList();

        public static Lista ReadList(string line)
        {
            var lista = new Lista();
            var br = 0;
            foreach (var c in line)
            {
                if (c == '[')
                    br++;
                else if (c == ']')
                    br--;
                else if (c is >= '0' and <= '9')
                    lista.Add((c - '0', br));
            }

            return lista;
        }

        public static void Reduce(Lista lista)
        {
            do
            {
                for (var i = 0; i < lista.Count; i++)
                {
                    if (i < lista.Count - 1 && lista[i].depth > 4 && lista[i + 1].depth == lista[i].depth) // explode
                    {
                        if (i > 0)
                            lista[i - 1] = (lista[i - 1].val + lista[i].val, lista[i - 1].depth);
                        if (i < lista.Count - 2)
                            lista[i + 2] = (lista[i + 2].val + lista[i + 1].val, lista[i + 2].depth);
                        lista[i] = (0, lista[i].depth - 1);
                        lista.RemoveAt(i + 1);
                    }
                }

                for (var i = 0; i < lista.Count; i++)
                {
                    if (lista[i].val > 9) //split
                    {
                        var lewa = (int)Math.Floor(lista[i].val / 2.0);
                        var prawa = (int)Math.Ceiling(lista[i].val / 2.0);

                        lista[i] = (lewa, lista[i].depth + 1);
                        lista.Insert(i + 1, (prawa, lista[i].depth));
                        break;
                    }
                }

            } while (lista.Any(x => x.depth > 4) || lista.Any(x => x.val > 9));
        }
    }
}


