using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AoCHelper;
using Microsoft.VisualBasic;

namespace AdventOfCode2020
{
    public sealed class Day23 : BaseDay
    {
        private readonly List<int> inputCups;
        
        public Day23()
        {
            inputCups = File.ReadAllText(InputFilePath).Select(x => int.Parse(x.ToString())).ToList();
        }
        
        public override string Solve_1()
        {
            var n = inputCups.Count;
            var cups = inputCups.ToList();
            var currentIndex = 0;
            for (var turn = 1; turn <= 100; turn++)
            {
                var currentLabel = cups[currentIndex % n];
                var picked = GetNextIndices(currentIndex, 3, n).Select(p => cups[p]).ToList();
                
                foreach (var destination in GetPreviousLabels(currentLabel, n))
                {
                    if (picked.Contains(destination))
                    {
                        continue;
                    }
                    
                    picked.ForEach(x => cups.Remove(x));

                    var destinationIndex = cups.IndexOf(destination);
                    cups.InsertRange(destinationIndex + 1, picked);

                    currentIndex = GetNextIndices(cups.IndexOf(currentLabel), 1, n).Single();
                    
                    break;
                }
            }

            var resultLabels = GetNextIndices(cups.IndexOf(1), 8, n).Select(i => cups[i].ToString()).ToArray();
            return Strings.Join(resultLabels, "");
        }

        private IEnumerable<int> GetNextIndices(int current, int c, int n)
        {
            for (var i = 0; i < c; i++)
            {
                yield return (i + current + 1) % n;
            }
        }
        
        private IEnumerable<int> GetPreviousLabels(int current, int n)
        {
            for (var i = 0; i < n; i++)
            {
                yield return (n - i - 2 + current) % n + 1;
            }
        }
        
        public override string Solve_2()
        {
            const int n = 1_000_000;
            var cups = new int[n + 1];
            for (var i = 0; i < inputCups.Count - 1; i++)
            {
                cups[inputCups[i]] = inputCups[i + 1];
            }

            cups[inputCups.Last()] = inputCups.Count + 1;

            foreach (var i in Enumerable.Range(inputCups.Count + 1, n - inputCups.Count - 1))
            {
                cups[i] = i + 1;
            }

            cups[n] = inputCups.First();

            var currentLabel = inputCups.First();
            for (var turn = 1; turn <= 10_000_000; turn++)
            {
                var pick1 = cups[currentLabel];
                var pick2 = cups[pick1];
                var pick3 = cups[pick2];
                
                cups[currentLabel] = cups[pick3];
                
                for (var i = 0; i < n; i++)
                {
                    var destination = (n - i - 2 + currentLabel) % n + 1;
                    if (destination == pick1 || destination == pick2 || destination == pick3)
                    {
                        continue;
                    }

                    var temp = cups[destination];
                    cups[destination] = pick1;
                    cups[pick3] = temp;

                    currentLabel = cups[currentLabel];
                    break;
                }
            }

            var label1 = cups[1];
            var label2 = cups[label1];

            var result =  (ulong)label1 * (ulong)label2;

            return result.ToString();
        }
    }
}