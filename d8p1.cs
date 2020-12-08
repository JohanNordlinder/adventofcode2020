using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D8P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_8_t_1.txt").ToList();
            Assert.AreEqual(5, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_8.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public int RunChallenge(List<string> input)
            {
                var program = input.Select(rawInstruction =>
                {
                    var parsed = rawInstruction.Split(' ');
                    return new { instructionName = parsed[0], value = Convert.ToInt32(parsed[1]) };
                }).ToList();
                var hasRun = new bool[input.Count];
                var accumelator = 0;
                for(int i = 0; i < program.Count; i++)
                {
                    var currentInstruction = program[i];
                    if (hasRun[i])
                    {
                        break;
                    } else
                    {
                        hasRun[i] = true;
                    }
                    if (currentInstruction.instructionName == "acc")
                    {
                        accumelator += currentInstruction.value;
                    } else if (currentInstruction.instructionName == "jmp")
                    {
                        i += currentInstruction.value - 1;
                    } 
                }

                return accumelator;
            }
        }
    }
}
