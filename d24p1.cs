using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D24P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_24_t_1.txt").ToList();
            Assert.AreEqual(10, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_24.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Tile
        {
            public bool IsBlack { get; set; } = false;
        }

        public static Coordinate Move(Coordinate lastPosition, string direction)
        {
            switch (direction)
            {
                case "w":
                    return new Coordinate { X = lastPosition.X - 2, Y = lastPosition.Y };
                case "e":
                    return new Coordinate { X = lastPosition.X + 2, Y = lastPosition.Y };
                case "ne":
                    return new Coordinate { X = lastPosition.X + 1, Y = lastPosition.Y + 1 };
                case "nw":
                    return new Coordinate { X = lastPosition.X - 1, Y = lastPosition.Y + 1 };
                case "se":
                    return new Coordinate { X = lastPosition.X + 1, Y = lastPosition.Y - 1 };
                case "sw":
                    return new Coordinate { X = lastPosition.X - 1, Y = lastPosition.Y - 1 };
                default:
                    throw new Exception("Invalid Move");
            }
        }

        public class Program
        {

            Dictionary<Coordinate, Tile> Tiles = new Dictionary<Coordinate, Tile>();

            public int RunChallenge(List<string> input)
            {
                foreach (var row in input)
                {
                    var currentCoordinate = new Coordinate { X = 0, Y = 0 };
                    var leftToParseOfRow = row;

                    while (leftToParseOfRow.Length > 0)
                    {
                        var directionToMove = string.Empty;

                        foreach (var direction in new List<string>() { "e", "se", "sw", "w", "nw", "ne" })
                        {
                            if (leftToParseOfRow.StartsWith(direction))
                            {
                                directionToMove = direction;
                                leftToParseOfRow = leftToParseOfRow.Substring(direction.Length);
                                break;
                            }
                        }

                        currentCoordinate = Move(currentCoordinate, directionToMove);
                    }

                    var destinationTile = GetTileCreateIfDoesNotExist(currentCoordinate);
                    destinationTile.IsBlack = !destinationTile.IsBlack;
                }
                return Tiles.Count(t => t.Value.IsBlack);
            }

            private Tile GetTileCreateIfDoesNotExist(Coordinate coordinate)
            {
                Tile tile;
                if (Tiles.TryGetValue(coordinate, out tile))
                {
                    return tile;
                }

                tile = new Tile();
                Tiles.Add(coordinate, tile);
                return tile;
            }
        }
    }
}
