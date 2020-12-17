using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day16 : BaseDay
    {
        private class Field
        {
            public string Name { get; set; }
            public (int a, int b)[] Ranges { get; set; }

            public int Position { get; set; }
            public Field()
            {
                Ranges = new (int a, int b)[2];
            }
        }
        
        private class Ticket
        {
            public int[] Values { get; set; }
            public bool IsValid { get; set; }
        }
        
        private readonly Field[] fields;
        private readonly int[] ticket;
        private readonly Ticket[] nearbyTickets;
        
        public Day16()
        {
            var input = File.ReadAllText(InputFilePath).Split("\r\n\r\n");

            var fieldsInput = input[0].Split("\r\n");
            fields = new Field[fieldsInput.Length];
            for (var i = 0; i < fieldsInput.Length; i++)
            {
                
                var line = fieldsInput[i];
                var match = Regex.Match(line, @"^([\sa-z]*):\s([0-9]*)-([0-9]*)\sor\s([0-9]*)-([0-9]*)");

                fields[i] = new Field
                {
                    Name = match.Groups[1].Value,
                    Ranges =
                    {
                        [0] = (int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value)),
                        [1] = (int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value))
                    }
                };

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
                var ticketErrorRate = nearbyTicket.Values.Where(x => !fields.Any(f => f.Ranges.Any(r => InRange(x, r)))).Sum();
                if (ticketErrorRate > 0)
                {
                    nearbyTicket.IsValid = false;
                }

                errorRate += ticketErrorRate;
            }

            return errorRate.ToString();
        }
        
        public override string Solve_2()
        {
            var validTickets = nearbyTickets.Where(t => t.IsValid).ToArray();
            var positionValues = new Dictionary<int, int[]>(validTickets.Length);
            
            for (var i = 0; i < ticket.Length; i++)
            {
                positionValues[i] = validTickets.Select(t => t.Values[i]).ToArray();
            }

            while (positionValues.Count > 0)
            {
                foreach (var field in fields)
                {
                    var matchedPositionValues = positionValues.Where(p => p.Value.All(x => field.Ranges.Any(r => InRange(x, r)))).ToArray();
                    if (matchedPositionValues.Length == 1)
                    {
                        var position = matchedPositionValues.Single().Key;
                        field.Position = position;
                        positionValues.Remove(position);
                    }
                }
            }
            
            long result = fields
                .Where(f => f.Name.StartsWith("departure"))
                .Aggregate<Field, long>(1, (current, departureField) => current * ticket[departureField.Position]);

            return result.ToString();
        }
        
        private static bool InRange(int n, (int a, int b) range)
        {
            return n >= range.a && n <= range.b;
        }
    }
}