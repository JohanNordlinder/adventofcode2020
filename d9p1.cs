using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D9P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_9_t_1.txt").ToList();
            Assert.AreEqual(127, new Program().RunChallenge(input, 5));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_9.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input, 25));
        }

        public class Program
        {
            public long RunChallenge(List<string> input, int preambleLength)
            {
                var numbers = input.Select(z => Convert.ToInt64(z)).ToList();

                for (var currentIndex = preambleLength; currentIndex < input.Count(); currentIndex++)
                {
                    var currentNumber = numbers[currentIndex];
                    var possibleParts = numbers.Skip(currentIndex - preambleLength).Take(preambleLength);
                    if (!possibleParts.Any(n1 => possibleParts.Any(n2 => n1 != n2 && n1 + n2 == currentNumber)))
                    {
                        return currentNumber;
                    }
                }
                throw new Exception("Failed to find solution");
            }
        }
    }
}
