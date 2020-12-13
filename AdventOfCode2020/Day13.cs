using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day13 : BaseDay
    {
        private int timestamp;
        private int[] buses;
        private readonly List<(int Line, int Offset)> busesOffset;

        public Day13()
        {
            var input = File.ReadAllLines(InputFilePath);
            timestamp = int.Parse(input[0]);
            buses = input[1].Split(',').Where(x => x != "x").Select(x => int.Parse(x)).ToArray();

            var inputBuses = input[1].Split(',');
            busesOffset = new List<(int, int)>();
            for (int i = 0; i < inputBuses.Length; i++)
            {
                if (inputBuses[i] != "x")
                {
                    busesOffset.Add((int.Parse(inputBuses[i]), i));
                }
            }
        }
        
        public override string Solve_1()
        {
            var minBus = 0;
            var minDeparture = 0;
            foreach (var bus in buses)
            {
                var departure = (timestamp / bus + 1) * bus;
                if (minDeparture == 0 || departure < minDeparture)
                {
                    minDeparture = departure;
                    minBus = bus;
                }
            }
            
            return ((minDeparture - timestamp) * minBus).ToString();
        }

        public override string Solve_2()
        {
            var rest = (ulong)busesOffset[0].Offset;
            var mod = (ulong)busesOffset[0].Line;
            ulong time = 0;
            for (var n = 0; n < busesOffset.Count - 1; n++)
            {
                var nextMod = (ulong)busesOffset[n + 1].Line;
                var nextRest = (ulong)((busesOffset[n + 1].Line - busesOffset[n + 1].Offset % busesOffset[n + 1].Line) % busesOffset[n + 1].Line);
                
                var i = 0;
                while (time % nextMod != nextRest)
                {
                    time = mod * (ulong) i++ + rest;
                }

                mod *= nextMod;
                rest = time;
            }
            
            return rest.ToString();
        }
    }
}