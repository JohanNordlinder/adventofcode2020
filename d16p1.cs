using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D16P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllText("d_16_t_1.txt");
            Assert.AreEqual(71, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllText("d_16.txt");
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            class ValidNumberRange
            {
                public int Min { get; set; }
                public int Max { get; set; }

                public bool IsNumberValid(int number)
                {
                    return number >= Min && number <= Max;
                }
            }

            class Field
            {
                public string Name { get; set; }
                public List<ValidNumberRange> Range { get; set; }
            }


            public int RunChallenge(string input)
            {
                int result = 0;
                var parts = input.Split(new[] {
                    Environment.NewLine + Environment.NewLine + "your ticket:" + Environment.NewLine,
                    Environment.NewLine + Environment.NewLine + "nearby tickets:" + Environment.NewLine
                    }, StringSplitOptions.RemoveEmptyEntries);
                var fields = parts[0].Split(Environment.NewLine).Select(row =>
                {
                    var parts = row.Split(new[] { ":", " or " }, StringSplitOptions.None);
                    return new Field
                    {
                        Name = parts[0],
                        Range = new List<ValidNumberRange>() {
                            ParseRange(parts[1]), ParseRange(parts[2])
                        }
                    };
                }).ToList();
                var nearbyTickets = parts[2].Split(Environment.NewLine).Select(row => row.Split(',').Select(v => Convert.ToInt32(v)));
                foreach (var ticket in nearbyTickets)
                {
                    foreach (var value in ticket)
                    {
                        if (!fields.Any(f => f.Range.Any(r => r.IsNumberValid(value))))
                        {
                            result += value;
                        }

                    }
                }

                return result;
            }

            private ValidNumberRange ParseRange(string raw)
            {
                var parts = raw.Split('-');
                return new ValidNumberRange { Min = Convert.ToInt32(parts[0]), Max = Convert.ToInt32(parts[1]) };
            }
        }
    }
}
