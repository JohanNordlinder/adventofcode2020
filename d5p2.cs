using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D5P2
    {

        [TestMethod]
        public void TestRun()
        {
            Assert.AreEqual(357, new Program().getSeatId("FBFBBFFRLR"));
            Assert.AreEqual(567, new Program().getSeatId("BFFFBBFRRR"));
            Assert.AreEqual(119, new Program().getSeatId("FFFBBBFRRR"));
            Assert.AreEqual(820, new Program().getSeatId("BBFFBBFRLL"));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_5.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public int RunChallenge(List<string> input)
            {
                var seatIds = input.Select(z => getSeatId(z)).ToList();
                seatIds.Sort();
                for (int i = seatIds.Min(); i < seatIds.Max(); i++)
                {
                    if (seatIds[i] + 1 != seatIds[i + 1])
                    {
                        return seatIds[i] + 1;
                    }
                }

                throw new Exception("Could not find seat id");
            }

            public int getSeatId(string input)
            {
                return recurse(input.Substring(0, 7), 127) * 8 + recurse(input.Substring(7), 7); ;
            }

            public int recurse(string input, int rows)
            {
                if (input.Length == 1) {
                    if (input[0] == 'F' || input[0] == 'L')
                    {
                        return 0;
                    } else
                    {
                        return 1;
                    }
                };

                var range = rows / 2;

                if (input[0] == 'F' || input[0] == 'L')
                {
                    return recurse(input.Substring(1), range);
                } else
                {
                    return range + range % 2 + recurse(input.Substring(1), range);
                }
            }
        }
    }
}
