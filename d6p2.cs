using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    [TestClass]
    public class D6P2
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllText("d_6_t_1.txt");
            Assert.AreEqual(6, new Program().RunChallenge(input));
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
                    .Select(group => new
                    {
                        personAnswers = group.Split(Environment.NewLine),
                        distinctAnswersInGroup = group.ToCharArray().Distinct()
                    });

                return groups.Sum(group => group.distinctAnswersInGroup.Count(answer => group.personAnswers.All(personAnswer => personAnswer.Contains(answer))));
            }

        }
    }
}
