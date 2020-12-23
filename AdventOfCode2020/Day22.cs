using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day22 : BaseDay
    {
        private readonly Queue<int> player1 = new Queue<int>();
        private readonly Queue<int> player2 = new Queue<int>();

        public Day22()
        {
            var input = File.ReadAllText(InputFilePath).Split("\r\n\r\n");

            var player1Input = input[0].Split("\r\n");
            for (var i = 1; i < player1Input.Length; i++)
            {
                player1.Enqueue(int.Parse(player1Input[i]));
            }
            
            var player2Input = input[1].Split("\r\n");
            for (var i = 1; i < player2Input.Length; i++)
            {
                player2.Enqueue(int.Parse(player2Input[i]));
            }
        }
        
        public override string Solve_1()
        {
            while (player1.Any() && player2.Any())
            {
                var p1 = player1.Dequeue();
                var p2 = player2.Dequeue();

                if (p1 > p2)
                {
                    player1.Enqueue(p1);
                    player1.Enqueue(p2);
                }

                if (p2 > p1)
                {
                    player2.Enqueue(p2);
                    player2.Enqueue(p1);
                }
            }

            return GetResult().ToString();
        }

        public override string Solve_2()
        {
            throw new System.NotImplementedException();
        }

        private int GetResult()
        {
            if (player1.Any())
            {
                return player1.ToArray().Select((t, i) => (player1.Count - i) * t).Sum();
            }
            if (player2.Any())
            {
                return player2.ToArray().Select((t, i) => (player2.Count - i) * t).Sum();
            }

            return 0;
        }
    }
}