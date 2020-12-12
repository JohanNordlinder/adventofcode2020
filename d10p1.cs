using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D10P1
    {

        [TestMethod]
        public void TestRun_1()
        {
            var input = System.IO.File.ReadAllLines("d_10_t_1.txt").ToList();
            Assert.AreEqual(7 * 5, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void TestRun_2()
        {
            var input = System.IO.File.ReadAllLines("d_10_t_2.txt").ToList();
            Assert.AreEqual(22 * 10, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_10.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public int RunChallenge(List<string> input)
            {
                var adapters = input.Select(z => Convert.ToInt32(z)).ToList();
                adapters.Sort();
                var differences = new List<int>();
                var currentVolage = 0;
                while (adapters.Any())
                {

                    var adapter = adapters.First(a => ((a >= currentVolage) && (a <= currentVolage + 3)));
                    differences.Add(adapter - currentVolage);
                    currentVolage = adapter;
                    adapters.Remove(adapter);
                }

                differences.Add(3);

                return differences.Count(z => z == 1) * differences.Count(z => z == 3);
            }
        }
    }
}
