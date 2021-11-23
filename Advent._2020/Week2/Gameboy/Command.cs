using System;

namespace Advent._2020.Week2.Gameboy;

public enum Instruction
{
    acc,
    jmp,
    nop
}

public record Command(Instruction Instr, int arg);

public class CommandFactory
{
    public static Command Create(string line)
    {
        var split = line.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return split[0] switch
        {
            nameof(Instruction.acc) => new Command(Instruction.acc, int.Parse(split[1])),
            nameof(Instruction.jmp) => new Command(Instruction.jmp, int.Parse(split[1])),
            nameof(Instruction.nop) => new Command(Instruction.nop, int.Parse(split[1])),
            _ => throw new ArgumentOutOfRangeException("Instruction name", $"unknown instruction: {split[0]}")
        };
    }
}