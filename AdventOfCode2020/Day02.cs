using System;
using System.IO;
using System.Linq;
using AoCHelper;

namespace AdventOfCode2020
{
    public class Policy
    {
        public int Lower { get; set; }
        public int Upper { get; set; }
        public char Character { get; set; }
        public string Password { get; set; }
    }
    
    public sealed class Day02 : BaseDay
    {
        private static Policy[] policies;
        
        public Day02()
        {
            policies = File.ReadAllLines(InputFilePath).Select(x =>
            {
                var split = x.Split(' ');
                var rangeSplit = split[0].Split('-');
                var policy = new Policy
                {
                    Lower = int.Parse(rangeSplit[0]),
                    Upper = int.Parse(rangeSplit[1]),
                    Character = split[1][0],
                    Password = split[2]
                };
                return policy;
            }).ToArray();
        }
        
        public override string Solve_1()
        {
            var valid = 0;
            foreach (var policy in policies)
            {
                var occurrences = policy.Password.Count(c => c == policy.Character);
                if (occurrences >= policy.Lower && occurrences <= policy.Upper)
                {
                    valid++;
                }
            }

            return valid.ToString();
        }

        public override string Solve_2()
        {
            var valid = 0;
            foreach (var policy in policies)
            {
                var index1 = policy.Lower - 1 ;
                var index2 = policy.Upper - 1;

                if (policy.Password[index1] == policy.Character && policy.Password[index2] != policy.Character)
                {
                    valid++;
                }
                else if (policy.Password[index1] != policy.Character && policy.Password[index2] == policy.Character)
                {
                    valid++;
                }
            }
            return valid.ToString();
        }
    }
}