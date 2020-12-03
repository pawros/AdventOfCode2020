using System;
using System.IO;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day03 : BaseDay
    {
        private string[] input;

        public Day03()
        {
            input = File.ReadAllLines(InputFilePath);
        }
        
        public override string Solve_1()
        {
            var H = input.Length;
            var W = input[0].Length;

            var trees = 0;
            
            var x = 0;
            for (var y = 0; y < H; y++)
            {
                if (input[y][x % W] == '#')
                {
                    trees++;
                }

                x += 3;
            }
            
            return trees.ToString();
        }

        public override string Solve_2()
        {
            var H = input.Length;
            var W = input[0].Length;
            
            (int x, int y)[] deltas = new[] {(1, 1), (3, 1), (5, 1), (7, 1), (1, 2)};
            
            long multiTrees = 1;
            
            foreach (var d in deltas)
            {
                var trees = 0;
                
                var x = 0;
                for (var y = 0; y < H; y += d.y)
                {
                    if (input[y][x % W] == '#')
                    {
                        trees++;
                    }
            
                    x += d.x;
                }
                
                multiTrees *= trees;
                Console.WriteLine(trees);
            }
            
            return multiTrees.ToString();
        }
    }
}