using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D15P1
    {

        [TestMethod]
        public void TestRun()
        {
            Assert.AreEqual(436L, new Program().RunChallenge(new int[] { 0, 3, 6 }));
            Assert.AreEqual(1L, new Program().RunChallenge(new int[] { 1, 3, 2 }));
            Assert.AreEqual(10L, new Program().RunChallenge(new int[] { 2, 1, 3 }));
            Assert.AreEqual(27L, new Program().RunChallenge(new int[] { 1, 2, 3 }));
            Assert.AreEqual(78L, new Program().RunChallenge(new int[] { 2, 3, 1 }));
            Assert.AreEqual(438L, new Program().RunChallenge(new int[] { 3, 2, 1 }));
            Assert.AreEqual(1836L, new Program().RunChallenge(new int[] { 3, 1, 2 }));

        }

        [TestMethod]
        public void RealRun()
        {
            Console.WriteLine("Result: " + new Program().RunChallenge(new int[] { 0, 13, 1, 16, 6, 17 }));
        }

        public class Program
        {
            class NumberSpokenInfo
            {
                public long TimesSpoken { get; set; }
                public long LastSpokenAtTurn { get; set; }
                public long LastSpokenAtTurnDiff { get; set; }

            }
            public long RunChallenge(int[] startingNumbers)
            {
                long lastSpoken = startingNumbers.Last();
                var lastSpokenIndex = new Dictionary<long, NumberSpokenInfo>();

                for (int i = 1; i <= startingNumbers.Length; i++)
                {
                    lastSpokenIndex.Add(startingNumbers[i - 1], new NumberSpokenInfo { TimesSpoken = 1, LastSpokenAtTurn = i });
                }

                for (long i = startingNumbers.Length + 1; i <= 2020; i++)
                {
                    long numberToSpeak;
                    NumberSpokenInfo lastSpokenInfo;
                    if (lastSpokenIndex.TryGetValue(lastSpoken, out lastSpokenInfo) && lastSpokenInfo.TimesSpoken > 1)
                    {
                        numberToSpeak = lastSpokenInfo.LastSpokenAtTurnDiff;
                        lastSpokenInfo.TimesSpoken++;
                    } else
                    {
                        numberToSpeak = 0;
                    }

                    NumberSpokenInfo numberToSpeakInfo;
                    if (lastSpokenIndex.TryGetValue(numberToSpeak, out numberToSpeakInfo))
                    {
                        numberToSpeakInfo.LastSpokenAtTurnDiff = i - numberToSpeakInfo.LastSpokenAtTurn;
                        numberToSpeakInfo.TimesSpoken++;
                        numberToSpeakInfo.LastSpokenAtTurn = i;
                    }
                    else
                    {
                        lastSpokenIndex.Add(numberToSpeak, new NumberSpokenInfo { TimesSpoken = 1, LastSpokenAtTurn = i });
                    }

                    lastSpoken = numberToSpeak;

                }
                return lastSpoken;
            }
        }
    }
}
