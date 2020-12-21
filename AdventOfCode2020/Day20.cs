using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day20 : BaseDay
    {
        private class Tile
        {
            public int Id { get; set; }
            public char[,] T { get; set; } = new char[N, N];
            public Dictionary<Edge, int> Matches = new Dictionary<Edge, int>();
        }
        
        private const int N = 10;
        private List<Tile> tiles = new List<Tile>();

        public Day20()
        {
            var input = File.ReadAllText(InputFilePath).Split("\r\n\r\n");
            foreach (var tileInput in input)
            {
                var tileSplit = tileInput.Split(":\r\n");
                var tileId = int.Parse(Regex.Match(tileSplit[0], @"Tile\s([0-9]*)").Groups[1].Value);

                var t = new char[N, N];
                var tileLines = tileSplit[1].Split("\r\n");
                for (var i = 0; i < N; i++)
                {
                    for (var j = 0; j < N; j++)
                    {
                        t[i, j] = tileLines[i][j];
                    }
                }

                tiles.Add(new Tile { Id = tileId, T = t });
            }
        }
        
        public override string Solve_1()
        {
            var edges = new Edge[] {Edge.Top, Edge.Right, Edge.Bottom, Edge.Left};
            
            for (var i = 0; i < tiles.Count - 2; i++)
            {
                for (var j = i + 1; j < tiles.Count - 1; j++)
                {
                    var tile1 = tiles[i];
                    var tile2 = tiles[j];

                    foreach (var edge1 in edges)
                    {
                        foreach (var edge2 in edges)
                        {
                            if (GetEdge(tile1.T, edge1).SequenceEqual(GetRotatedEdge(tile2.T, edge2)))
                            {
                                tile1.Matches[edge1] = tile2.Id;
                                tile2.Matches[edge2] = tile1.Id;
                            }
                        }
                    }
                }
            }
            
            throw new System.NotImplementedException();
        }

        private enum Edge
        {
            Top,
            Right,
            Bottom,
            Left
        }
        
        private char[] GetEdge(char[,] tile, Edge edge)
        {
            var e = new char[N];
            
            switch (edge)
            {
                case Edge.Top:
                {
                    for (var y = 0; y < N; y++)
                    {
                        e[y] = tile[0, y];
                    }

                    break;
                }

                case Edge.Right:
                {
                    for (var x = 0; x < N; x++)
                    {
                        e[x] = tile[x, N - 1];
                    }

                    break;
                }
                
                case Edge.Bottom:
                {
                    for (var y = N - 1; y >= 0; y--)
                    {
                        e[N - y - 1] = tile[N - 1, y];
                    }

                    break;
                }
                
                case Edge.Left:
                    for (var x = N - 1; x >= 0; x--)
                    {
                        e[N - x - 1] = tile[x, 0];
                    }
                
                    break;
            }

            return e;
        }

        private char[] GetRotatedEdge(char[,] tile, Edge edge)
        {
            var e = new char[N];

            switch (edge)
            {
                case Edge.Top:
                {
                    for (var y = N - 1; y >= 0; y--)
                    {
                        e[N - y - 1] = tile[0, y];
                    }

                    break;
                }
                
                case Edge.Right:
                {
                    for (var x = N - 1; x >= 0; x--)
                    {
                        e[N - x - 1] = tile[x, N - 1];
                    }

                    break;
                }

                case Edge.Bottom:
                {
                    for (var y = 0; y < N; y++)
                    {
                        e[y] = tile[N - 1, y];
                    }

                    break;
                }
                
                case Edge.Left:
                {
                    for (var x = 0; x < N; x++)
                    {
                        e[x] = tile[x, 0];
                    }

                    break;
                }
            }

            return e;
        }
        
        public override string Solve_2()
        {
            throw new System.NotImplementedException();
        }
    }
}