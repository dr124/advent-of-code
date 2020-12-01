using System;
using System.Linq;
using Advent.Core;
using MathNet.Numerics.LinearAlgebra;

namespace Advent._2019.Week2
{
    public static class Day12
    {
        public static (int m1, int m2)[] pairs = {(0, 1), (0, 2), (0, 3), (1, 2), (1, 3), (2, 3)};
        private static Matrix<double> pos;
        private static Matrix<double> vel;
        private static Matrix<double> start;

        public static void Execute()
        {
            var input = Matrix<double>.Build.DenseOfArray(new double[,]
            {
                {16, -11, 2},
                {0, -4, 7},
                {6, 4, -10},
                {-3, -2, -4}
            });
            pos = input.Clone();
            vel = Matrix<double>.Build.Dense(4, 3, 0);
            start = input.Clone();

            for (var i = 0; i < 10; i++)
                Simulate();
            var taskA = vel.RowAbsoluteSums() * pos.RowAbsoluteSums();

            pos = input.Clone();
            vel = Matrix<double>.Build.Dense(4, 3, 0);

            var taskB = new int[3];
            var ii = 0;
            do
            {
                ii++;
                Simulate();

                for (var i = 0; i < 3; i++)
                    if (taskB[i] == 0 && IsAxisZero(i))
                        taskB[i] = ii + 1;
            } while (taskB.Any(x => x == 0));

            Console.WriteLine($"A: {taskA}");
            Console.WriteLine($"B: {Utils.lcm(taskB[0], Utils.lcm(taskB[1],taskB[2]))}");
        }

        private static bool IsAxisZero(int axis)
        {
            return pos[0, axis] == start[0, axis]
                   && pos[1, axis] == start[1, axis]
                   && pos[2, axis] == start[2, axis]
                   && pos[3, axis] == start[3, axis];
        }

        public static void Simulate()
        {
            for (var i = 0; i < pairs.Length; i++)
            for (var axis = 0; axis < 3; axis++)
            {
                var m1 = pairs[i].m1;
                var m2 = pairs[i].m2;

                var p1 = pos[m1, axis];
                var p2 = pos[m2, axis];
                vel[m1, axis] += p2.CompareTo(p1);
                vel[m2, axis] -= p2.CompareTo(p1);
            }

            for (var c = 0; c < 3; c++)
            for (var r = 0; r < 4; r++)
                pos[r, c] += vel[r, c];
        }
    }
}