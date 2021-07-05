using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D23P1
    {

        [TestMethod]
        public void TestRun()
        {
            Assert.AreEqual("67384529", new Program().RunChallenge(new int[] { 3, 8, 9, 1, 2, 5, 4, 6, 7 }));
        }

        [TestMethod]
        public void RealRun()
        {
            Console.WriteLine("Result: " + new Program().RunChallenge(new int[] { 2, 1, 9, 7, 4, 8, 3, 6, 5 }));
        }

        public class Program
        {
            private LinkedList<int> Cups = new LinkedList<int>();

            private int MinimumCupLabel;
            private int MaximumCupLabel;

            public string RunChallenge(int[] cupRaw)
            {
                MinimumCupLabel = cupRaw.Min();
                MaximumCupLabel = cupRaw.Max();
                cupRaw.ToList().ForEach(c => Cups.AddLast(c));

                var currentCup = Cups.First;
                var pickedUp = new Stack<LinkedListNode<int>>();
                for (int i = 0; i < 100; i++)
                {
                    Console.WriteLine("--move {0}--", i + 1);
                    Console.WriteLine("cups: {0}", string.Join(" ", Cups.Select(c => c == currentCup.Value ? "(" + c + ")" : c.ToString())));

                    for (int j = 0; j < 3; j++)
                    {
                        var nextCup = GetNextCup(currentCup);
                        pickedUp.Push(nextCup);
                        Cups.Remove(nextCup);
                    }

                    Console.WriteLine("pick up: {0}", string.Join(" ", pickedUp.Select(c => c.Value)));

                    var destinationCup = findDestinationCup(currentCup.Value);

                    Console.WriteLine("destination: {0}" + Environment.NewLine, destinationCup.Value);

                    for (int j = 0; j < 3; j++)
                    {
                        Cups.AddAfter(destinationCup, pickedUp.Pop());

                    }
                    pickedUp.Clear();
                    currentCup = GetNextCup(currentCup);
                }

                var result = string.Empty;

                currentCup = Cups.Find(1).Next;
                for (int i = 0; i < cupRaw.Length - 1; i++)
                {
                    result += currentCup.Value;
                    currentCup = GetNextCup(currentCup);
                }

                return result;
            }

            private LinkedListNode<int> findDestinationCup(int currentCupValue)
            {
                if (currentCupValue == MinimumCupLabel)
                {
                    return findDestinationCup(MaximumCupLabel + 1);
                }
                var destinationCupNumber = currentCupValue - 1;
                var destinationCup = Cups.Find(destinationCupNumber) ?? findDestinationCup(destinationCupNumber);
                return destinationCup;
            }

            private LinkedListNode<int> GetNextCup(LinkedListNode<int> current)
            {
                return current.Next ?? Cups.First;
            }
        }
    }
}
