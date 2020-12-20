using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day19 : BaseDay
    {
        private class Rule
        {
            public List<int[]> Subrules { get; set; } = new List<int[]>();
        }
        
        private readonly Dictionary<int, Rule> rules;
        private readonly Dictionary<int, char> terminalSymbols;
        private readonly string[] words;
        
        public Day19()
        {
            var input = File.ReadAllText(InputFilePath).Split("\r\n\r\n");
            rules = new Dictionary<int, Rule>();
            terminalSymbols = new Dictionary<int, char>();
            
            foreach (var line in input[0].Split("\r\n"))
            {
                var match = Regex.Match(line, "([0-9]*):\\s(\"([ab])\"|(.*))");
                var ruleId = int.Parse(match.Groups[1].Value);

                if (!string.IsNullOrEmpty(match.Groups[3].Value))
                {
                    terminalSymbols.Add(ruleId, match.Groups[3].Value[0]);
                }
                else
                {
                    var rule = new Rule
                    {
                        Subrules = match.Groups[4].Value
                            .Split("|")
                            .Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse).ToArray())
                            .ToList()
                    };

                    rules.Add(ruleId, rule);
                }
            }

            words = input[1].Split("\r\n");
        }
        
        public override string Solve_1()
        {
            var regexBuilder = new StringBuilder();
            
            regexBuilder.Append("^");
            BuildRegex(0, regexBuilder);
            regexBuilder.Append("$");

            var regex = regexBuilder.ToString();
            
            var count = words.Select(word => Regex.Match(word, regex)).Count(match => match.Success);
            return count.ToString();
        }

        public override string Solve_2()
        {
            rules[8] = new Rule
            {
                Subrules = new List<int[]>
                {
                    new [] {42},
                    new [] {42, 42},
                    new [] {42, 42, 42},
                    new [] {42, 42, 42, 42},
                    new [] {42, 42, 42, 42, 42},
                }
            };
            
            rules[11] = new Rule
            {
                Subrules = new List<int[]>
                {
                    new[] {42, 31},
                    new[] {42, 42, 31, 31},
                    new[] {42, 42, 42, 31, 31, 31},
                    new[] {42, 42, 42, 42, 31, 31, 31, 31},
                }
            };
            
            var regexBuilder = new StringBuilder();
            
            regexBuilder.Append("^");
            BuildRegex(0, regexBuilder);
            regexBuilder.Append("$");

            var regex = regexBuilder.ToString();
            
            var count = words.Select(word => Regex.Match(word, regex)).Count(match => match.Success);
            return count.ToString();
        }
        
        private void BuildRegex(int n, StringBuilder regexBuilder)
        {
            if (terminalSymbols.ContainsKey(n))
            {
                regexBuilder.Append(terminalSymbols[n]);
                return;
            }

            var rule = rules[n];
            
            regexBuilder.Append("(");
            for (var i = 0; i < rule.Subrules.Count; i++)
            {
                foreach (var c in rule.Subrules[i])
                {
                    BuildRegex(c, regexBuilder);
                }

                if (i != rule.Subrules.Count - 1)
                {
                    regexBuilder.Append("|");
                }
            }
            regexBuilder.Append(")");
        }
    }
}