using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D20P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllText("d_20_t_1.txt");
            Assert.AreEqual(20899048083289, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void TestFlipHorizontal()
        {
            var tile = new Program.Tile
            {
                Id = 1,
                Rows = new List<string> {
                    "###",
                    "#.#",
                    "..."
                }
            };
            var expected = new List<string> {
                "...",
                "#.#",
                "###"
                };

            Program.FlipHorizontal(tile);
            Assert.AreEqual(Environment.NewLine + string.Join(Environment.NewLine, expected), Environment.NewLine + string.Join(Environment.NewLine, tile.Rows));
        }

        [TestMethod]
        public void TestRotate()
        {
            var tile = new Program.Tile
            {
                Id = 1,
                Rows = new List<string> {
                    "123",
                    "456",
                    "789"
                }
            };
            var expected = new List<string> {
                    "741",
                    "852",
                    "963"
                };

            Program.RotateRight(tile);
            Assert.AreEqual(Environment.NewLine + string.Join(Environment.NewLine, expected), Environment.NewLine + string.Join(Environment.NewLine, tile.Rows));

            var original = new List<string> {
                "123",
                "456",
                "789"
            };
            Program.RotateRight(tile);
            Program.RotateRight(tile);
            Program.RotateRight(tile);
            Assert.AreEqual(Environment.NewLine + string.Join(Environment.NewLine, original), Environment.NewLine + string.Join(Environment.NewLine, tile.Rows));
        }

            [TestMethod]
        public void TestDoesTilesFitTogether()
        {
            var t1 = new Program.Tile
            {
                Id = 1,
                Rows = new List<string> {
                    ".#.",
                    "..#",
                    "###"
                }
            };
            var t2 = new Program.Tile
            {
                Id = 2,
                Rows = new List<string> {
                    "..#",
                    "..#",
                    "..#"
                }
            };

            Assert.AreEqual(true, Program.DoesTilesFitTogether(t1, t2));

            var t3 = new Program.Tile
            {
                Id = 3,
                Rows = new List<string> {
                    "###",
                    "#.#",
                    "###"
                }
            };
            var t4 = new Program.Tile
            {
                Id = 4,
                Rows = new List<string> {
                    "...",
                    ".#.",
                    "..."
                }
            };

            Assert.AreEqual(false, Program.DoesTilesFitTogether(t3, t4));
        }

        [TestMethod]
        public void FitWithNumberOfOtherTiles_ShouldRestoreObjectsAfterCompare()
        {
            var tile1 = new Program.Tile
            {
                Id = 1,
                Rows = new List<string> {
                    "#..",
                    "#.#",
                    "#.."
                }
            };

            var tile2 = new Program.Tile
            {
                Id = 1,
                Rows = new List<string> {
                    "#..",
                    "#.#",
                    "#.."
                }
            };

            var expected = new List<string> {
                    "#..",
                    "#.#",
                    "#.."
                };

            Program.DoesTilesFitTogether(tile1, tile2);
            Assert.AreEqual(Environment.NewLine + string.Join(Environment.NewLine, expected), Environment.NewLine + string.Join(Environment.NewLine, tile1.Rows));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllText("d_20.txt");
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public class Tile
            {
                public int Id { get; set; }
                public List<string> Rows { get; set; }
            }

            public long RunChallenge(string input)
            {
                var tiles = ParseTiles(input);

                var cornerIds = new List<long>();

                foreach (var t1 in tiles)
                {
                    var fitWithNumberOfOtherTiles = 0;
                    foreach (var t2 in tiles)
                    {
                        if (t1.Id == t2.Id)
                        {
                            continue;
                        }

                        if (DoesTilesFitTogether(t1, t2))
                        {
                            fitWithNumberOfOtherTiles++;
                        }
                    }

                    if (fitWithNumberOfOtherTiles == 2)
                    {
                        cornerIds.Add(t1.Id);
                    }
                }

                return cornerIds.Aggregate((v1, v2) => v1 * v2);
            }

            List<Tile> ParseTiles(string lines)
            {
                var tileInput = lines.Split(Environment.NewLine + Environment.NewLine);
                return tileInput.Select(ti => ParseTile(ti.Split(Environment.NewLine).ToList())).ToList();
            }

            Tile ParseTile(List<string> lines)
            {
                return new Tile
                {
                    Id = int.Parse(lines.First().Split(new string[] { "Tile ", ":" }, StringSplitOptions.RemoveEmptyEntries)[0]),
                    Rows = lines.Skip(1).ToList()
                };
            }

            public static void FlipHorizontal(Tile tile)
            {
                tile.Rows.Reverse();
            }

            public static void RotateRight(Tile tile)
            {
                var newRows = new List<string>();

                for (int i = tile.Rows.Count - 1; i >= 0; i--)
                {
                    var newLine = string.Empty;
                    for (int j = tile.Rows[0].Length - 1; j >= 0; j--)
                    {
                        newLine += tile.Rows[j][i];
                    }
                    newRows.Add(newLine);
                }
                newRows.Reverse();
                tile.Rows = newRows;
            }

            public static bool DoesTilesFitTogether(Tile t1, Tile t2)
            {
                var doesFit = false;

                // No flip
                doesFit = doesFit || DoesTilesFitTogetherIfRotated(t1, t2);
                // Flip first
                FlipHorizontal(t1);
                doesFit = doesFit || DoesTilesFitTogetherIfRotated(t1, t2);
                // Flip both
                FlipHorizontal(t2);
                doesFit = doesFit || DoesTilesFitTogetherIfRotated(t1, t2);
                // Flip only second
                FlipHorizontal(t1);
                doesFit = doesFit || DoesTilesFitTogetherIfRotated(t1, t2);
                // Restore second
                FlipHorizontal(t2);

                return doesFit;
            }

            public static bool DoesTilesFitTogetherIfRotated(Tile t1, Tile t2)
            {
                var doesFit = false;
                for (int i = 0; i < 4; i++)
                {
                    RotateRight(t1);
                    for (int y = 0; y < 4; y++)
                    {
                        RotateRight(t2);
                        if (t1.Rows[0].Equals(t2.Rows[0]))
                        {
                            doesFit = true;
                        }
                    }
                }

                return doesFit;
            }
        }
    }
}
