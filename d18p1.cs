using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    [TestClass]
    public class D18P1
    {

        [TestMethod]
        public void TestRun()
        {
            Assert.AreEqual(71L, new Program().RunChallenge(new List<string>() { "1 + 2 * 3 + 4 * 5 + 6" }));
            Assert.AreEqual(51L, new Program().RunChallenge(new List<string>() { "1 + (2 * 3) + (4 * (5 + 6))" }));
            Assert.AreEqual(26L, new Program().RunChallenge(new List<string>() { "2 * 3 + (4 * 5)" }));
            Assert.AreEqual(437L, new Program().RunChallenge(new List<string>() { "5 + (8 * 3 + 9 + 3 * 4 * 3)" }));
            Assert.AreEqual(12240L, new Program().RunChallenge(new List<string>() { "5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))" }));
            Assert.AreEqual(13632L, new Program().RunChallenge(new List<string>() { "((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2" }));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_18.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public long RunChallenge(List<string> input)
            {
                long result = 0;

                foreach (var row in input)
                {
                    result += GetResultFromRow(row);
                }

                return result;
            }

            private long GetResultFromRow(string row)
            {
                return Simplyfy(row);
            }

            private long Simplyfy(string row)
            {
                var expression = row;
                var regex = new Regex(@"\(([^()]*)\)");
                Match match;
                do
                {
                    match = regex.Match(expression);
                    if (match.Success)
                    {
                        var result = Simplyfy(match.Groups[0].Value.TrimStart('(').TrimEnd(')'));
                        expression = expression.Replace(match.Groups[0].Value, result.ToString());
                    }

                } while (match.Success);


                var parts = expression.Split(" ");
                var sum = Convert.ToInt64(parts[0]);
                for (int i = 1; i < parts.Count(); i++)
                {
                    switch (parts[i])
                    {
                        case "+":
                            sum += Convert.ToInt64(parts[i + 1]);
                            i++;
                            break;
                        case "*":
                            sum *= Convert.ToInt64(parts[i + 1]);
                            i++;
                            break;
                    }
                }
                return sum;
            }
        }
    }
}
