using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D7P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_7_t_1.txt").ToList();
            Assert.AreEqual(4, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_7.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public int RunChallenge(List<string> input)
            {
                var bags = input.Select(i => i.TrimEnd('.')).Select(i =>
                {
                    IEnumerable<ContainingBag> containsBags;
                    if (i.Contains("no other bags"))
                    {
                        containsBags = new List<ContainingBag>();
                    } else
                    {
                        containsBags = i.Split("contain")[1].Split(',').Select(contains => {
                            var number = Convert.ToInt32(contains.Trim().Split(' ')[0]);
                            return new ContainingBag
                            {
                                Number = number,
                                BagName = contains.Substring(2).ToLower().Trim() + (number == 1 ? "s" : "")
                            };
                        });
                    }
                    
                    return new Bag {
                        BagName = i.Split("contain")[0].ToLower().Trim(),
                        ContainsBags = containsBags.ToList() };
                }).ToList();

                var solution = HowManyCanHoldThisBag("shiny gold bags", bags);

                return solution;
            }

            private int HowManyCanHoldThisBag(string bagName, List<Bag> bags)
            {
                var matchingBags = bags.Where(z => z.ContainsBags.Any(containing => containing.BagName == bagName)).ToList();
                matchingBags.ForEach(b => bags.Remove(b));
                return matchingBags.Count() + matchingBags.Sum(bag => HowManyCanHoldThisBag(bag.BagName, bags));
            }
        }

        private class Bag
        {
            public string BagName { get; set; }
            public List<ContainingBag> ContainsBags { get; set; }
        }

        private class ContainingBag
        {
            public int Number { get; set; }
            public string BagName { get; set; }
        }
    }
}
