using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day08 : BaseDay
    {
        private readonly List<Instruction> instructions = new List<Instruction>();
        private Instruction lastInstruction;
        
        private class Instruction
        {
            public string Operation { get; set; }
            public int Argument { get; set; }
            public int ExecutionCount { get; set; }
        }
        
        public Day08()
        {
            foreach (var line in File.ReadLines(InputFilePath))
            {
                var match = Regex.Match(line, @"(nop|acc|jmp)\s(\+|-)([0-9]*)");
                var instruction = new Instruction
                {
                    Operation = match.Groups[1].Value,
                    Argument = match.Groups[2].Value == "+"
                        ? int.Parse(match.Groups[3].Value)
                        : -1 * int.Parse(match.Groups[3].Value)
                };
                instructions.Add(instruction);
            }

            lastInstruction = instructions.Last();
        }
        
        public override string Solve_1()
        {
            return ExecuteProgram().ToString();
        }
        
        public override string Solve_2()
        {
            var lastInstruction = instructions.Last();
            var globalResult = 0;
            
            foreach (var instruction in instructions)
            {
                foreach (var instr in instructions)
                {
                    instr.ExecutionCount = 0;
                }

                if (instruction.Operation == "nop")
                {
                    instruction.Operation = "jmp";
                    var result = ExecuteProgram();
                    if (lastInstruction.ExecutionCount == 1)
                    {
                        globalResult = result;
                        break;
                    }

                    instruction.Operation = "nop";
                }
                else if (instruction.Operation == "jmp")
                {
                    instruction.Operation = "nop";
                    var result = ExecuteProgram();
                    if (lastInstruction.ExecutionCount == 1)
                    {
                        globalResult = result;
                        break;
                    }

                    instruction.Operation = "jmp";
                }
            }

            return globalResult.ToString();
        }
        
        int ExecuteProgram()
        {
            var accumulator = 0;
            var programCounter = 0;
            
            while (true)
            {
                var instruction = instructions[programCounter];
                instruction.ExecutionCount += 1;
                if (instruction.ExecutionCount > 1)
                {
                    break;
                }
                
                switch (instruction.Operation)
                {
                    case "nop":
                        programCounter++;
                        break;
                    case "acc":
                        programCounter++;
                        accumulator += instruction.Argument;
                        break;
                    case "jmp":
                        programCounter += instruction.Argument;
                        break;
                }

                if (instruction == lastInstruction)
                {
                    break;
                }
            }

            return accumulator;
        }
    }
}