using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D16P2
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllText("d_16_t_2.txt");
            Assert.AreEqual(12 * 11 * 13, new Program().RunChallenge(input, ""));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllText("d_16.txt");
            Console.WriteLine("Result: " + new Program().RunChallenge(input, "departure"));
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
                public List<int> PossibleFieldPositions = new List<int>();
                public int? FinalFieldPosition { get; set; } = null;

            }


            public long RunChallenge(string input, string fieldmask)
            {
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

                var nearbyTickets = parts[2].Split(Environment.NewLine).Select(row => row.Split(',').Select(v => Convert.ToInt32(v)).ToList());

                var validTickets = nearbyTickets.Where(ticket => ticket.All(value => fields.Any(f => f.Range.Any(r => r.IsNumberValid(value)))));

                foreach (var field in fields)
                {
                    for (int possibleLocation = 0; possibleLocation < fields.Count(); possibleLocation++)
                    {
                        var isMatch = validTickets.All(ticket => field.Range.Any(r => r.IsNumberValid(ticket[possibleLocation])));
                        if (isMatch)
                        {
                            field.PossibleFieldPositions.Add(possibleLocation);
                        }
                    }

                }

                while (fields.Any(f => f.FinalFieldPosition == null))
                {
                    var field = fields.Where(f => f.FinalFieldPosition == null).OrderBy(f => f.PossibleFieldPositions.Count).First();
                    field.FinalFieldPosition = field.PossibleFieldPositions.First();
                    fields.ForEach(f => f.PossibleFieldPositions.Remove((int)field.FinalFieldPosition));
                }

                var fieldsWeCareAbout = fields.Where(f => f.Name.StartsWith(fieldmask));
                var myTicket = parts[1].Split(',').Select(v => Convert.ToInt32(v)).ToList();

                long result = 1;
                foreach (var field in fieldsWeCareAbout)
                {
                    result *= myTicket[(int)field.FinalFieldPosition];
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
