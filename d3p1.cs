using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D31P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_3_t_1.txt").ToList();
            Assert.AreEqual(7, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_3.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public int RunChallenge(List<string> input)
            {
                int treesEncountered = 0;
                int posY = 0;
                int posX = 0;
                var formated = input.Select(inputString => inputString.ToCharArray()).ToArray();
                int repeatAfter = formated[0].Length;
                while (posY < formated.Count() - 1)
                {
                    posX += 3;
                    posY += 1;
                    if (formated[posY][posX % repeatAfter] == '#')
                    {
                        treesEncountered++;
                        Console.Out.WriteLine("Tree at y=" + posY + " x=" + posX);
                    }
                }

                return treesEncountered;
            }
        }
    }
}
