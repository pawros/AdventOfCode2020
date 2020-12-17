using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day16 : BaseDay
    {
        private class Ticket
        {
            public int[] Values { get; set; }
            public bool IsValid { get; set; }
        }
        
        private readonly (int a, int b)[] ranges;
        private readonly int[] ticket;
        private readonly Ticket[] nearbyTickets;
        public Day16()
        {
            var input = File.ReadAllText(InputFilePath).Split("\r\n\r\n");

            var fieldsInput = input[0].Split("\r\n");
            ranges = new (int a, int b)[fieldsInput.Length * 2];
            for (var i = 0; i < fieldsInput.Length; i++)
            {
                var line = fieldsInput[i];
                var match = Regex.Match(line, @"^[\sa-z]*:\s([0-9]*)-([0-9]*)\sor\s([0-9]*)-([0-9]*)");
                
                ranges[2 * i] = (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
                ranges[2 * i + 1] = (int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
            }

            var ticketInput = input[1].Split("\r\n");
            ticket = ticketInput[1].Split(',').Select(x => int.Parse(x)).ToArray();
            
            var nearbyTicketsInput = input[2].Split("\r\n");
            nearbyTickets = new Ticket[nearbyTicketsInput.Length - 1];
            for (var i = 1; i < nearbyTicketsInput.Length; i++)
            {
                var ticket = new Ticket
                {
                    Values = nearbyTicketsInput[i].Split(',').Select(x => int.Parse(x)).ToArray(),
                    IsValid = true
                };
                nearbyTickets[i - 1] = ticket;
            }
        }
        
        public override string Solve_1()
        {
            var errorRate = 0;
            foreach (var nearbyTicket in nearbyTickets)
            {
                var ticketErrorRate = nearbyTicket.Values.Where(x => !ranges.Any(r => InRange(x, r))).Sum();
                if (ticketErrorRate > 0)
                {
                    nearbyTicket.IsValid = false;
                }

                errorRate += ticketErrorRate;
            }

            bool InRange(int n, (int a, int b) range)
            {
                return n >= range.a && n <= range.b;
            }
            
            return errorRate.ToString();
        }

        public override string Solve_2()
        {
            var validTickets = nearbyTickets.Where(t => t.IsValid).ToArray();
            var positionValues = new Dictionary<int, int[]>(validTickets.Length);
            
            for (var i = 0; i < validTickets.Length; i++)
            {
                positionValues[i] = validTickets.Select(t => t.Values[i]).ToArray();
            }
            
            return string.Empty;
        }
    }
}