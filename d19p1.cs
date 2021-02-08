using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D19P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllText("d_19_t_1.txt");
            Assert.AreEqual(2, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllText("d_19.txt");
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {

            private class Rule
            {
                public int Number { get; set; }
                public string ExactMatch { get; set; }
                public List<List<int>> RuleGroups = new List<List<int>>();
            }

            private Rule[] rules;

            public int RunChallenge(string input)
            {
                var inputParts = input.Split(Environment.NewLine + Environment.NewLine);
                var rawRules = inputParts[0].Split(Environment.NewLine);
                rules = new Rule[rawRules.Length];
                foreach (var rawRule in rawRules)
                {
                    ParseRule(rawRule);
                }

                var matchingStrings = FindPossibleStrings(0, String.Empty);
                var maxLength = matchingStrings.Max(z => z.Length);

                var messages = inputParts[1].Split(Environment.NewLine).Where(m => m.Length <= maxLength);

                var validCount = messages.Count(m => matchingStrings.Contains(m));

                return validCount;
            }

            private List<string> FindPossibleStrings(int ruleNumber, string previousString)
            {
                var possibleStrings = new List<string>();
                var rule = rules[ruleNumber];

                if (rule.ExactMatch != null)
                {
                    possibleStrings.Add(previousString + rule.ExactMatch);
                } else
                {
                    foreach (var ruleGroup in rule.RuleGroups)
                    {
                        var stringsFromGroup = new List<string>() { previousString };

                        foreach (var ruleNo in ruleGroup)
                        {
                            var possibleStringsFromGroup = new List<string>();
                            foreach (var possibility in stringsFromGroup)
                            {
                                possibleStringsFromGroup.AddRange(FindPossibleStrings(ruleNo, possibility));
                            }
                            stringsFromGroup = possibleStringsFromGroup;
                        }
                        possibleStrings.AddRange(stringsFromGroup);
                    }
                }
                return possibleStrings;
            }

            private void ParseRule(string input)
            {
                var rule = new Rule();
                rule.Number = int.Parse(input.Substring(0, input.IndexOf(':')));
                if (input.Contains("\""))
                {
                    rule.ExactMatch = input.Substring(input.IndexOf('"') + 1, 1);
                } else
                {
                    var groups = input.Split(new string[] { ": ", " | " }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var group in groups.Skip(1))
                    {
                        var numbers = group.Split(' ').Select(v => int.Parse(v));
                        rule.RuleGroups.Add(numbers.ToList());
                    }
                }
                rules[rule.Number] = rule;
            }
        }
    }
}
