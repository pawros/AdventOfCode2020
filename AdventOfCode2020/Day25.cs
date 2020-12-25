using System;
using System.IO;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day25 : BaseDay
    {
        private int cardPublicKey;
        private int doorPublicKey;

        public Day25()
        {
            var input = File.ReadAllLines(InputFilePath);
            cardPublicKey = int.Parse(input[0]);
            doorPublicKey = int.Parse(input[1]);
        }
        
        public override string Solve_1()
        {
            var cardLoop = GetLoopNumber(cardPublicKey, 7);
            var encryptionKey = GetEncryptionKey(doorPublicKey, cardLoop);

            return encryptionKey.ToString();
        }

        private int GetLoopNumber(int number, int subject)
        {
            var value = 1;
            var loop = 1;
            while (true)
            {
                value *= subject;
                value %= 20201227;

                if (value == number)
                {
                    return loop;
                }

                loop++;
            }
        }

        private long GetEncryptionKey(int subject, int loop)
        {
            long value = 1;
            for (var i = 1; i <= loop; i++)
            {
                value *= subject;
                value %= 20201227;
            }

            return value;
        }
        
        public override string Solve_2()
        {
            return "The End.";
        }
    }
}