using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D1P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_1_t_1.txt").ToList();
            Assert.AreEqual(514579, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_1.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public int RunChallenge(List<string> input)
            {
                var numbers = input.Select(v => Convert.ToInt32(v));
                return FindMatch(numbers);
            }

            private int FindMatch(IEnumerable<int> numbers)
            {
                foreach (var number1 in numbers)
                {
                    foreach (var number2 in numbers)
                    {
                        if (number1 + number2 == 2020)
                        {
                            return number1 * number2;
                        }
                    }
                }
                throw new Exception("Could not find result!");
            }
        }
    }
}
