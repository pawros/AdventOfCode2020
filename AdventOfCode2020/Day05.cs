using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day05 : BaseDay
    {
        private string[] seats;
        private List<int> ids;
        
        public Day05()
        {
            seats = File.ReadAllLines(InputFilePath);
            ids = new List<int>();
        }
        
        public override string Solve_1()
        {
            foreach (var id in seats)
            {
                var rowPart = id.Substring(0, 7);
                var colPart = id.Substring(7);
                
                (int a, int b) rowRange = (0, 127);
                foreach (var c in rowPart)
                {
                    if (c == 'F')
                    {
                        rowRange = (rowRange.a, rowRange.a / 2 + rowRange.b / 2);
                        continue;
                    }

                    if (c == 'B')
                    {
                        rowRange = (rowRange.a / 2 + rowRange.b / 2 + 1, rowRange.b);
                    }
                }

                (int a, int b) colRange = (0, 7);
                foreach (var c in colPart)
                {
                    if (c == 'L')
                    {
                        colRange = (colRange.a, colRange.a / 2 + colRange.b / 2);
                        continue;
                    }

                    if (c == 'R')
                    {
                        colRange = (colRange.a / 2 + colRange.b / 2 + 1, colRange.b);
                    }
                }

                ids.Add(rowRange.a * 8 + colRange.a);
            }

            return ids.Max().ToString();
        }
        
        public override string Solve_2()
        {
            var missingId = 0;
            var orderedIds = ids.OrderBy(x => x).ToArray();
            for (var i = 0; i < orderedIds.Length - 1; i++)
            {
                if (orderedIds[i] + 1 != orderedIds[i + 1])
                {
                    missingId = orderedIds[i] + 1;
                    break;
                }
            }

            return missingId.ToString();
        }
    }
}