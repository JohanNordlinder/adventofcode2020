using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D22P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllText("d_22_t_1.txt");
            Assert.AreEqual(306, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllText("d_22.txt");
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            public int RunChallenge(string input)
            {
                var playersDecksRaw = input.Split(new string[] { "Player 1:", "Player 2:" }, StringSplitOptions.RemoveEmptyEntries);
                var player1Deck = new Queue<int>();
                var player2Deck = new Queue<int>();

                foreach (var card in playersDecksRaw[0].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
                {
                    player1Deck.Enqueue(int.Parse(card));
                }
                foreach (var card in playersDecksRaw[1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
                {
                    player2Deck.Enqueue(int.Parse(card));
                }

                do
                {
                    var player1Card = player1Deck.Dequeue();
                    var player2Card = player2Deck.Dequeue();

                    if (player1Card > player2Card)
                    {
                        player1Deck.Enqueue(player1Card);
                        player1Deck.Enqueue(player2Card);
                    }
                    else
                    {
                        player2Deck.Enqueue(player2Card);
                        player2Deck.Enqueue(player1Card);
                    }
                } while (player1Deck.Count > 0 && player2Deck.Count > 0);

                var winningDeck = player1Deck.Count > 0 ? player1Deck : player2Deck;
                var score = 0;

                for (int i = winningDeck.Count; i > 0; i--)
                {
                    score += winningDeck.Dequeue() * i;
                }
                return score;
            }
        }
    }
}
