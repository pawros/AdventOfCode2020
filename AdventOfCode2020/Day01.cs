using System.IO;
using System.Linq;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day01 : BaseDay
    {
        private readonly int[] input;

        public Day01()
        {
            input = File.ReadAllLines(InputFilePath).Select(int.Parse).ToArray();
        }
        
        public override string Solve_1()
        {
            for (var i = 0; i < input.Length - 1; i++)
            {
                for (var j = 0; j < input.Length; j++)
                {
                    if (input[i] + input[j] == 2020)
                    {
                        return (input[i] * input[j]).ToString();
                    }
                }
            }
            
            return string.Empty;
        }

        public override string Solve_2()
        {
            for (var i = 0; i < input.Length - 2; i++)
            {
                for (var j = i + 1; j < input.Length - 1; j++)
                {
                    for (var k = j + 1; k < input.Length; k++)
                    {
                        if (input[i] + input[j] + input[k] == 2020)
                        {
                            return (input[i] * input[j] * input[k]).ToString();
                        }
                    }
                }
            }
            
            return string.Empty;
        }
    }
}