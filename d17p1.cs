using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D17P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_17_t_1.txt").ToList();
            Assert.AreEqual(112, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_17.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public Dictionary<Coordinate, bool> Cubes { get; set; } = new Dictionary<Coordinate, bool>();
            public Dictionary<Coordinate, bool> NextCubes { get; set; }

            public int RunChallenge(List<string> input)
            {
                // Also consider qubes next to initial state
                input.Insert(0, new String('.', input[0].Length));
                input.Add(new String('.', input[0].Length));
                input = input.Select(s => s = '.' + s + '.').ToList();

                for (int y = 0; y < input.Count; y++)
                {
                    for (int x = 0; x < input[0].Length; x++)
                    {
                        var active = input[y][x] == '#';
                        Cubes.Add(new Coordinate { X = x, Y = y, Z = -1 }, false);
                        Cubes.Add(new Coordinate { X = x, Y = y, Z = 0 }, active);
                        Cubes.Add(new Coordinate { X = x, Y = y, Z = 1 }, false);
                    }
                }

                for (int cycle = 0; cycle < 6; cycle++)
                {
                    NextCubes = new Dictionary<Coordinate, bool>();
                    foreach (var cube in Cubes)
                    {
                        var neighbors = GetNeighbors(cube.Key).Count(n => n);
                        if (cube.Value)
                        {
                            if (neighbors == 2 || neighbors == 3)
                            {
                                NextCubes.Add(cube.Key, true);
                            }
                            else
                            {
                                NextCubes.Add(cube.Key, false);
                            }
                        }
                        else
                        {
                            if (neighbors == 3)
                            {
                                NextCubes.Add(cube.Key, true);
                            }
                            else
                            {
                                NextCubes.Add(cube.Key, false);
                            }
                        }
                    }
                    Cubes = NextCubes;
                }
                return Cubes.Values.Count(z => z);
            }

            private IEnumerable<bool> GetNeighbors(Coordinate cord)
            {
                var values = new List<bool>();
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        for (int z = -1; z <= 1; z++)
                        {
                            var coordinate = new Coordinate
                            {
                                X = cord.X + x,
                                Y = cord.Y + y,
                                Z = cord.Z + z
                            };
                            if (!coordinate.Equals(cord))
                            {
                                values.Add(GetCube(coordinate));
                            }
                        }
                    }
                }

                return values;
            }

            private bool GetCube(Coordinate coordinate)
            {
                bool active;
                if (Cubes.TryGetValue(coordinate, out active))
                {
                    return active;
                }

                if (!NextCubes.TryGetValue(coordinate, out _))
                {
                    NextCubes.Add(coordinate, false);
                }

                return false;
            }
        }
    }
}
