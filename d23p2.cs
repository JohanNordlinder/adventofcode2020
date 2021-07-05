using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D23P2
    {

        [TestMethod]
        public void TestRun()
        {
            Assert.AreEqual(149245887792, new Program().RunChallenge(new int[] { 3, 8, 9, 1, 2, 5, 4, 6, 7 }));
        }

        [TestMethod]
        public void RealRun()
        {
            Console.WriteLine("Result: " + new Program().RunChallenge(new int[] { 2, 1, 9, 7, 4, 8, 3, 6, 5 }));
        }

        public class Program
        {
            private LinkedList<int> Cups = new LinkedList<int>();

            private int MaximumCupLabel;
            private Dictionary<int, LinkedListNode<int>> LookupTable = new Dictionary<int, LinkedListNode<int>>();

            public long RunChallenge(int[] cupsRaw)
            {
                cupsRaw.ToList().ForEach(c =>
                {
                    Cups.AddLast(c);
                    LookupTable.Add(c, Cups.Last);
                });

                for (int i = cupsRaw.Max() + 1; i <= 1000000; i++)
                {
                    Cups.AddLast(i);
                    LookupTable.Add(i, Cups.Last);
                }

                var pickedUp = new Stack<LinkedListNode<int>>();

                var currentCup = Cups.First;

                for (int i = 0; i < 10000000; i++)
                {

                    for (int j = 0; j < 3; j++)
                    {
                        var nextCup = GetNextCup(currentCup);
                        pickedUp.Push(nextCup);
                        Cups.Remove(nextCup);
                        LookupTable.Remove(nextCup.Value);
                    }

                    var destinationCup = findDestinationCup(currentCup.Value);

                    for (int j = 0; j < 3; j++)
                    {
                        var cupToPlace = pickedUp.Pop();
                        Cups.AddAfter(destinationCup, cupToPlace);
                        LookupTable.Add(cupToPlace.Value, cupToPlace);
                    }

                    pickedUp.Clear();
                    currentCup = GetNextCup(currentCup);
                }

                var one = Cups.Find(1);
                long first = one.Next.Value;
                long second = one.Next.Next.Value;

                return first * second;
            }

            private LinkedListNode<int> findDestinationCup(int currentCupValue)
            {
                if (currentCupValue == 1)
                {
                    return Cups.Find(1000000);
                }
                var destinationCupNumber = currentCupValue - 1;
                LinkedListNode<int> foundCup;
                var destinationCup = LookupTable.TryGetValue(destinationCupNumber, out foundCup) ? foundCup : findDestinationCup(destinationCupNumber);
                return destinationCup;
            }

            private LinkedListNode<int> GetNextCup(LinkedListNode<int> current)
            {
                return current.Next ?? Cups.First;
            }
        }
    }
}
