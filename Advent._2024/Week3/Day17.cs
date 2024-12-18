namespace Advent._2024.Week3;

public class Day17 : IDay
{
    private readonly int _registerA;
    private readonly int _registerB;
    private readonly int _registerC;
    private int instructionPointer = 0;
    private List<int> output = [];
    private readonly List<int> program = [];

    public Day17(string[] input)
    {
        _registerA = int.Parse(input[0].Split(':', StringSplitOptions.TrimEntries)[1]);
        _registerB = int.Parse(input[1].Split(':', StringSplitOptions.TrimEntries)[1]);
        _registerC = int.Parse(input[2].Split(':', StringSplitOptions.TrimEntries)[1]);
        program = input[4].Split(':')[1].Split(',').Select(int.Parse).ToList();
    }

    public object Part1()
    {
        var computer = new OpcodeComputer(_registerA, _registerB, _registerC, program);
        computer.Calculate();
        return string.Join(",", computer.Output);
    }

    public object Part2()
    {
        for (int i = 0; i < program.Count; i++)
        {
        }

        return -1;
    }

    private enum Operation
    {
        Adv,
        Bxl,
        Bst,
        Jnz,
        Bxc,
        Out,
        Bdv,
        Cdv
    }

    private class OpcodeComputer(long registerA, long registerB, long registerC, List<int> program)
    {
        private long _registerA = registerA;
        private long _registerB = registerB;
        private long _registerC = registerC;
        private int _instructionPointer;
        public readonly List<int> Output = [];
        public bool Failed;

        private long GetCombo(int operand)
        {
            return operand switch
            {
                0 => 0,
                1 => 1,
                2 => 2,
                3 => 3,
                4 => _registerA,
                5 => _registerB,
                6 => _registerC,
                7 => 7, //throw new Exception("should never get here - operand 7"),
                _ => throw new Exception("should never get here - operand >= 8")
            };
        }

        private void DoCalc()
        {
            var instr = (Operation)program[_instructionPointer];
            var literalOperand = program[_instructionPointer + 1];
            var combo = GetCombo(literalOperand);

            _instructionPointer += 2;

            switch (instr)
            {
                case Operation.Adv:
                    _registerA = _registerA / (1 << (int)combo);
                    break;
                case Operation.Bdv:
                    _registerB = _registerA / (1 << (int)combo);
                    break;
                case Operation.Cdv:
                    _registerC = _registerA / (1 << (int)combo);
                    break;
                case Operation.Bxl:
                    _registerB = _registerB ^ literalOperand;
                    break;
                case Operation.Bst:
                    _registerB = combo % 8;
                    break;
                case Operation.Jnz:
                    if (_registerA == 0)
                    {
                        // do nothing
                    }
                    else
                    {
                        _instructionPointer = literalOperand;
                    }

                    break;
                case Operation.Bxc:
                    _registerB = _registerB ^ _registerC;
                    break;
                case Operation.Out:
                    Output.Add((int)(combo & 7));
                    break;
                default:
                    throw new Exception("no instr");
            }
        }

        public void Calculate()
        {
            while (_instructionPointer < program.Count && !Failed)
            {
                DoCalc();
            }
        }
    }
}