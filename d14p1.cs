using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    [TestClass]
    public class D14P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_14_t_1.txt").ToList();
            Assert.AreEqual(165L, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_14.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public long RunChallenge(List<string> program)
            {
                var numberMatcher = new Regex(@"\d+");
                var memory = new Dictionary<long, long>();
                string mask = null;
                foreach (var instruction in program)
                {
                    if (instruction.StartsWith("mask"))
                    {
                        mask = instruction.Split(" = ")[1];
                        continue;
                    }
                    var numbers = numberMatcher.Matches(instruction);
                    var address = Convert.ToInt64(numbers[0].Value);
                    var valueToAdd = ApplyMask(mask, Convert.ToInt64(numbers[1].Value));
                    memory[address] = valueToAdd;
                }
                return memory.Values.Sum();
            }

            private long ApplyMask(string mask, long value)
            {
                var binaryString = Convert.ToString(value, 2).PadLeft(36, '0');
                var resultString = String.Empty;
                for (int i = 0; i < binaryString.Length; i++)
                {
                    if (mask[i] != 'X')
                    {
                        resultString += mask[i];
                    }
                    else
                    {
                        resultString += binaryString[i];
                    }
                }
                return Convert.ToInt64(resultString, 2);
            }
        }
    }
}
