using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D4P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_4_t_1.txt").ToList();
            Assert.AreEqual(2, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_4.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public int RunChallenge(List<string> input)
            {
                var passPorts = new List<string>();
                var currentPassport = string.Empty;
                foreach(var passportRow in input)
                {
                    if (passportRow.Length == 0)
                    {
                        passPorts.Add(currentPassport);
                        currentPassport = string.Empty;
                    } else
                    {
                        currentPassport += (" " + passportRow);
                    }
                }
                passPorts.Add(currentPassport);

                var result = passPorts.Count(z => isValid(z));

                return result;
            }

            private bool isValid(string input)
            {
                var split = input.Split(' ');
                bool byr = split.Any(s => s.Contains("byr"));
                bool iyr = split.Any(s => s.Contains("iyr"));
                bool eyr = split.Any(s => s.Contains("eyr"));
                bool hgt = split.Any(s => s.Contains("hgt"));
                bool hcl = split.Any(s => s.Contains("hcl"));
                bool ecl = split.Any(s => s.Contains("ecl"));
                bool pid = split.Any(s => s.Contains("pid"));
                bool cid = split.Any(s => s.Contains("cid"));

                return byr && iyr && eyr && hgt && hcl && ecl && pid;
            }
        }
    }
}
