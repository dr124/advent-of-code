// ReSharper disable SwitchStatementMissingSomeCases

using System.Collections;

#pragma warning disable 8509

namespace advent2019.Intcode
{
    public partial class Computer
    {
        private void MathCodes()
        {
            Memory[Instr.Arg3Index] = Instr.Op switch
            {
                Operation.Add => (Instr.Arg2 + Instr.Arg1),
                Operation.Multiply => (Instr.Arg2 * Instr.Arg1),
            };

            Pointer += 4;
        }

        private void DiagCodes()
        {
            switch (Instr.Op)
            {
                case Operation.ReadInput:
                    var xd = ReadInputValue();
                    Memory[Instr.Arg1Index] = xd;
                    break;
                case Operation.WriteOutput:
                    var xd2 = Memory[Instr.Arg1Index];
                    WriteOutputValue(xd2);
                    break;
            }

            Pointer += 2;
        }

        private void JumpCodes()
        {
            switch (Instr.Op)
            {
                case Operation.JumpTrue when Instr.Arg1 != 0:
                case Operation.JumpZero when Instr.Arg1 == 0:
                    Pointer = (int)Instr.Arg2;
                    break;
                default:
                    Pointer += 3;
                    break;
            }
        }
        
        private void IfCodes()
        {
            Memory[Instr.Arg3] = Instr.Op switch
            {
                Operation.LessThan => (Instr.Arg1 < Instr.Arg2 ? 1 : 0),
                Operation.AreEqual => (Instr.Arg1 == Instr.Arg2 ? 1 : 0),
            };

            Pointer += 4;
        }

        private void SetBase()
        {
            switch (Instr.Op)
            {
                case Operation.SetBase:
                    RelativeBase += (int)Instr.Arg1;
                    Pointer += 2;
                    break;
            }
        }
    }
}