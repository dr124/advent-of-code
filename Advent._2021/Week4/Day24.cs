using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using Advent.Core;
using Advent.Core.Extensions;
using Microsoft.Diagnostics.Runtime.Interop;
using T = System.Int64;

namespace Advent._2021.Week4;

public class Day24 : IReadInputDay
{

    private List<Instruction> instructions = new();
    public void ReadData()
    {
        var lines = File.ReadAllLines("Week4/Day24.txt");
        foreach (var line in lines)
        {
            var xd = line.Split(" ", SplitOptions.Clear);
            if (xd[0] == "inp")
            {
                instructions.Add(new Instruction(xd[0], xd[1][0]));
            }else if (T.TryParse(xd[2], out var value))
            {
                instructions.Add(new (xd[0], xd[1][0], value: value));
            }
            else
            {
                instructions.Add(new(xd[0], xd[1][0], var2: xd[2][0]));
            }
        }
    }

    public object TaskA()
    {
        var numbers = new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
        for (int e = 0; e < 14; e++)
        for (int i = 0; i < 14; i++)
        {
            int mini = 1;
            long mini_val = long.MaxValue;
            for (int j = 1; j <= 9; j++)
            {
                numbers[i] = j;
                var p = XD(numbers);
                if (p < mini_val)
                {
                    mini = j;
                    mini_val = p;
                }
            }

            numbers[i] = mini;
        }
       
        return string.Join("", numbers);
    }

    public object TaskB()
    {
        var numbers = new[] { 9,9,9,9,9,9,9,9,9,9,9,9,9,9 };
        for (int e = 0; e < 14; e++)
        for (int i = 0; i < 14; i++)
        {
            int mini = 9;
            long mini_val = long.MaxValue;
            for (int j = 9; j >= 1; j--)
            {
                numbers[i] = j;
                var p = XD(numbers);
                if (p < mini_val)
                {
                    mini = j;
                    mini_val = p;
                }
            }

            numbers[i] = mini;
        }

        return string.Join("", numbers);
    }

    private T XD(int[] input)
    {
        Mem w = new();
        Mem x = new();
        Mem y = new();
        Mem z = new();

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        Mem Memory(char c)
        {
            return c switch
            {
                'x' => x,
                'z' => z,
                'y' => y,
                'w' => w,
                _ => new Mem()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        Mem Var2(Instruction instr) => instr.isVar2 ? Memory(instr.var2) : instr.mem;

        int i = 0;

        for (var index = 0; index < instructions.Count; index++)
        {
            var instr = instructions[index];
            var instr2 = Var2(instr);
            switch (instr.name)
            {
                case "inp":
                    Memory(instr.var1).Val = input[i++];
                    break;
                case "add":
                    Memory(instr.var1).Val += instr2.Val;
                    break;
                case "mul":
                     Memory(instr.var1).Val *= instr2.Val;
                    break;
                case "div":
                    Memory(instr.var1).Val /= instr2.Val;
                    break;
                case "mod":
                    Memory(instr.var1).Val %= instr2.Val;
                    break;
                case "eql":
                    Memory(instr.var1).Val = Memory(instr.var1).Val == instr2.Val ? 1: 0;
                    break;
            }
        }
        return Memory('z').Val;
    }


    class Instruction
    {
        public string name;
        public char var1;
        public char var2;
        public T value;
        public bool isVar2 => var2 != 0;
        public Mem mem;
        public Instruction(string name, char var1, char var2 = '\0', T value = 0)
        {
            this.name = name;
            this.var1 = var1;
            this.var2 = var2;
            this.value = value;
            if (!isVar2) 
                mem = new Mem { Val = value };
        }
    }
    record Mem
    {
        public T Val;
    }
}
