using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D24P2
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_24_t_1.txt").ToList();
            Assert.AreEqual(2208, new Program().RunChallenge(input));
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
            Dictionary<Coordinate, Tile> NextTiles = new Dictionary<Coordinate, Tile>();

            List<string> AllDirections = new List<string>() { "e", "se", "sw", "w", "nw", "ne" };

            public int RunChallenge(List<string> input)
            {
                foreach (var row in input)
                {
                    var currentCoordinate = new Coordinate { X = 0, Y = 0 };
                    var leftToParseOfRow = row;

                    while (leftToParseOfRow.Length > 0)
                    {
                        var directionToMove = string.Empty;

                        foreach (var direction in AllDirections)
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

                for (int i = 0; i < 100; i++)
                {
                    // Expand known tiles by one coordinate in each direction
                    foreach (var coordinate in Tiles.Keys.ToList())
                    {
                        FindNumberOfAdjacentBlackSquares(coordinate);
                    }

                    foreach (var tile in Tiles.ToList())
                    {
                        var adjacentBlack = FindNumberOfAdjacentBlackSquares(tile.Key);
                        var nextTile = new Tile();
                        if (tile.Value.IsBlack && (adjacentBlack == 0 || adjacentBlack > 2))
                        {
                            nextTile.IsBlack = false;
                        }
                        else if (!tile.Value.IsBlack && adjacentBlack == 2)
                        {
                            nextTile.IsBlack = true;
                        }
                        else
                        {
                            nextTile.IsBlack = tile.Value.IsBlack;
                        }
                        NextTiles.Add(tile.Key, nextTile);
                    }

                    Tiles = NextTiles;
                    NextTiles = new Dictionary<Coordinate, Tile>();
                    // Debug.WriteLine("Day {0}: {1}", i + 1, Tiles.Count(t => t.Value.IsBlack));
                }

                return Tiles.Count(t => t.Value.IsBlack);
            }

            private int FindNumberOfAdjacentBlackSquares(Coordinate coordinate)
            {
                return AllDirections.Count(d => GetTileCreateIfDoesNotExist(Move(coordinate, d)).IsBlack);
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
