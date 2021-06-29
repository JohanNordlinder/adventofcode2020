using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D2P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_2_t_1.txt").ToList();
            Assert.AreEqual(2, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_2.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public int RunChallenge(List<string> input)
            {
                var formated = input.Select(inputString =>
                {
                    var split = inputString.Split('-', ' ', ':');
                    return new
                    {
                        min = Convert.ToInt32(split[0]),
                        max = Convert.ToInt32(split[1]),
                        character = split[2][0],
                        password = split[4],
                    };
                });
                var result = formated.Count(z =>
                {
                    var occurences = z.password.ToCharArray().Count(c => c == z.character);
                    return occurences >= z.min && occurences <= z.max;
                });

                return result;
            }
        }
    }
}
