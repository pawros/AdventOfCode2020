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

            public bool IsAlmostValid => Validate1();
            public bool IsCompletelyValid => Validate2();
            
            private bool Validate1()
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
            
            
            private bool Validate2()
            {
                var valid = true;
                valid &= IsByrValid();
                valid &= IsIyrValid();
                valid &= IsEyrValid();
                valid &= IsHgtValid();
                valid &= IsHclValid();
                valid &= IsEclValid();
                valid &= IsPidValid();
                return valid;
            }

            private bool IsByrValid()
            {
                if (string.IsNullOrWhiteSpace(Byr))
                {
                    return false;
                }
                
                var valid = true;
                valid &= Byr.Length == 4;
                valid &= int.Parse(Byr) >= 1920;
                valid &= int.Parse(Byr) <= 2002;
                return valid;
            }

            private bool IsIyrValid()
            {
                if (string.IsNullOrWhiteSpace(Iyr))
                {
                    return false;
                }
                
                var valid = true;
                valid &= Iyr.Length == 4;
                valid &= int.Parse(Iyr) >= 2010;
                valid &= int.Parse(Iyr) <= 2020;
                return valid;
            }
            
            private bool IsEyrValid()
            {
                if (string.IsNullOrWhiteSpace(Eyr))
                {
                    return false;
                }
                
                var valid = true;
                valid &= Eyr.Length == 4;
                valid &= int.Parse(Eyr) >= 2020;
                valid &= int.Parse(Eyr) <= 2030;
                return valid;
            }
           
            private bool IsHgtValid()
            {
                if (string.IsNullOrWhiteSpace(Hgt))
                {
                    return false;
                }

                if (Hgt.Contains("cm"))
                {
                    var cm = int.Parse(Hgt.Remove(Hgt.IndexOf("cm", StringComparison.Ordinal)));
                    return cm >= 150 && cm <= 193;
                }

                if (Hgt.Contains("in"))
                {
                    var inc = int.Parse(Hgt.Remove(Hgt.IndexOf("in", StringComparison.Ordinal)));
                    return inc >= 59 && inc <= 76;
                }

                return false;
            }
            
            private bool IsHclValid()
            {
                if (string.IsNullOrWhiteSpace(Hcl))
                {
                    return false;
                }

                var regex = @"#\b[0-9a-z]{6}\b";
                return Regex.Match(Hcl, regex).Success;
            }

            private bool IsEclValid()
            {
                if (string.IsNullOrWhiteSpace(Ecl))
                {
                    return false;
                }

                var regex = @"\b(amb|blu|brn|gry|grn|hzl|oth)\b";
                return Regex.Match(Ecl, regex).Success;
            }

            private bool IsPidValid()
            {
                if (string.IsNullOrWhiteSpace(Pid))
                {
                    return false;
                }

                var regex = @"\b[0-9]{9}\b";
                return Regex.Match(Pid, regex).Success;
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
            return passports.Count(p => p.IsAlmostValid).ToString();
        }

        public override string Solve_2()
        {
            return passports.Count(p => p.IsCompletelyValid).ToString();
        }
    }
}