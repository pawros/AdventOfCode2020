using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day12 : BaseDay
    {
        private readonly string[] input;

        public Day12()
        {
            input = File.ReadAllLines(InputFilePath);
        }
        
        private readonly Dictionary<char, Dictionary<int, char>> map = new Dictionary<char, Dictionary<int, char>>()
        {
            ['N'] = new Dictionary<int, char>()
            {
                {0, 'N'},
                {1, 'E'},
                {2, 'S'},
                {3, 'W'},
                {-1, 'W'},
                {-2, 'S'},
                {-3, 'E'},
            },
            ['E'] = new Dictionary<int, char>()
            {
                {0, 'E'},
                {1, 'S'},
                {2, 'W'},
                {3, 'N'},
                {-1, 'N'},
                {-2, 'W'},
                {-3, 'S'},
            },
            ['S'] = new Dictionary<int, char>()
            {
                {0, 'S'},
                {1, 'W'},
                {2, 'N'},
                {3, 'E'},
                {-1, 'E'},
                {-2, 'N'},
                {-3, 'W'},
            },
            ['W'] = new Dictionary<int, char>()
            {
                {0, 'W'},
                {1, 'N'},
                {2, 'E'},
                {3, 'S'},
                {-1, 'S'},
                {-2, 'E'},
                {-3, 'N'},
            }
        };
        
        public override string Solve_1()
        {
            (int x, int y) position = (0, 0);
            var face = 'E';
            
            foreach (var s in input)
            {
                var match = Regex.Match(s, @"(N|S|E|W|L|R|F)([0-9]*)");
                var action = match.Groups[1].Value[0];
                var value = int.Parse(match.Groups[2].Value);

                switch (action)
                {
                    case 'N':
                    case 'S':
                    case 'E':
                    case 'W':
                        Move(action, value);
                        break;
                    case 'L':
                    case 'R':
                        Turn(action, value);
                        break;
                    case 'F':
                        Move(face, value);
                        break;
                }
            }

            return (Math.Abs(position.x) + Math.Abs(position.y)).ToString();
            
            void Move(char d, int value)
            {
                switch (d)
                {
                    case 'N':
                        position.y += value;
                        break;
                    case 'S':
                        position.y -= value;
                        break;
                    case 'E':
                        position.x += value;
                        break;
                    case 'W':
                        position.x -= value;
                        break;
                }
            };
            
            void Turn(char r, int deg)
            {
                var turns = deg / 90;
                switch (r)
                {
                    case 'R':
                        face = map[face][turns];
                        break;
                    case 'L':
                        face = map[face][-turns];
                        break;
                }
            }
            
        }

        public override string Solve_2()
        {
            (int x, int y) shipPosition = (0, 0);
            (int x, int y) waypointPosition = (10, 1);
            var face = 'E';
            
            foreach (var s in input)
            {
                var match = Regex.Match(s, @"(N|S|E|W|L|R|F)([0-9]*)");
                var action = match.Groups[1].Value[0];
                var value = int.Parse(match.Groups[2].Value);

                switch (action)
                {
                    case 'N':
                    case 'S':
                    case 'E':
                    case 'W':
                        MoveWaypoint(action, value);
                        break;
                    case 'L':
                    case 'R':
                        TurnWaypoint(action, value);
                        break;
                    case 'F':
                        MoveShip(value);
                        break;
                }
            }

            return (Math.Abs(shipPosition.x) + Math.Abs(shipPosition.y)).ToString();
            
            void MoveWaypoint(char d, int value)
            {
                switch (d)
                {
                    case 'N':
                        waypointPosition.y += value;
                        break;
                    case 'S':
                        waypointPosition.y -= value;
                        break;
                    case 'E':
                        waypointPosition.x += value;
                        break;
                    case 'W':
                        waypointPosition.x -= value;
                        break;
                }
            };

            void MoveShip(int value)
            {
                shipPosition.x += waypointPosition.x * value;
                shipPosition.y += waypointPosition.y * value;
            }
            
            void TurnWaypoint(char r, int deg)
            {
                var turns = deg / 90;
                if (r == 'L')
                {
                    turns = -turns;
                }
                
                var temp = 0;
                switch (turns)
                {
                    case 0:
                        return;
                    case 1:
                    case -3:
                        temp = waypointPosition.x;
                        waypointPosition.x = waypointPosition.y;
                        waypointPosition.y = -temp;
                        break;
                    case -1:
                    case 3:
                        temp = waypointPosition.x;
                        waypointPosition.x = -waypointPosition.y;
                        waypointPosition.y = temp;
                        break;
                    case 2:
                    case -2:
                        waypointPosition.x = -waypointPosition.x;
                        waypointPosition.y = -waypointPosition.y;
                        break;
                }
                
                switch (r)
                {
                    case 'R':
                        face = map[face][turns];
                        break;
                    case 'L':
                        face = map[face][-turns];
                        break;
                }
            }        
        }
    }
}