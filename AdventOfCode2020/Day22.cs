using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day22 : BaseDay
    {
        private readonly Queue<int> player1Deck = new Queue<int>();
        private readonly Queue<int> player2Deck = new Queue<int>();

        public Day22()
        {
            var input = File.ReadAllText(InputFilePath).Split("\r\n\r\n");

            var player1Input = input[0].Split("\r\n");
            for (var i = 1; i < player1Input.Length; i++)
            {
                player1Deck.Enqueue(int.Parse(player1Input[i]));
            }
            
            var player2Input = input[1].Split("\r\n");
            for (var i = 1; i < player2Input.Length; i++)
            {
                player2Deck.Enqueue(int.Parse(player2Input[i]));
            }
        }
        
        public override string Solve_1()
        {
            var player1 = new Queue<int>(player1Deck);
            var player2 = new Queue<int>(player2Deck);
            
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
            
            return GetResult(player1, player2).ToString();
        }

        public override string Solve_2()
        {
            var player1 = new Queue<int>(player1Deck);
            var player2 = new Queue<int>(player2Deck);

            PlayGame(player1, player2);
            
            return GetResult(player1, player2).ToString();
        }

        private Player PlayGame(Queue<int> player1, Queue<int> player2)
        {
            var player1States = new List<int[]>();
            var player2States = new List<int[]>();
            while (player1.Any() && player2.Any())
            {
                if (player1States.Any(s => s.SequenceEqual(player1.ToArray())) || 
                    player2States.Any(s => s.SequenceEqual(player2.ToArray())))
                {
                    return Player.P1;
                }
                
                player1States.Add(player1.ToArray());
                player2States.Add(player2.ToArray());
                
                var p1 = player1.Dequeue();
                var p2 = player2.Dequeue();
                
                if (player1.Count >= p1 && player2.Count >= p2)
                {
                    var winner = PlayGame(new Queue<int>(player1.Take(p1)), new Queue<int>(player2.Take(p2)));
                    if (winner == Player.P1)
                    {
                        player1.Enqueue(p1);
                        player1.Enqueue(p2);
                    }
                    else if (winner == Player.P2)
                    {
                        player2.Enqueue(p2);
                        player2.Enqueue(p1);
                    }

                    continue;
                }
                
                if (p1 > p2)
                {
                    player1.Enqueue(p1);
                    player1.Enqueue(p2);
                }
                else if (p2 > p1)
                {
                    player2.Enqueue(p2);
                    player2.Enqueue(p1);
                }
            }

            if (player1.Any())
            {
                return Player.P1;
            }

            if (player2.Any())
            {
                return Player.P2;
            }

            throw new ArgumentOutOfRangeException();
        }
        
        private enum Player
        {
            P1,
            P2
        }
        
        private int GetResult(Queue<int> player1, Queue<int> player2)
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