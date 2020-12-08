using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day07 : BaseDay
    {
        private readonly Dictionary<string, List<(string name, int qty)>> bags = new Dictionary<string, List<(string, int)>>();
        
        public Day07()
        {
            foreach (var line in File.ReadAllLines(InputFilePath))
            {
                var matches = Regex.Matches(line, @"^([a-z]*\s[a-z]*)|(([1-9])\s([a-z]*\s[a-z]*))");

                string key = null;
                var list = new List<(string, int)>();
                foreach (Match match in matches)
                {
                    if (!string.IsNullOrEmpty(match.Groups[1].Value))
                    {
                        key = match.Groups[1].Value;
                    }
                    else
                    {
                        list.Add((match.Groups[4].Value, int.Parse(match.Groups[3].Value)));
                    }
                }

                bags.TryAdd(key, list);
            }
        }
        
        public override string Solve_1()
        {
            return bags.Count(x => ContainsGold(x.Key)).ToString();
        }

        private bool ContainsGold(string name)
        {
            if (bags[name].Any(x => x.name == "shiny gold"))
            {
                return true;
            }

            var hasGold = false;
            
            foreach (var bag in bags[name])
            {
                hasGold |= ContainsGold(bag.name);
            }

            return hasGold;
        }
        
        public override string Solve_2()
        {
            return CountBags("shiny gold").ToString();
        }

        private int CountBags(string rootName)
        {
            if (bags[rootName].Count == 0)
            {
                return 0;
            }
            
            var sum = 0;
            foreach (var bag in bags[rootName])
            {
                sum += bag.qty;
                sum += bag.qty * CountBags(bag.name);
            }

            return sum;
        }
    }
}