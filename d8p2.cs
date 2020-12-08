using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AdventOfCode2020
{
    [TestClass]
    public class D8P2
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_8_t_1.txt").ToList();
            Assert.AreEqual(8, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_8.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            private enum InstructionType
            {
                jmp,
                nop,
                acc
            }

            private class Instruction
            {
                public InstructionType Type { get; set; }
                public int Value { get; set; }
                public bool HasExcuted { get; set; }

                public Instruction Clone()
                {
                    return new Instruction
                    {
                        Type = Type,
                        Value = Value,
                        HasExcuted = HasExcuted
                    };
                }
            }

            public int RunChallenge(List<string> input)
            {
                var instructions = input.Select(rawInstruction =>
                {
                    var parsed = rawInstruction.Split(' ');
                    return new Instruction
                    {
                        Type = (InstructionType) Enum.Parse(typeof (InstructionType), parsed[0]),
                        Value = Convert.ToInt32(parsed[1])
                    };
                }).ToList();

                var possibleModificationCount = instructions.Count(z => z.Type == InstructionType.jmp || z.Type == InstructionType.nop);
                for (int modifyIndex = 0; modifyIndex < possibleModificationCount; modifyIndex++)
                {
                    try
                    {
                        var programToTest = getModifiedCopy(modifyIndex, instructions);
                        return runProgram(programToTest);
                    } catch (Exception)
                    {
                        continue;
                    }
                }
                throw new Exception("Could not find a solution");
            }

            private List<Instruction> getModifiedCopy(int modifyIndex, List<Instruction> instructions)
            {
                var copy = instructions.Select(i => i.Clone()).ToList();

                var modificationPositionsEncountered = 0;

                for (int j = 0; j < copy.Count; j++)
                {
                    if (copy[j].Type == InstructionType.jmp || copy[j].Type == InstructionType.nop)
                    {
                        if (modificationPositionsEncountered == modifyIndex)
                        {
                            var toChange = copy[j];
                            toChange.Type = toChange.Type == InstructionType.nop ? InstructionType.jmp : InstructionType.nop;
                            break;
                        }
                        modificationPositionsEncountered++;
                    }
                }

                return copy;
            }

            private int runProgram(List<Instruction> instructions)
            {
                var accumulator = 0;

                for (int i = 0; i < instructions.Count; i++)
                {
                    var currentInstruction = instructions[i];
                    instructions[i].HasExcuted = instructions[i].HasExcuted ? throw new Exception("Program beginning to loop...") : true;

                    if (currentInstruction.Type == InstructionType.acc)
                    {
                        accumulator += currentInstruction.Value;
                    }
                    else if (currentInstruction.Type == InstructionType.jmp)
                    {
                        i += currentInstruction.Value - 1;
                    }
                }

                return accumulator;
            }
        }
    }
}
