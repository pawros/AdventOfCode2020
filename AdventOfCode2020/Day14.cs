using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day14 : BaseDay
    {
        private string[] input;
        private const int Bit = 36;
        
        public Day14()
        {
            input = File.ReadAllLines(InputFilePath);
        }
        
        public override string Solve_1()
        {
            var memory = new Dictionary<int, long>();
            string mask = string.Empty;
            foreach (var line in input)
            {
                if (line.StartsWith("mask"))
                {
                    mask = Regex.Match(line, @"mask = ([01X]*)").Groups[1].Value;
                }
                else
                {
                    var match = Regex.Match(line, @"mem\[([0-9]*)] = ([0-9]*)");
                    var address = int.Parse(match.Groups[1].Value);
                    var value = int.Parse(match.Groups[2].Value);
                    var binaryValue = Convert.ToString(value, 2).PadLeft(Bit, '0').ToCharArray();;
                    
                    ApplyMask(binaryValue, mask);
                    var newValue = Convert.ToInt64(new string(binaryValue), 2);

                    memory[address] = newValue;
                }
            }

            return memory.Values.Sum(x => x).ToString();
        }

        public override string Solve_2()
        {
            var memory = new Dictionary<long, long>();
            string mask = string.Empty;
            foreach (var line in input)
            {
                if (line.StartsWith("mask"))
                {
                    mask = Regex.Match(line, @"mask = ([01X]*)").Groups[1].Value;
                }
                else
                {
                    var match = Regex.Match(line, @"mem\[([0-9]*)] = ([0-9]*)");
                    var address = int.Parse(match.Groups[1].Value);
                    var value = int.Parse(match.Groups[2].Value);
                    var binaryAddress = Convert.ToString(address, 2).PadLeft(Bit, '0').ToCharArray();
                    ApplyAddressMask(binaryAddress, mask);

                    var n = binaryAddress.Count(x => x == 'X');
                    var pow = (int) Math.Pow(2, n);
                    for (var i = 0; i < pow; i++)
                    {
                        var index = Convert.ToString(i, 2).PadLeft(n, '0').ToCharArray();
                        var pi = 0;
                        var tempAddress = (char[])binaryAddress.Clone();
                        
                        for (var pos = 0; pos < tempAddress.Length; pos++)
                        {
                            if (tempAddress[pos] == 'X')
                            {
                                tempAddress[pos] = index[pi++];
                            }
                        }
                        var v = Convert.ToInt64(new string(tempAddress), 2);
                        memory[v] = value;
                    }
                }
            }

            return memory.Values.Sum(x => x).ToString();
        }

        private void ApplyMask(char[] binary, string mask)
        {
            for (var i = 0; i < Bit; i++)
            {
                switch (mask[i])
                {
                    case 'X':
                        continue;
                    case '1':
                    case '0':
                        binary[i] = mask[i];
                        break;
                }
            }
        }

        private void ApplyAddressMask(char[] binary, string mask)
        {
            for (var i = 0; i < Bit; i++)
            {
                switch (mask[i])
                {
                    case '0':
                        continue;
                    case '1':
                        binary[i] = '1';
                        break;
                    case 'X':
                        binary[i] = 'X';
                        break;
                }
            }
        }
    }
}