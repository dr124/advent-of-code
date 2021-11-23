using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent._2020.Week2.Gameboy;
using Advent.Core_2019_2020;

namespace Advent._2020.Week2;

public class Day8 : Day<Command[], int>
{
    public class MyGameBoy : GameBoy
    {
        private readonly HashSet<int> _indexes = new();
        public MyGameBoy()
        {
            OnCommandExecuted = () =>
            {
                if (_indexes.Contains(Index))
                    Stop();
                _indexes.Add(Index);
            };
        }
    }
        
    protected override Command[] ReadData()
    {
        return File.ReadAllLines("Week2/input8.txt")
            .Select(CommandFactory.Create)
            .ToArray();
    }

    protected override int TaskA()
    {
        var gameboy = new MyGameBoy
        {
            Commands = Input
        };
        gameboy.Run();
        return gameboy.Accumulator;
    }


    protected override int TaskB()
    {
        var found = false;
        var i = -1;
        MyGameBoy gameboy = null;
        while (i < Input.Length && !found)
        {
            ++i;
                
            var input2 = Input.ToArray();
            switch (input2[i].Instr)
            {
                case Instruction.nop:
                    input2[i] = new Command(Instruction.jmp, input2[i].arg);
                    break;
                case Instruction.jmp:
                    input2[i] = new Command(Instruction.nop, input2[i].arg);
                    break;
                default:
                    continue;
            }

            gameboy = new MyGameBoy
            {
                Commands = input2, 
                OnProgramFinished = () => found = true
            };

            gameboy.Run();
        }

        return gameboy?.Accumulator ?? -1;
    }
}