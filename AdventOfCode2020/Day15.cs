using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day15 : BaseDay
    {
        private readonly string input;

        public Day15()
        {
            input = File.ReadAllText(InputFilePath);
        }
        
        public override string Solve_1()
        {
            var numbers = input.Split(',').Select(x => int.Parse(x)).ToList();
            var turn = numbers.Count;
            while (turn < 2020)
            {
                var lastSpoken = numbers.Last();
                if (numbers.Count(x => x == lastSpoken) < 2)
                {
                    numbers.Add(0);
                }
                else
                {
                    var indices = GetTwoLastIndices(lastSpoken).ToArray();
                    numbers.Add(indices[0] - indices[1]);
                }

                turn++;
            }
            
            return numbers.Last().ToString();
            
            IEnumerable<int> GetTwoLastIndices(int lastSpoken)
            {
                var count = 0;
                for (var i = numbers.Count - 1; i >= 0; i--)
                {
                    if (numbers[i] == lastSpoken)
                    {
                        count++;
                        yield return i + 1;
                    }

                    if (count == 2)
                    {
                        yield break;
                    }
                }
            }
        }

        public override string Solve_2()
        {
            var numbers = input.Split(',')
                .Select(x => int.Parse(x))
                .Select((x, i) => new { X = x, I = i})
                .ToDictionary(x => x.X, x => (lastTurn: x.I, prevLastTurn: -1));
            
            var lastSpoken = numbers.Last().Key;
            
            var turn = numbers.Count;
            while (turn < 30000000)
            {
                if (numbers[lastSpoken].prevLastTurn == -1)
                {
                    lastSpoken = 0;
                    numbers[lastSpoken] = (turn, numbers[lastSpoken].lastTurn);
                }
                else
                {
                    lastSpoken = numbers[lastSpoken].lastTurn - numbers[lastSpoken].prevLastTurn;
                    var prev = numbers.ContainsKey(lastSpoken) ? numbers[lastSpoken].lastTurn : -1;
                    numbers[lastSpoken] = (turn, prev);
                }
                
                turn++;
            }

            return lastSpoken.ToString();
        }
    }
}