using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D22P2
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllText("d_22_t_1.txt");
            Assert.AreEqual(291, new Program().RunChallenge(input));
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

                var winner = PlayGame(1, player1Deck, player2Deck);

                Debug.WriteLine("== Post-game results ==");
                PrintDeck(1, player1Deck);
                PrintDeck(2, player2Deck);

                var winningDeck = winner == 1 ? player1Deck : player2Deck;
                var score = 0;

                for (int i = winningDeck.Count; i > 0; i--)
                {
                    score += winningDeck.Dequeue() * i;
                }
                return score;
            }


            private int PlayGame(int gameNumber, Queue<int> player1Deck, Queue<int> player2Deck)
            {
                // Debug.WriteLine("=== Game {0} ===" + Environment.NewLine, gameNumber);
                var previousRounds = new Dictionary<string, bool>();

                int round = 0;

                while (player1Deck.Count > 0 && player2Deck.Count > 0)
                {
                    round++;
                    // Debug.WriteLine("-- Round {0} (Game {1})--", round, gameNumber);

                    // PrintDeck(1, player1Deck);
                    // PrintDeck(2, player2Deck);

                    var gameState = string.Join(",", player1Deck.ToArray()) + ":" + string.Join(",", player2Deck.ToArray());
                    if (!previousRounds.TryAdd(gameState, true))
                    {
                        //Debug.WriteLine("Tried to play the same decks again!, player 1 wins!");
                        return 1;
                    }

                    var player1Card = player1Deck.Dequeue();
                    var player2Card = player2Deck.Dequeue();

                    // Debug.WriteLine("Player 1 plays: " + player1Card);
                    // Debug.WriteLine("Player 2 plays: " + player2Card);

                    int winnerOfRound;

                    if (player1Deck.Count >= player1Card && player2Deck.Count >= player2Card)
                    {
                        // Debug.WriteLine("Playing a sub - game to determine the winner..." + Environment.NewLine);
                        winnerOfRound = PlayGame(gameNumber + 1, new Queue<int>(player1Deck.Take(player1Card)), new Queue<int>(player2Deck.Take(player2Card)));
                        // Debug.WriteLine("...anyway, back to game {0}.", gameNumber);
                    }
                    else
                    {
                        winnerOfRound = player1Card > player2Card ? 1 : 2;
                    }

                    // Debug.WriteLine("Player {0} wins round {1} of game {2}!" + Environment.NewLine, winnerOfRound, round, gameNumber);

                    if (winnerOfRound == 1)
                    {
                        player1Deck.Enqueue(player1Card);
                        player1Deck.Enqueue(player2Card);
                    }
                    else
                    {
                        player2Deck.Enqueue(player2Card);
                        player2Deck.Enqueue(player1Card);
                    }
                }

                var winnerOfGame = player1Deck.Count > 0 ? 1 : 2;

                // Debug.WriteLine("The winner of game {0} is player {1}!" + Environment.NewLine, gameNumber, winnerOfGame);
                return winnerOfGame;
            }

            private void PrintDeck(int playerNumber, Queue<int> deck)
            {
                Debug.WriteLine("Player {0}'s deck: " + string.Join(", ", deck.ToArray()), playerNumber);
            }
        }
    }
}
