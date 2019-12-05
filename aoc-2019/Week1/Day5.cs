using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
// ReSharper disable EmptyGeneralCatchClause

namespace advent.Week1
{
    public static class Day5
    {
        public static int DiagCode = 5;

        public static void Execute()
        {
            //task 1
            var ints = File.ReadAllText(@"Week1\input5.txt")
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            DiagCode = 1;
            Process(ints);

            DiagCode = 5;
            Process(ints);
        }

        public static int[] Process(int[] ints)
        {
            var list = ints.ToList();
            for (var i = 0; i < list.Count;)
                i = OpCode(list, i);
            return list.ToArray();
        }


        public static int MathCodes(List<int> list, int i)
        {
            var instr = new Instruction(list, i);
            switch (instr.Instr)
            {
                case 1: 
                    list[instr.Param3Value] = instr.Param2Value + instr.Param1Value;
                    break;
                case 2:
                    list[instr.Param3Value] = instr.Param2Value * instr.Param1Value;
                    break;
            }

            return i + 4;
        }

        public static int DiagCodes(List<int> list, int i)
        {
            var instr = new Instruction(list, i);
            switch (instr.Instr)
            {
                case 3: // input a value
                    list[list[i + 1]] = DiagCode;
                    break;
                case 4: // output a value
                    var xd = list[list[i + 1]];
                    Console.WriteLine($"{xd}");
                    Debug.WriteLine($"{xd}");
                    break;
            }

            return i + 2;
        }

        public static int JumpCodes(List<int> list, int i)
        {
            var instr = new Instruction(list, i);
            switch (instr.Instr)
            {
                case 5: // jump if true 
                    if (instr.Param1Value != 0)
                        return instr.Param2Value;
                    break;
                case 6: // jump is zero
                    if (instr.Param1Value == 0)
                        return instr.Param2Value;
                    break;
            }

            return i + 3;
        }

        public static int IfCodes(List<int> list, int i)
        {
            var instr = new Instruction(list, i);
            switch (instr.Instr)
            {
                case 7: // less than
                    list[instr.Param3Value] = instr.Param1Value < instr.Param2Value ? 1 : 0;
                    break;
                case 8: // equals
                    list[instr.Param3Value] = instr.Param1Value == instr.Param2Value ? 1 : 0;
                    break;
            }

            return i + 4;
        }

        public static int OpCode(List<int> list, int i)
        {
            var instr = new Instruction(list, i);
            switch (instr.Instr)
            {
                case 1:
                case 2:
                    return MathCodes(list, i);
                case 3:
                case 4:
                    return DiagCodes(list, i);
                case 5:
                case 6:
                    return JumpCodes(list, i);
                case 7:
                case 8:
                    return IfCodes(list, i);
                case 99:
                    return list.Count; // program exit
                default:
                    throw new Exception("witam, error, pozdrawiam");
            }
        }

        public class Instruction
        {
            public int Instr;
            public int Param1Value, Param2Value, Param3Value;
            public int? Param1Mode, Param2Mode, Param3Mode;

            public Instruction(List<int> list, int i)
            {
                var str = list[i].ToString();

                // one digit instr
                Instr = list[i];

                try
                {
                    Instr = int.Parse(str.Substring(str.Length - 2, 2));
                    Param1Mode = int.Parse(str.Substring(str.Length - 3, 1));
                    Param2Mode = int.Parse(str.Substring(str.Length - 4, 1));
                    Param3Mode = int.Parse(str.Substring(str.Length - 5, 1));
                } catch {}

                try
                {
                    Param1Value = Param1Mode == 1 ? list[i + 1] : list[list[i + 1]];
                    Param2Value = Param2Mode == 1 ? list[i + 2] : list[list[i + 2]];
                    Param3Value = list[i + 3]; /*Param3Mode == 1 ? list[i + 3] : list[i + 3];*/
                } catch { }
            }
        }
    }
}