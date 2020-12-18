using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    [TestClass]
    public class D14P2
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_14_t_2.txt").ToList();
            Assert.AreEqual(208L, new Program().RunChallenge(input));
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
                    var addressesToWriteTo = ApplyMask(mask, Convert.ToInt64(numbers[0].Value));
                    foreach (var adress in addressesToWriteTo)
                    {
                        memory[adress] = Convert.ToInt64(numbers[1].Value);
                    }
                }
                return memory.Values.Sum();
            }

            private List<long> ApplyMask(string mask, long value)
            {
                var binaryString = Convert.ToString(value, 2).PadLeft(36, '0');
                var resultStrings = new List<string>() { String.Empty };
                for (int i = 0; i < binaryString.Length; i++)
                {
                    switch (mask[i])
                    {
                        case '0':
                            resultStrings = resultStrings.Select(s => s + binaryString[i]).ToList();
                            break;
                        case '1':
                            resultStrings = resultStrings.Select(s => s + '1').ToList();
                            break;
                        case 'X':
                            resultStrings = resultStrings.SelectMany(s => new List<string> { s + '1', s + '0' }).ToList();
                            break;
                    }
                }
                return resultStrings.Select(s => Convert.ToInt64(s, 2)).ToList();
            }
        }
    }
}
