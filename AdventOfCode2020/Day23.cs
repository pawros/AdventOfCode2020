using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCHelper;
using Microsoft.VisualBasic;

namespace AdventOfCode2020
{
    public sealed class Day23 : BaseDay
    {
        private readonly List<int> cups;
        private readonly int n;
        
        public Day23()
        {
            cups = File.ReadAllText(InputFilePath).Select(x => int.Parse(x.ToString())).ToList();
            n = cups.Count;
        }
        
        public override string Solve_1()
        {
            var currentIndex = 0;
            for (var turn = 1; turn <= 100; turn++)
            {
                var currentLabel = cups[currentIndex % n];
                var picked = GetNextIndices(currentIndex, 3).Select(p => cups[p]).ToList();
                
                foreach (var destination in GetPreviousLabels(currentLabel))
                {
                    if (picked.Contains(destination))
                    {
                        continue;
                    }
                    
                    picked.ForEach(x => cups.Remove(x));

                    var destinationIndex = cups.IndexOf(destination);
                    cups.InsertRange(destinationIndex + 1, picked);

                    currentIndex = GetNextIndices(cups.IndexOf(currentLabel), 1).Single();
                    
                    break;
                }
            }

            var resultLabels = GetNextIndices(cups.IndexOf(1), 8).Select(i => cups[i].ToString()).ToArray();
            return Strings.Join(resultLabels, "");
        }

        public override string Solve_2()
        {
            throw new System.NotImplementedException();
        }

        private IEnumerable<int> GetNextIndices(int current, int c)
        {
            for (var i = 0; i < c; i++)
            {
                yield return (i + current + 1) % n;
            }
        }
        
        private IEnumerable<int> GetPreviousLabels(int current)
        {
            for (var i = 0; i < n; i++)
            {
                yield return (n - i - 2 + current) % n + 1;
            }
        }
    }
}