using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day04 : BaseDay
    {
        private class Passport
        {
            public string Byr { get; set; }
            public string Iyr { get; set; }
            public string Eyr { get; set; }
            public string Hgt { get; set; }
            public string Hcl { get; set; }
            public string Ecl { get; set; }
            public string Pid { get; set; }
            public string Cid { get; set; }

            public bool IsValid => Validate();
            public bool IsFullyValid => FullyValidated();
            
            private bool Validate()
            {
                var valid = true;
                valid &= !string.IsNullOrWhiteSpace(Byr);
                valid &= !string.IsNullOrWhiteSpace(Iyr);;
                valid &= !string.IsNullOrWhiteSpace(Eyr);;
                valid &= !string.IsNullOrWhiteSpace(Hgt);;
                valid &= !string.IsNullOrWhiteSpace(Hcl);;
                valid &= !string.IsNullOrWhiteSpace(Ecl);
                valid &= !string.IsNullOrWhiteSpace(Pid);
                return valid;
            }
            
            
            private bool FullyValidated()
            {
                var valid = true;
                valid &= IsFieldValid(Byr, @"\b(19[2-8][0-9]|199[0-9]|200[0-2])\b");
                valid &= IsFieldValid(Iyr, @"\b(201[0-9]|2020)\b");
                valid &= IsFieldValid(Eyr, @"\b(202[0-9]|2030)\b");
                valid &= IsFieldValid(Hgt, @"\b((1[5-8][0-9]|19[0-3])cm|(59|6[0-9]|7[0-6])in)\b");
                valid &= IsFieldValid(Hcl, @"#\b[0-9a-z]{6}\b");
                valid &= IsFieldValid(Ecl, @"\b(amb|blu|brn|gry|grn|hzl|oth)\b");
                valid &= IsFieldValid(Pid, @"\b[0-9]{9}\b");
                return valid;
            }

            private bool IsFieldValid(string field, string regex)
            {
                return !string.IsNullOrWhiteSpace(field) && Regex.Match(field, regex).Success;

            }
        }

        private IList<Passport> passports;

        private void ParseInput()
        {
            passports = new List<Passport>();
            
            var input = File.ReadAllText(InputFilePath);
            var chunks = input.Split("\r\n\r\n");

            foreach (var chunk in chunks)
            {
                var passport = new Passport();
                var newChunk = chunk.Replace("\r\n", " ");
                var fields = newChunk.Split(' ');
                foreach (var field in fields)
                {
                    var fieldSplit = field.Split(':');
                    switch (fieldSplit[0])
                    {
                        case "byr":
                            passport.Byr = fieldSplit[1];
                            break;
                        case "iyr":
                            passport.Iyr = fieldSplit[1];
                            break;
                        case "eyr":
                            passport.Eyr = fieldSplit[1];
                            break;
                        case "hgt":
                            passport.Hgt = fieldSplit[1];
                            break;
                        case "hcl":
                            passport.Hcl = fieldSplit[1];
                            break;
                        case "ecl":
                            passport.Ecl = fieldSplit[1];
                            break;
                        case "pid":
                            passport.Pid = fieldSplit[1];
                            break;
                        case "cid":
                            passport.Cid = fieldSplit[1];
                            break;
                    }
                }
                passports.Add(passport);
            }
        }
        
        public Day04()
        {
            ParseInput();
        }
        
        public override string Solve_1()
        {
            return passports.Count(p => p.IsValid).ToString();
        }

        public override string Solve_2()
        {
            return passports.Count(p => p.IsFullyValid).ToString();
        }
    }
}