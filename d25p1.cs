using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D25P1
    {

        [TestMethod]
        public void TestRun()
        {
            Assert.AreEqual(14897079, new Program().RunChallenge(5764801, 17807724));
        }

        [TestMethod]
        public void RealRun()
        {
            Console.WriteLine("Result: " + new Program().RunChallenge(17607508, 15065270));
        }

        public class Program
        {
            public int SubjectNumber { get; set; } = 7;

            public long RunChallenge(int cardPublicKey, int doorPublicKey)
            {
                var cardLoopSize = BruteForce(cardPublicKey);
                var privateKey = Encrypt(doorPublicKey, cardLoopSize);
                return privateKey;
            }

            private long BruteForce(int key)
            {
                var keyToTest = 1L;
                var cardLoopSize = 0;
                while (true)
                {
                    cardLoopSize++;
                    keyToTest = RunEncryptionAlgoritm(keyToTest, 7);
                    if (keyToTest == key)
                    {
                        break;
                    }
                }
                return cardLoopSize;
            }

            private long Encrypt(int subjectNumber, long loopSize)
            {
                long privateKey = 1;
                for (int i = 0; i < loopSize; i++)
                {
                    privateKey = RunEncryptionAlgoritm(privateKey, subjectNumber);
                }

                return privateKey;
            }

            private long RunEncryptionAlgoritm(long key, long subjectNumber)
            {
                var result = key;
                result *= subjectNumber;
                result %= 20201227;
                return result;
            }
        }
    }
}
