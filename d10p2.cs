using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D10P2
    {

        [TestMethod]
        public void TestRun_1()
        {
            var input = System.IO.File.ReadAllLines("d_10_t_1.txt").ToList();
            Assert.AreEqual(8L, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void TestRun_2()
        {
            var input = System.IO.File.ReadAllLines("d_10_t_2.txt").ToList();
            Assert.AreEqual(19208L, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_10.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {

            private class Adapter
            {
                public int Number { get; set; }
                public long PathsToHere { get; set; }
            }

            public long RunChallenge(List<string> input)
            {
                var parsedInput = input.Select(z => Convert.ToInt32(z)).ToList();
                parsedInput.Sort();
                var adapters = parsedInput.Select(z => new Adapter { Number = z, PathsToHere = 0 }).ToList();
                var lastAdapter = new Adapter { Number = adapters.Select(z => z.Number).Max() + 3, PathsToHere = 0 };
                adapters.Add(lastAdapter);

                Adapter currentAdapter = adapters.First();
                for (int i = 0; i < adapters.Count; i++)
                {
                    var adapterMatchning = adapters.Where(a => ((a.Number < currentAdapter.Number) && (a.Number >= currentAdapter.Number - 3))).ToList();

                    currentAdapter.PathsToHere = adapterMatchning.Sum(a => a.PathsToHere);

                    if (currentAdapter.Number <= 3)
                    {
                        currentAdapter.PathsToHere++;
                    }

                    if (adapters[i] == lastAdapter)
                    {
                        break;
                    }
                    currentAdapter = adapters[i + 1];
                }

                return lastAdapter.PathsToHere;
            }
        }
    }
}
