using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace AdventOfCode2020
{
    [TestClass]
    public class D12P2
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_12_t_1.txt").ToList();
            Assert.AreEqual(286, new Program().RunChallenge(input));
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
                var waypoint = new Coordinate { X = 10, Y = 1 };

                Action debugPrintout = () =>
                {
                    Debug.WriteLine(Environment.NewLine + $"Ship X:{ship.Position.X} Y:{ ship.Position.Y}");
                    Debug.WriteLine($"Waypoint X:{waypoint.X} Y:{ waypoint.Y}");
                };

                foreach (var instruction in instructions)
                {
                    var value = int.Parse(instruction.Substring(1));

                    switch (instruction[0])
                    {
                        case 'N':
                            waypoint.Y += value;
                            break;
                        case 'S':
                            waypoint.Y -= value;
                            break;
                        case 'E':
                            waypoint.X += value;
                            break;
                        case 'W':
                            waypoint.X -= value;
                            break;
                        case 'F':
                            for (int i = 0; i < value; i++)
                            {
                                var localCoordinate = translateWorldCoordinateToShipLocalCoordinate(waypoint, ship.Position);
                                ship.Position.X += localCoordinate.X;
                                ship.Position.Y += localCoordinate.Y;
                                waypoint.X += localCoordinate.X;
                                waypoint.Y += localCoordinate.Y;
                            }
                            break;
                        case 'L':
                            for (int i = 0; i < value / 90; i++)
                            {
                                var localCoordinate = translateWorldCoordinateToShipLocalCoordinate(waypoint, ship.Position);
                                var translatedLocalCoordinates = new Coordinate { X = -localCoordinate.Y, Y = localCoordinate.X };
                                waypoint = translateShipLocalCoordinateToWorldCoordinates(translatedLocalCoordinates, ship.Position);
                            }
                            break;
                        case 'R':
                            for (int i = 0; i < value / 90; i++)
                            {
                                var localCoordinate = translateWorldCoordinateToShipLocalCoordinate(waypoint, ship.Position);
                                var translatedLocalCoordinates = new Coordinate { X = localCoordinate.Y, Y = -localCoordinate.X };
                                waypoint = translateShipLocalCoordinateToWorldCoordinates(translatedLocalCoordinates, ship.Position);
                            }
                            break;
                    }
                    debugPrintout();
                }
                return Math.Abs(ship.Position.X) + Math.Abs(ship.Position.Y);
            }

            private Coordinate translateWorldCoordinateToShipLocalCoordinate(Coordinate worldCoordinate, Coordinate ShipCoordinate)
            {
                return new Coordinate { X = worldCoordinate.X - ShipCoordinate.X, Y = worldCoordinate.Y - ShipCoordinate.Y };
            }

            private Coordinate translateShipLocalCoordinateToWorldCoordinates(Coordinate localCoordinate, Coordinate ShipCoordinate)
            {
                return new Coordinate { X = localCoordinate.X + ShipCoordinate.X, Y = localCoordinate.Y + ShipCoordinate.Y };
            }
        }
    }
}
