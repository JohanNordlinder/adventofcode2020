using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    [TestClass]
    public class D19P2
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllText("d_19_t_2.txt");
            Assert.AreEqual(12, new Program().RunChallenge(input));
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
                rules = new Rule[200];
                foreach (var rawRule in rawRules)
                {
                    ParseRule(rawRule);
                }

                var messages = inputParts[1].Split(Environment.NewLine);

                var rule42 = FindPossibleStrings(42, String.Empty);
                var rule31 = FindPossibleStrings(31, String.Empty);

                var validCount = 0;

                foreach (var message in messages)
                {
                    // Rule 0 is rule 8 followed by rule 11
                    bool rule42Passed = false;
                    bool rule31Passed = false;
                    string messageWithoutRule42 = string.Empty;

                    // Rule 8 is any of the strings in rule 42 in any combination of any length greater than 1
                    var rule42regexExpression = @"^(";
                    foreach (var option in rule42)
                    {
                        rule42regexExpression += "(" + option + ")|";
                    }
                    var rule42Matcher = new Regex(rule42regexExpression.TrimEnd('|') + ")+");
                    var rule42Match = rule42Matcher.Match(message);
                    if (rule42Match.Success)
                    {
                        messageWithoutRule42 = message.Substring(rule42Match.Value.Length);
                        rule42Passed = true;
                    }

                    // Rule 11 is a string starting with any combination of the strings in rule 42 of any length greater than 1
                    // and then the same count of any combination of the strings in rule 31
                    var rule31regexExpression = @"^(";
                    foreach (var option in rule31)
                    {
                        rule31regexExpression += "(" + option + ")|";
                    }
                    var rule31Matcher = new Regex(rule31regexExpression.TrimEnd('|') + ")+$");
                    var match31Match = rule31Matcher.Match(messageWithoutRule42);
                    if (match31Match.Success)
                    {
                        rule31Passed = true;
                    }

                    // Check if both rules passed and verify that the count of rule 31 matches can never be less than rule 42 matches
                    if (rule42Passed && rule31Passed && rule42Match.Value.Length > match31Match.Value.Length)
                    {
                        Debug.WriteLine(message);
                        validCount++;
                    }
                }

                return validCount;
            }

            private List<string> FindPossibleStrings(int ruleNumber, string previousString)
            {
                var possibleStrings = new List<string>();

                var rule = rules[ruleNumber];

                if (rule.ExactMatch != null)
                {
                    possibleStrings.Add(previousString + rule.ExactMatch);
                }
                else
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
                }
                else
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
