using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D13P2
    {

        [TestMethod]
        public void TestRun()
        {
            Assert.AreEqual(1068781L, new Program().RunChallenge_Old("7,13,x,x,59,x,31,19"));
            Assert.AreEqual(1068781L, new Program().RunChallenge("7,13,x,x,59,x,31,19"));

            Assert.AreEqual(3417, new Program().RunChallenge_Old("17,x,13,19"));
            Assert.AreEqual(3417, new Program().RunChallenge("17,x,13,19"));

            Assert.AreEqual(754018, new Program().RunChallenge_Old("67,7,59,61"));
            Assert.AreEqual(754018, new Program().RunChallenge("67,7,59,61"));

            Assert.AreEqual(779210, new Program().RunChallenge_Old("67,x,7,59,61"));
            Assert.AreEqual(779210, new Program().RunChallenge("67,x,7,59,61"));

            Assert.AreEqual(1261476, new Program().RunChallenge_Old("67,7,x,59,61"));
            Assert.AreEqual(1261476, new Program().RunChallenge("67,7,x,59,61"));

            Assert.AreEqual(1202161486, new Program().RunChallenge_Old("1789,37,47,1889"));
            Assert.AreEqual(1202161486, new Program().RunChallenge("1789,37,47,1889"));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_13.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input[1]));
        }

        public class Program
        {

            public long RunChallenge(string input)
            {
                var raw = input.Split(',').Select((z, index) => z != "x" ? Convert.ToInt64(z) : -1).ToList();
                var busses = raw.Select((b, index) => new { Timestamp = b, Offset = index }).Where(b => b.Timestamp != -1).ToList();

                var timeJumpSize = 1L;
                var time = 0L;
                foreach (var buss in busses)
                {
                    while ((time + buss.Offset) % buss.Timestamp != 0L)
                    {
                        time += timeJumpSize;
                    }
                    timeJumpSize *= buss.Timestamp;
                }
                return time;
            }

            public long RunChallenge_Old(string input)
            {
                var busses = input.Split(',').Select(z => z != "x" ? Convert.ToInt64(z) : -1).ToList();

                var max = busses.Max();
                var offset = busses.IndexOf(max);

                long time = 0;
                bool allDepart;
                while (true)
                {
                    time += max;
                    allDepart = true;
                    var timeToCompare = time - offset;
                    for (int i = 0; i < busses.Count; i++)
                    {
                        if ((busses[i] != -1) && (timeToCompare + i) % busses[i] != 0)
                        {
                            allDepart = false;
                            break;
                        }
                    }
                    if (allDepart)
                    {
                        return timeToCompare;
                    }
                }
            }
        }
    }
}
