using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day18: BaseDay
    {
        private string[] input;
        private List<char[]> expressions;

        public Day18()
        {
            input = File.ReadAllLines(InputFilePath);
            expressions = input.Select(i => i.Replace(" ", String.Empty).ToCharArray()).ToList();
        }
        
        public override string Solve_1()
        {
            var getPriority = new Func<char, int>(c => c == '+' || c == '*' ? 1 : 0);
            
            return expressions.Select(expression => ToRPN(expression, getPriority)).Select(SolveRPN).Sum().ToString();
        }

        public override string Solve_2()
        {
            var getPriority = new Func<char, int>(c =>
            {
                return c switch
                {
                    '+' => 2,
                    '*' => 1,
                    _ => 0
                };
            });
            
            return expressions.Select(expression => ToRPN(expression, getPriority)).Select(SolveRPN).Sum().ToString();
        }
        
        private static List<char> ToRPN(char[] tokens, Func<char, int> getPriority)
        {
            var stack = new Stack<char>();
            var output = new List<char>();
            
            foreach (var token in tokens)
            {
                if(int.TryParse(token.ToString(), out var _))
                {
                    output.Add(token);
                    continue;
                }
                
                switch (token)
                {
                    case '(':
                        stack.Push(token);
                        break;
                    case ')':
                    {
                        while (stack.Count > 0 && stack.Peek() != '(')
                        {
                            output.Add(stack.Pop());
                        }

                        stack.Pop();
                        break;
                    }
                    case '+':
                    case '*':
                        while (stack.Count > 0 && getPriority(stack.Peek()) >= getPriority(token))
                        {
                            output.Add(stack.Pop());
                        }
                        
                        stack.Push(token);
                        break;
                }
            }

            while (stack.Count > 0)
            {
                output.Add(stack.Pop());
            }

            return output;
        }

        private static long SolveRPN(IEnumerable<char> tokens)
        {
            var stack = new Stack<long>();
            foreach (var token in tokens)
            {
                if (int.TryParse(token.ToString(), out var parsedToken))
                {
                    stack.Push(parsedToken);
                    continue;
                }

                switch (token)
                {
                    case '+':
                    {
                        long result = stack.Pop() + stack.Pop();
                        stack.Push(result);
                        break;
                    }
                    case '*':
                    {
                        long result = stack.Pop() * stack.Pop();
                        stack.Push(result);
                        break;
                    }
                }
            }

            return stack.Pop();
        }
    }
}