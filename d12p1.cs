using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D12P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_12_t_1.txt").ToList();
            Assert.AreEqual(25, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_12.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            private class Ship
            {
                public Direction Direction { get; set; } = Direction.RIGHT;
                public Coordinate Position { get; set; } = new Coordinate();
            }

            public int RunChallenge(List<string> instructions)
            {
                var ship = new Ship();
                foreach (var instruction in instructions)
                {
                    var value = int.Parse(instruction.Substring(1));
                    switch (instruction[0])
                    {
                        case 'N':
                            ship.Position.Y += value;
                            break;
                        case 'S':
                            ship.Position.Y -= value;
                            break;
                        case 'E':
                            ship.Position.X += value;
                            break;
                        case 'W':
                            ship.Position.X -= value;
                            break;
                        case 'F':
                            for (int i = 0; i < value; i++)
                            {
                                ship.Position = CoordinateMovement.Move(ship.Position, ship.Direction);
                            }
                            break;
                        case 'L':
                            for (int i = 0; i < value / 90; i++)
                            {
                                ship.Direction = DirectionTurns.TurnLeft(ship.Direction);
                            }
                            break;
                        case 'R':
                            for (int i = 0; i < value / 90; i++)
                            {
                                ship.Direction = DirectionTurns.TurnRight(ship.Direction);
                            }
                            break;

                    }
                }
                return Math.Abs(ship.Position.X) + Math.Abs(ship.Position.Y);
            }
        }
    }
}
