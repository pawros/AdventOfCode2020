using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day10 : BaseDay
    {
        private readonly List<int> input;

        public Day10()
        {
            input = File.ReadLines(InputFilePath).Select(int.Parse).ToList();

        }
        
        public override string Solve_1()
        {
            var sortedInput = new List<int> {0};
            sortedInput.AddRange(input.OrderBy(x => x));

            var diff1 = 0;
            var diff3 = 1;
            
            for (var i = 0; i < sortedInput.ToArray().Length - 1; i++)
            {
                var diff = sortedInput[i + 1] - sortedInput[i];
                switch (diff)
                {
                    case 1:
                        diff1++;
                        break;
                    case 3:
                        diff3++;
                        break;
                }
            }
            
            return (diff1 * diff3).ToString();
        }

        
        public override string Solve_2()
        {
            var sortedInput = new List<int>{0};
            sortedInput.AddRange(input.OrderBy(x => x));
            sortedInput.Add(sortedInput.Last() + 3);
            var paths = sortedInput.ToDictionary(x => x, x => 0L);
            paths[0] = 1;

            foreach (var adapter in sortedInput)
            {
                for (var d = 1; d <= 3; d++)
                {
                    var diff = adapter + d;
                    if (sortedInput.Contains(diff))
                    {
                        paths[diff] += paths[adapter];
                    }
                }
            }

            return paths.Last().Value.ToString();
        }
    }
}