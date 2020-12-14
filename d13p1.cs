using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D13P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_13_t_1.txt").ToList();
            Assert.AreEqual(295, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_13.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public int RunChallenge(List<string> input)
            {
                var time = Convert.ToInt32(input[0]);
                var busses = input[1].Split(',').Where(z => z != "x").Select(z => Convert.ToInt32(z));
                var bussesAndTimes = busses.Select(b => new
                {
                    Timestamp = b,
                    TimeToWait = b * ((time / b) + 1) - time
                });
                var nextBus = bussesAndTimes.Aggregate((b1, b2) => b1.TimeToWait < b2.TimeToWait ? b1 : b2);
                return nextBus.Timestamp * nextBus.TimeToWait;
            }
        }
    }
}
