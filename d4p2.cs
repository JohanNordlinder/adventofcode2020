using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    [TestClass]
    public class D4P2
    {

        [TestMethod]
        public void TestRun()
        {
            Assert.AreEqual(true, new Program().byr("2002"));
            Assert.AreEqual(false, new Program().byr("2003"));

            Assert.AreEqual(true, new Program().hgt("60in"));
            Assert.AreEqual(true, new Program().hgt("190cm"));
            Assert.AreEqual(false, new Program().hgt("190in"));
            Assert.AreEqual(false, new Program().hgt("190"));

            Assert.AreEqual(true, new Program().hcl("#123abc"));
            Assert.AreEqual(false, new Program().hcl("#123abz"));
            Assert.AreEqual(false, new Program().hcl("123abc"));

            Assert.AreEqual(true, new Program().ecl("brn"));
            Assert.AreEqual(false, new Program().ecl("wat"));

            Assert.AreEqual(true, new Program().pid("000000001"));
            Assert.AreEqual(false, new Program().pid("0123456789"));

            var input = System.IO.File.ReadAllText("d_4_t_2.txt");
            Assert.AreEqual(0, new Program().RunChallenge(input));

            input = System.IO.File.ReadAllText("d_4_t_3.txt");
            Assert.AreEqual(4, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllText("d_4.txt");
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public int RunChallenge(string rawInput)
            {
                var result = rawInput
                    .Split(Environment.NewLine + Environment.NewLine)
                    .Count(isValidPassport);

                return result;
            }

            private bool isValidPassport(string passport)
            {
                var validations = new List<Func<string, bool>> {
                    byr,
                    iyr,
                    eyr,
                    hgt,
                    hcl,
                    ecl,
                    pid,
                    //cid
                };

                var passportParts = passport
                    .Split(new[] { " ", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(partRaw =>
                {
                    var fields = partRaw.Split(':');
                    return new
                    {
                        param = fields[0],
                        value = fields[1]
                    };
                });

                var isValid = validations.All(
                    validation => passportParts.Any(
                        part => part.param == validation.Method.Name && validation.Invoke(part.value)));

                return isValid;
            }

            public bool iyr(string value)
            {
                var data = Convert.ToInt32(value);
                return data >= 2010 && data <= 2020;
            }

            public bool eyr(string value)
            {
                var data = Convert.ToInt32(value);
                return data >= 2020 && data <= 2030;
            }

            public bool byr(string value)
            {
                var data = Convert.ToInt32(value);
                return data >= 1920 && data <= 2002;
            }

            public bool hgt(string value)
            {
                if (value.Contains("cm"))
                {
                    var parsed = Convert.ToInt32(value.Replace("cm", ""));
                    return parsed >= 150 && parsed <= 193;
                }
                else
                {
                    var parsed = Convert.ToInt32(value.Replace("in", ""));
                    return parsed >= 59 && parsed <= 76;
                }
            }

            public bool hcl(string value)
            {
                return new Regex("^#[0-9a-f]{6}$").IsMatch(value);
            }

            public bool ecl(string value)
            {
                var validValues = new List<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
                return validValues.Contains(value);
            }

            public bool pid(string value)
            {
                return new Regex("^[0-9]{9}$").IsMatch(value);
            }
            public bool cid(string value)
            {
                return true;
            }
        }
    }
}
