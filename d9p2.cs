using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D9P2
    {
        
        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_9_t_1.txt").ToList();
            Assert.AreEqual(62, new Program().RunChallenge(input, 5, 127));
        }
        
        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_9.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input, 25, 1038347917));
        }
        
        public class Program
        {
            public long RunChallenge(List<string> input, int preambleLength, int invalidNumber)
            {
                var numbers = input.Select(z => Convert.ToInt64(z)).ToList();

                for (var possibleStart = 0; possibleStart < input.Count(); possibleStart++)
                {
                    for(int possibleLength = 1; possibleLength < input.Count() - possibleStart; possibleLength++) { 

                        var range = numbers.Skip(possibleStart).Take(possibleLength);
                        if(range.Sum() == invalidNumber) {
                            return range.Min() + range.Max();
                        }
                    }
                }
                throw new Exception("Failed to find solution");
            }
        }
    }
}
