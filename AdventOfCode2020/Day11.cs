using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCHelper;
using Microsoft.CSharp.RuntimeBinder;

namespace AdventOfCode2020
{
    public sealed class Day11 : BaseDay
    {
        private char[,] seats;
        private (int x, int y)[] directions = new (int, int)[]
        {
            (-1, -1),
            (-1,  0),
            (-1,  1),
            ( 0, -1),
            ( 0,  1),
            ( 1, -1),
            ( 1,  0),
            ( 1,  1),
        };
        
        
        public Day11()
        {
            var input = File.ReadAllLines(InputFilePath);
            seats = new char[input.Length, input[0].Length];
            for (var x = 0; x < seats.GetLength(0); x++)
            {
                for (var y = 0; y < seats.GetLength(1); y++)
                {
                    seats[x, y] = input[x][y];
                }
            }
        }
        
        public override string Solve_1()
        {
            var s1 = (char[,])seats.Clone();
            var s2 = (char[,])seats.Clone();

            var different = true;
            while(different)
            {
                for (var x = 0; x < seats.GetLength(0); x++)
                {
                    for (var y = 0; y < seats.GetLength(1); y++)
                    {
                        switch (s1[x, y])
                        {
                            case 'L':
                                var anyOccupied = false;
                                foreach (var d in directions)
                                {
                                
                                    var dx = x + d.x;
                                    var dy = y + d.y;
                                
                                    if (dx >= 0 && dx < seats.GetLength(0) && 
                                        dy >= 0 && dy < seats.GetLength(1))
                                    {
                                        if (s1[dx, dy] == '#')
                                        {
                                            anyOccupied = true;
                                            break;
                                        }
                                    }
                                }

                                if (!anyOccupied)
                                {
                                    s2[x, y] = '#';
                                }
                            
                                break;
                        
                            case '#':
                                var occupied = 0;
                                foreach (var d in directions)
                                {
                                    var dx = x + d.x;
                                    var dy = y + d.y;
                                
                                    if (dx >= 0 && dx < seats.GetLength(0) && 
                                        dy >= 0 && dy < seats.GetLength(1))
                                    {
                                        if (s1[dx, dy] == '#')
                                        {
                                            occupied++;
                                        }
                                    
                                        if (occupied >= 4)
                                        {
                                            s2[x, y] = 'L';
                                            break;
                                        }
                                    }
                                }
                            
                                break;
                        }
                    }
                }

                different = !AreTheSame(s1, s2);
                s1 = (char[,]) s2.Clone();
            }
            
            return CountOccupied(s1).ToString();
        }

        
        public override string Solve_2()
        {
            var s1 = (char[,])seats.Clone();
            var s2 = (char[,])seats.Clone();

            var different = true;
            while(different)
            {
                for (var x = 0; x < seats.GetLength(0); x++)
                {
                    for (var y = 0; y < seats.GetLength(1); y++)
                    {
                        var vectors = new List<List<char>>();
                        switch (s1[x, y])
                        {
                            case 'L':
                                
                                foreach (var d in directions)
                                {
                                    var directionVector = new List<char>();
                                    for (var m = 1; m < seats.GetLength(0); m++)
                                    {
                                        var dx = x + m * d.x ;
                                        var dy = y + m * d.y;
                                        
                                        if (dx >= 0 && dx < seats.GetLength(0) && 
                                            dy >= 0 && dy < seats.GetLength(1))
                                        {
                                            directionVector.Add(s1[dx, dy]);
                                        }
                                    }
                                    vectors.Add(directionVector);
                                }

                                var anyOccupied = false;
                                foreach (var vector in vectors)
                                {
                                    if (GetFirstVisible(vector) == '#')
                                    {
                                        anyOccupied = true;
                                        break;
                                    }
                                }
                                
                                if (!anyOccupied)
                                {
                                    s2[x, y] = '#';
                                }
                            
                                break;
                        
                            case '#':
                                var occupied = 0;
                                
                                foreach (var d in directions)
                                {
                                    var directionVector = new List<char>();
                                    for (var m = 1; m < seats.GetLength(0); m++)
                                    {
                                        var dx = x + m * d.x ;
                                        var dy = y + m * d.y;
                                        
                                        if (dx >= 0 && dx < seats.GetLength(0) && 
                                            dy >= 0 && dy < seats.GetLength(1))
                                        {
                                            directionVector.Add(s1[dx, dy]);
                                        }
                                    }
                                    vectors.Add(directionVector);
                                }

                                foreach (var vector in vectors)
                                {
                                    if (GetFirstVisible(vector) == '#')
                                    {
                                        occupied++;
                                    }
                                    
                                    if (occupied >= 5)
                                    {
                                        s2[x, y] = 'L';
                                        break;
                                    }
                                }
                            
                                break;
                        }
                    }
                }

                different = !AreTheSame(s1, s2);
                s1 = (char[,]) s2.Clone();
            }
            
            return CountOccupied(s1).ToString();
        }

        private char GetFirstVisible(IEnumerable<char> vector)
        {
            foreach (var c in vector)
            {
                if (c == 'L' || c == '#')
                {
                    return c;
                }
            }

            return '.';
        }
        
        private int CountOccupied(char[,] s)
        {
            var occupied = 0;
            for (var x = 0; x < s.GetLength(0); x++)
            {
                for (var y = 0; y < s.GetLength(1); y++)
                {
                    if (s[x, y] == '#')
                    {
                        occupied++;
                    }
                }
            }
            
            return occupied;
        }
        
        private bool AreTheSame(char[,] s1, char[,] s2)
        {
            for (var x = 0; x < seats.GetLength(0); x++)
            {
                for (var y = 0; y < seats.GetLength(1); y++)
                {
                    if (s1[x, y] != s2[x, y])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}