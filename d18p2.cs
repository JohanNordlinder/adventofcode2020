using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    [TestClass]
    public class D18P2
    {

        [TestMethod]
        public void TestRun()
        {
            Assert.AreEqual(231L, new Program().RunChallenge(new List<string>() { "1 + 2 * 3 + 4 * 5 + 6" }));
            Assert.AreEqual(51L, new Program().RunChallenge(new List<string>() { "1 + (2 * 3) + (4 * (5 + 6))" }));
            Assert.AreEqual(46L, new Program().RunChallenge(new List<string>() { "2 * 3 + (4 * 5)" }));
            Assert.AreEqual(1445L, new Program().RunChallenge(new List<string>() { "5 + (8 * 3 + 9 + 3 * 4 * 3)" }));
            Assert.AreEqual(669060L, new Program().RunChallenge(new List<string>() { "5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))" }));
            Assert.AreEqual(23340L, new Program().RunChallenge(new List<string>() { "((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2" }));
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
                var regeg = new Regex(@"\(([^()]*)\)");
                Match match;
                do
                {
                    match = regeg.Match(expression);
                    if (match.Success)
                    {
                        var result = Simplyfy(match.Groups[0].Value.TrimStart('(').TrimEnd(')'));
                        expression = expression.Replace(match.Groups[0].Value, result.ToString());
                    }

                } while (match.Success);


                return SimplifyFlat(expression);
            }

            private long SimplifyFlat(string row)
            {
                var expression = row;
                var regeg = new Regex(@"\d* \+ \d*");
                Match match;
                do
                {
                    match = regeg.Match(expression);
                    if (match.Success)
                    {
                        var toParse = match.Groups[0].Value.Split(" + ");
                        long result = Convert.ToInt64(toParse[0]) + Convert.ToInt64(toParse[1]);
                        expression = expression.ReplaceFirst(match.Groups[0].Value, result.ToString());
                    }

                } while (match.Success);

                var parts = expression.Split(" ");
                var sum = Convert.ToInt64(parts[0]);
                for (int i = 1; i < parts.Count(); i++)
                {
                    if (parts[i] == "*")
                    {
                        sum *= Convert.ToInt64(parts[i + 1]);
                        i++;
                    }
                }

                return sum;
            }
        }
    }
}
