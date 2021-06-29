using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    [TestClass]
    public class D20P2
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllText("d_20_t_1.txt");
            Assert.AreEqual(273, new Program().RunChallenge(input));
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
            #region flip square
           
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

            #endregion

            #region flip 360

            var original = new List<string> {
                "123",
                "456",
                "789"
            };
            Program.RotateRight(tile);
            Program.RotateRight(tile);
            Program.RotateRight(tile);
            Assert.AreEqual(Environment.NewLine + string.Join(Environment.NewLine, original), Environment.NewLine + string.Join(Environment.NewLine, tile.Rows));

            #endregion

            #region flip rectangle

            tile = new Program.Tile
            {
                Id = 1,
                Rows = new List<string> {
                    "01234",
                    "56789",
                }
            };
            expected = new List<string> {
                    "50",
                    "61",
                    "72",
                    "83",
                    "94"
                };

            Program.RotateRight(tile);
            Assert.AreEqual(Environment.NewLine + string.Join(Environment.NewLine, expected), Environment.NewLine + string.Join(Environment.NewLine, tile.Rows));

            #endregion
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

            Assert.AreEqual(true, Program.CanBeRotatedUntilTopSidesAreEqual(t1, t2));

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

            Assert.AreEqual(false, Program.CanBeRotatedUntilTopSidesAreEqual(t3, t4));
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
                public bool HasBeenMergedIntoOther { get; set; }
            }

            private Tile SeaMonster = new Tile
            {
                Id = 1337,
                Rows = new List<string> {
                        "                  # ",
                        "#    ##    ##    ###",
                        " #  #  #  #  #  #   "
                    }
            };

            private void PrintTile(Tile tile)
            {
                Debug.Write(Environment.NewLine + Environment.NewLine + string.Join(Environment.NewLine, tile.Rows));
            }

            public long RunChallenge(string input)
            {
                var tiles = ParseTiles(input);

                while (tiles.Count(t => !t.HasBeenMergedIntoOther) > 1)
                {
                    foreach (var t1 in tiles.Where(t => !t.HasBeenMergedIntoOther))
                    {
                        bool oneWasMerged;
                        do
                        {
                            oneWasMerged = false;
                            foreach (var t2 in tiles)
                            {
                                if (t1.Id == t2.Id || t2.HasBeenMergedIntoOther)
                                {
                                    continue;
                                }

                                if (CanBeRotatedUntilTopSidesAreEqual(t1, t2))
                                {
                                    t1.Rows.RemoveAt(0);
                                    FlipHorizontal(t1);
                                    t2.Rows.RemoveAt(0);
                                    t1.Rows.AddRange(t2.Rows);
                                    t2.HasBeenMergedIntoOther = true;
                                    oneWasMerged = true;
                                }
                            }
                        } while (oneWasMerged);
                    }
                }

                var finalTile = tiles.First(t => !t.HasBeenMergedIntoOther);

                FlipHorizontal(finalTile);
                StripCornersFromTile(finalTile);

                var tags = CountHashCharsInTile(finalTile);

                int numberOfSeaMonters = 0;

                for (int i = 1; i <= 8; i++)
                {
                    numberOfSeaMonters = FindNumberOfSeaMonsters(finalTile);
                    if (numberOfSeaMonters > 0)
                    {
                        break;
                    }

                    RotateRight(finalTile);
                    if (i == 4)
                    {
                        FlipHorizontal(finalTile);
                    }
                }

                PrintTile(finalTile);

                return tags - numberOfSeaMonters * CountHashCharsInTile(SeaMonster);
            }

            private int FindNumberOfSeaMonsters(Tile finalTile)
            {
                var monsterFound = 0;
                var seaMonsterLength = SeaMonster.Rows[0].Length;
                var seaMonsterAsRegex = SeaMonster.Rows.Select(row => new Regex(row.Replace(" ", "."))).ToList();
                for (int i = 1; i < finalTile.Rows.Count - 1; i++)
                {
                    var middlerow = finalTile.Rows[i];
                    for (int index = 0; index < middlerow.Length - seaMonsterLength; index++)
                    {
                        var headMatch = seaMonsterAsRegex[0].IsMatch(finalTile.Rows[i - 1].Substring(index, seaMonsterLength));
                        var bodyMatch = seaMonsterAsRegex[1].IsMatch(middlerow.Substring(index, seaMonsterLength));
                        var lowerBodyMatch = seaMonsterAsRegex[2].IsMatch(finalTile.Rows[i + 1].Substring(index, seaMonsterLength));
                        if (headMatch && bodyMatch && lowerBodyMatch)
                        {
                            monsterFound++;
                        }
                    }
                }

                return monsterFound;
            }

            private void StripCornersFromTile(Tile tile)
            {
                tile.Rows.RemoveAt(0);
                tile.Rows.RemoveAt(tile.Rows.Count - 1);
                tile.Rows = tile.Rows.Select(r => r.Substring(1, r.Length - 2)).ToList();
            }

            private int CountHashCharsInTile(Tile tile)
            {
                return tile.Rows.SelectMany(r => r).Count(s => s == '#');
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

                for (int j = 0; j < tile.Rows[0].Length; j++)
                {
                    var newLine = string.Empty;
                    for (int i = tile.Rows.Count - 1; i >= 0; i--)
                    {
                        newLine += tile.Rows[i][j];
                    }
                    newRows.Add(newLine);
                }
                tile.Rows = newRows;
            }

            public static bool CanBeRotatedUntilTopSidesAreEqual(Tile t1, Tile t2)
            {
                var flipCombo = new List<Action>();

                // No flip
                flipCombo.Add(() => { });
                // First flipped
                flipCombo.Add(() => FlipHorizontal(t1));
                // Both flipped
                flipCombo.Add(() => FlipHorizontal(t2));
                // Restore first, only second flipped
                flipCombo.Add(() => FlipHorizontal(t1));
                // Restore second
                flipCombo.Add(() => FlipHorizontal(t2));

                foreach (Action flip in flipCombo)
                {
                    flip();
                    if (DoesTilesFitTogetherIfRotated(t1, t2))
                    {
                        return true;
                    }
                    
                }

                return false;
            }

            public static bool DoesTilesFitTogetherIfRotated(Tile t1, Tile t2)
            {
                for (int i = 0; i < 4; i++)
                {
                    RotateRight(t1);
                    for (int y = 0; y < 4; y++)
                    {
                        RotateRight(t2);
                        if (t1.Rows[0].Equals(t2.Rows[0]))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }
    }
}
