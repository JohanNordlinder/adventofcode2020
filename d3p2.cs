using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D31P2
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_3_t_1.txt").ToList();
            var program = new Program();
            var map = program.ParseInput(input);
            Assert.AreEqual(2, program.CalculateCollisions(1, 1, map));
            Assert.AreEqual(7, program.CalculateCollisions(3, 1, map));
            Assert.AreEqual(3, program.CalculateCollisions(5, 1, map));
            Assert.AreEqual(4, program.CalculateCollisions(7, 1, map));
            Assert.AreEqual(2, program.CalculateCollisions(1, 2, map));
            Assert.AreEqual(336L, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_3.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public long RunChallenge(List<string> input)
            {
                var map = ParseInput(input);
                var result =
                    CalculateCollisions(1, 1, map) *
                    CalculateCollisions(3, 1, map) *
                    CalculateCollisions(5, 1, map) *
                    CalculateCollisions(7, 1, map) *
                    CalculateCollisions(1, 2, map);

                return result;
            }

            public char[][] ParseInput(List<string> input)
            {
                return input.Select(inputString => inputString.ToCharArray()).ToArray();
            }

            public long CalculateCollisions(int xMovement, int yMovement, char[][] map)
            {
                long treesEncountered = 0;
                int posY = 0;
                int posX = 0;
                int repeatAfter = map[0].Length;
                while (posY < map.Count() - 1)
                {
                    posX += xMovement;
                    posY += yMovement;
                    if (map[posY][posX % repeatAfter] == '#')
                    {
                        treesEncountered++;
                    }
                }

                return treesEncountered;

            }
        }
    }
}
