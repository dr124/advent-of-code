using System.Collections.Generic;
using System.Linq;

namespace Advent._2019.Intcode;

public enum Mode
{
    Pointer = 0,
    Value = 1,
    Relative = 2
}

public enum Operation
{
    Add = 1,
    Multiply = 2,
    ReadInput = 3,
    WriteOutput = 4,
    JumpTrue = 5,
    JumpZero = 6,
    LessThan = 7,
    AreEqual = 8,
    SetBase = 9,
    HALT = 99
}

public class Instruction
{
    private readonly Mode arg1Mode, arg2Mode, arg3Mode;
    public readonly long Arg1, Arg2, Arg3;
    public readonly int Arg1Index, Arg2Index, Arg3Index;
    public readonly Operation Op;
    private readonly int _relativeBase;

#if DEBUG
    private string raw;
#endif

    public Instruction(List<long> memory, int i, int relativeBase)
    {
#if DEBUG
        var str = memory[i].ToString();
        raw = $"{memory[i]}, {memory[i + 1]}, {memory[i + 1]}, {memory[i + 3]}";
#endif
        int inst = (int)memory[i];
        var digits = new[] { inst / 10000, inst / 1000 % 10, inst / 100 % 10, inst % 100 };

        _relativeBase = relativeBase;

        Op = (Operation)digits[3];
        arg1Mode = (Mode)digits[2];
        arg2Mode = (Mode)digits[1];
        arg3Mode = (Mode)digits[0];

        Arg1Index = arg1Mode switch
        {
            Mode.Pointer => (int)memory[i + 1],
            Mode.Value => i + 1,
            Mode.Relative => _relativeBase + (int)memory[i + 1]
        };
        Arg2Index = arg2Mode switch
        {
            Mode.Pointer => (int)memory[i + 2],
            Mode.Value => i + 2,
            Mode.Relative => _relativeBase + (int)memory[i + 2]
        };
        Arg3Index = arg3Mode switch
        {
            Mode.Pointer => (int)memory[i + 3],
            Mode.Value => i + 3,
            Mode.Relative => _relativeBase + (int)memory[i + 3]
        };

        if(memory.Count > Arg1Index && Arg1Index >= 0)
            Arg1 = memory[Arg1Index];
        if (memory.Count > Arg2Index && Arg2Index >=0)
            Arg2 = memory[Arg2Index];
        Arg3 = Arg3Index;
    }

}