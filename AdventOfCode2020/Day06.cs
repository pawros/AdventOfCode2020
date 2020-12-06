using System;
using System.IO;
using System.Linq;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day06 : BaseDay
    {
        private string input;

        public Day06()
        {
            input = File.ReadAllText(InputFilePath);
        }
        
        public override string Solve_1()
        {
            var totalDistinct = 0;
            var groups = input.Split("\r\n\r\n").Select(x => x.Replace("\r\n", String.Empty));
            foreach (var group in groups)
            {
                totalDistinct += group.Distinct().Count(); 
            }

            return totalDistinct.ToString();
        }

        public override string Solve_2()
        {
            var totalCommon = 0;
            var groups = input.Split("\r\n\r\n");
            foreach (var group in groups)
            {
                var lines = group.Split("\r\n");
                for (var i = 0; i < lines.Length - 1; i++)
                {
                    lines[i + 1] = new string(lines[i].Intersect(lines[i + 1]).ToArray());
                }

                totalCommon += lines.Last().Length;
            }

            return totalCommon.ToString();
        }
    }
}