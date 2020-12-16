using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day15 : BaseDay
    {
        private readonly int[] sequence;

        public Day15()
        {
            var input = File.ReadAllText(InputFilePath);
            sequence = input.Split(',').Select(x => int.Parse(x)).ToArray();
        }
        
        public override string Solve_1()
        {
            return GetNthSpoken(sequence, 2020).ToString();
        }

        public override string Solve_2()
        {
            return GetNthSpoken(sequence, 30_000_000).ToString();
        }

        private static int GetNthSpoken(IReadOnlyList<int> sequence, int nth)
        {
            var numbers = new int[nth];
            for (var i = 0; i < sequence.Count; i++)
            {
                numbers[sequence[i]] = i + 1;
            }

            var lastSpoken = sequence[^1];
            for (var turn = sequence.Count; turn < nth; turn++)
            {
                if (numbers[lastSpoken] > 0)
                {
                    var newSpoken = turn - numbers[lastSpoken];
                    numbers[lastSpoken] = turn;
                    lastSpoken = newSpoken;

                }
                else
                {
                    numbers[lastSpoken] = turn;
                    lastSpoken = 0;
                }
            }

            return lastSpoken;
        }
    }
}