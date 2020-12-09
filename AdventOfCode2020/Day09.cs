using System;
using System.IO;
using System.Linq;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day09 : BaseDay
    {
        private readonly long[] input;
        private const int N = 25;
        private long globalNumber = 0;
        
        public Day09()
        {
            input = File.ReadAllLines(InputFilePath).Select(long.Parse).ToArray();
        }
        
        public override string Solve_1()
        {
            for (var i = N; i < input.Length; i++)
            {
                if (!FindSum(input[i], i - N))
                {
                    globalNumber = input[i];
                    break;
                }
            }

            return globalNumber.ToString();
        }

        private bool FindSum(long sum, int start)
        {
            for (var i = start; i < start + N - 1; i++)
            {
                for (var j = i + 1; j < start + N; j++)
                {
                    if (input[i] + input[j] == sum)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        public override string Solve_2()
        {
            var result = FindSetMinMax();
            return (result.Item1 + result.Item2).ToString();
        }

        private (long, long) FindSetMinMax()
        {
            (long smallest, long largest) pair = (0, 0);
            var globalNumberIndex = Array.IndexOf(input, globalNumber);
            for (var i = 0; i < globalNumberIndex; i++)
            { 
                long sum = 0;
                var index = i;
                while (sum < globalNumber)
                {
                    sum += input[index];
                    index++;
                }

                if (sum == globalNumber)
                {
                    var subset = input.Skip(i).Take(index - i).ToArray();
                    pair.smallest = subset.Min();
                    pair.largest = subset.Max();
                }
            }

            return pair;
        }
    }
}