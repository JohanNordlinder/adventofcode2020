using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    [TestClass]
    public class D6P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllText("d_6_t_1.txt");
            Assert.AreEqual(11, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllText("d_6.txt");
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public int RunChallenge(string rawInput)
            {
                var groups = rawInput
                    .Split(Environment.NewLine + Environment.NewLine)
                    .Select(group => group.Replace(Environment.NewLine, string.Empty));

                return groups.Sum(group => group.ToCharArray().Distinct().Count());
            }
        }
    }
}
