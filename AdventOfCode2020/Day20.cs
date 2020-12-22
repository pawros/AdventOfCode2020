using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day20 : BaseDay
    {
        private enum Edge
        {
            Top,
            Right,
            Bottom,
            Left,
            FlippedTop,
            FlippedRight,
            FlippedBottom,
            FlippedLeft
        }
        
        private class Tile
        {
            public int Id { get; set; }
            public readonly Dictionary<Edge, int> Matches = new Dictionary<Edge, int>();
            public char[,] T { get; set; } = new char[N, N];
            public char[,] Tc
            {
                get
                {
                    var tc = new char[C, C];
                    for (var i = 1; i < N - 1; i++)
                    {
                        for (var j = 1; j < N - 1; j++)
                        {
                            tc[i - 1, j - 1] = T[i, j];
                        }
                    }

                    return tc;
                }
            }

            public (int X, int Y) Position { get; set; }
        }

        private const int N = 10;
        private const int C = N - 2;
        private readonly int S;
        private readonly List<Tile> tiles = new List<Tile>();
        
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

                tiles.Add(new Tile {Id = tileId, T = t});
            }
            
            S = (int) Math.Sqrt(tiles.Count);
        }

        public override string Solve_1()
        {
            var edges = new Edge[] {Edge.Top, Edge.Right, Edge.Bottom, Edge.Left};
            var edgesToCheck = new Edge[]
            {
                Edge.Top, Edge.Right, Edge.Bottom, Edge.Left,
                Edge.FlippedTop, Edge.FlippedRight, Edge.FlippedBottom, Edge.FlippedLeft
            };

            for (var i = 0; i < tiles.Count - 1; i++)
            {
                for (var j = i + 1; j < tiles.Count; j++)
                {
                    var tile1 = tiles[i];
                    var tile2 = tiles[j];

                    foreach (var edge in edges)
                    {
                        foreach (var adjacentEdge in edgesToCheck)
                        {
                            if (GetEdge(tile1.T, edge).SequenceEqual(GetAdjacentEdge(tile2.T, adjacentEdge)))
                            {
                                tile1.Matches[edge] = tile2.Id;
                                tile2.Matches[adjacentEdge] = tile1.Id;
                            }
                        }
                    }
                }
            }
            
            var cornerTiles = tiles.Where(t => t.Matches.Count == 2).Select(t => t.Id).ToArray();
            var result = cornerTiles.Aggregate<int, long>(1, (current, cornerTile) => current * cornerTile);

            return result.ToString();
        }
        
        public override string Solve_2()
        {
            var seed0 = tiles.First(t => t.Matches.Count == 2);
            seed0.Position = (0, 0);

            var tilesToPlace = tiles.ToDictionary(x => x.Id, x => x);
            tilesToPlace.Remove(seed0.Id);

            var seeds = new Dictionary<int, Tile>();
            
            var rotated = seed0.T;
            for (var r = 0; r < 4; r++)
            {
                var re = GetEdge(rotated, Edge.Right);
                var hasRightAdjacent = TryGetAdjacent(re, tilesToPlace, Edge.Left, out var rt);

                var be = GetEdge(rotated, Edge.Bottom);
                var hasBottomAdjacent = TryGetAdjacent(be, tilesToPlace, Edge.Top, out var bt);

                if (hasRightAdjacent && hasBottomAdjacent)
                {
                    seed0.T = rotated;

                    var tr = tiles.Single(t => t.Id == rt.Id);
                    tr.T = rt.T;
                    tr.Position = (0, 1);

                    var tb = tiles.Single(t => t.Id == bt.Id);
                    tb.T = bt.T;
                    tb.Position = (1, 0);

                    tilesToPlace.Remove(tr.Id);
                    tilesToPlace.Remove(tb.Id);

                    seeds[tr.Id] = tr;
                    seeds[tb.Id] = tb;
                    
                    break;
                }
                
                rotated = Rotate(rotated);
            }

            while (seeds.Count > 0)
            {
                var seed = seeds.First().Value;
                
                var re = GetEdge(seed.T, Edge.Right);
                var hasR = TryGetAdjacent(re, tilesToPlace, Edge.Left, out var rt);
                if (hasR)
                {
                    var tr = tiles.Single(t => t.Id == rt.Id);
                    tr.T = rt.T;
                    tr.Position = (seed.Position.X, seed.Position.Y + 1);

                    tilesToPlace.Remove(tr.Id);
                    seeds[tr.Id] = tr;
                }
                
                var be = GetEdge(seed.T, Edge.Bottom);
                var hasB = TryGetAdjacent(be, tilesToPlace, Edge.Top, out var bt);
                if (hasB)
                {
                    var tb = tiles.Single(t => t.Id == bt.Id);
                    tb.T = bt.T;
                    tb.Position = (seed.Position.X + 1, seed.Position.Y);

                    tilesToPlace.Remove(tb.Id);
                    seeds[tb.Id] = tb;
                }

                seeds.Remove(seed.Id);
            }

            var image = MergeTiles();

            FindMonsters(image, out var newImg, out var monsterPositions);
            
            DrawMonsters(newImg, monsterPositions);

            PrintTile(newImg);
            
            return GetRoughness(newImg).ToString();
        }

        private bool TryGetAdjacent(char[] edge, Dictionary<int, Tile> tilesToPlace, Edge adjacentOn, out Tile resultTile)
        {
            resultTile = null;
            foreach (var (_, tile) in tilesToPlace)
            {
                var t1 = tile.T;
                for (var r1 = 0; r1 < 4; r1++)
                {
                    var leftEdge = GetAdjacentEdge(t1, adjacentOn);
                    if (edge.SequenceEqual(leftEdge))
                    {
                        tile.T = t1;
                        resultTile = tile;
                        return true;
                    }

                    t1 = Rotate(t1);
                }

                t1 = tile.T;
                t1 = Flip(t1);
                for (var r1 = 0; r1 < 4; r1++)
                {
                    var leftEdge = GetAdjacentEdge(t1, adjacentOn);
                    if (edge.SequenceEqual(leftEdge))
                    {
                        tile.T = t1;
                        resultTile = tile;
                        return true;
                    }

                    t1 = Rotate(t1);
                }
            }
            
            return false;
        }

        private char[,] MergeTiles()
        {
            var image = new char[S * C, S * C];
            for (var i = 0; i < S; i++)
            {
                for (var j = 0; j < S; j++)
                {
                    var tile = tiles.Single(t => t.Position.X == i && t.Position.Y == j);

                    for (var x = 0; x < C; x++)
                    {
                        for (var y = 0; y < C; y++)
                        {
                            image[i * C + x, j * C + y] = tile.Tc[x, y];
                        }
                    }
                }
            }

            return image;
        }

        private void FindMonsters(char[,] image, out char[,] newImg, out List<(int, int)> monsterPositions)
        {
            var monsterParts = 15;
            var monster = CreateMonster();
            
            var img = image;
            var patX = monster.GetLength(0);
            var patY = monster.GetLength(1);
            var accX = img.GetLength(0) - patX + 1;
            var accY = img.GetLength(1) - patY + 1;

            monsterPositions = new List<(int x, int y)>();
            
            var accumulator = new int[accX, accY];
            for (var r = 0; r < 4; r++)
            {
                for (var ax = 0; ax < accX; ax++)
                {
                    for (var ay = 0; ay < accY; ay++)
                    {
                        var accValue = 0;
                        for (var px = 0; px < patX; px++)
                        {
                            for (var py = 0; py < patY; py++)
                            {
                                if (monster[px, py] == img[ax + px, ay + py])
                                {
                                    accValue++;
                                }
                            }
                        }

                        accumulator[ax, ay] = accValue;
                    }
                }

                for (var ax = 0; ax < accX; ax++)
                {
                    for (var ay = 0; ay < accY; ay++)
                    {
                        if (accumulator[ax, ay] == monsterParts)
                        {
                            monsterPositions.Add((ax, ay));
                        }
                    }
                }

                if (monsterPositions.Count > 0)
                {
                    newImg = img;
                    return;
                }
                
                monsterPositions.Clear();
                img = Rotate(img);
            }

            img = image;
            img = Flip(img);
            
            for (var r = 0; r < 4; r++)
            {
                for (var ax = 0; ax < accX; ax++)
                {
                    for (var ay = 0; ay < accY; ay++)
                    {
                        var accValue = 0;
                        for (var px = 0; px < patX; px++)
                        {
                            for (var py = 0; py < patY; py++)
                            {
                                if (monster[px, py] == img[ax + px, ay + py])
                                {
                                    accValue++;
                                }
                            }
                        }

                        accumulator[ax, ay] = accValue;
                    }
                }

                for (var ax = 0; ax < accX; ax++)
                {
                    for (var ay = 0; ay < accY; ay++)
                    {
                        if (accumulator[ax, ay] == monsterParts)
                        {
                            monsterPositions.Add((ax, ay));
                        }
                    }
                }

                if (monsterPositions.Count > 0)
                {
                    newImg = img;
                    return;
                }
                
                monsterPositions.Clear();
                img = Rotate(img);
            }

            newImg = null;
        }

        private void DrawMonsters(char[,] img, List<(int x, int y)> monsterPositions)
        {
            var monster = CreateMonster();
            var patX = monster.GetLength(0);
            var patY = monster.GetLength(1);
            
            foreach (var mp in monsterPositions)
            {
                for (var px = 0; px < patX; px++)
                {
                    for (var py = 0; py < patY; py++)
                    {
                        if (monster[px, py] == '#')
                        {
                            img[mp.x + px, mp.y + py] = 'O';
                        }
                    }
                }
            }
        }
        
        private char[,] CreateMonster()
        {
            var monster = new char[3, 20];
            monster[0, 18] = '#';
            monster[1, 0] = '#';
            monster[1, 5] = '#';
            monster[1, 6] = '#';
            monster[1, 11] = '#';
            monster[1, 12] = '#';
            monster[1, 17] = '#';
            monster[1, 18] = '#';
            monster[1, 19] = '#';
            monster[2, 1] = '#';
            monster[2, 4] = '#';
            monster[2, 7] = '#';
            monster[2, 10] = '#';
            monster[2, 13] = '#';
            monster[2, 16] = '#';

            return monster;
        }
        
        private static char[,] Flip(char[,] tile)
        {
            var n = tile.GetLength(0);
            
            var r = new char[n, n];
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    r[i, n - j - 1] = tile[i, j];
                }
            }

            return r;
        }
        
        private static char[,] Rotate(char[,] tile)
        {
            var n = tile.GetLength(0);
            
            var r = new char[n, n];
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    r[j, n - i - 1] = tile[i, j];
                }
            }

            return r;
        }

        private static char[] GetEdge(char[,] tile, Edge edge)
        {
            var e = new char[N];

            if (edge == Edge.Top)
                for (var y = 0; y < N; y++)
                {
                    e[y] = tile[0, y];
                }
            else if (edge == Edge.Right)
                for (var x = 0; x < N; x++)
                {
                    e[x] = tile[x, N - 1];
                }
            else if (edge == Edge.Bottom)
                for (var y = N - 1; y >= 0; y--)
                {
                    e[N - y - 1] = tile[N - 1, y];
                }
            else if (edge == Edge.Left)
                for (var x = N - 1; x >= 0; x--)
                {
                    e[N - x - 1] = tile[x, 0];
                }

            return e;
        }

        private static char[] GetAdjacentEdge(char[,] tile, Edge edge)
        {
            var e = new char[N];

            if (edge == Edge.Top)
                for (var y = N - 1; y >= 0; y--)
                {
                    e[N - y - 1] = tile[0, y];
                }
            else if (edge == Edge.Right)
                for (var x = N - 1; x >= 0; x--)
                {
                    e[N - x - 1] = tile[x, N - 1];
                }
            else if (edge == Edge.Bottom)
                for (var y = 0; y < N; y++)
                {
                    e[y] = tile[N - 1, y];
                }
            else if (edge == Edge.Left)
                for (var x = 0; x < N; x++)
                {
                    e[x] = tile[x, 0];
                }
            else if (edge == Edge.FlippedTop)
                for (var y = 0; y < N; y++)
                {
                    e[y] = tile[0, y];
                }
            else if (edge == Edge.FlippedRight)
                for (var x = N - 1; x >= 0; x--)
                {
                    e[N - x - 1] = tile[x, 0];
                }
            else if (edge == Edge.FlippedBottom)
                for (var y = N - 1; y >= 0; y--)
                {
                    e[N - y - 1] = tile[N - 1, y];
                }
            else if (edge == Edge.FlippedLeft)
                for (var x = 0; x < N; x++)
                {
                    e[x] = tile[x, N - 1];
                }

            return e;
        }

        private static int GetRoughness(char[,] tile)
        {
            var r = 0;
            for (var i = 0; i < tile.GetLength(0); i++)
            {
                for (var j = 0; j < tile.GetLength(1); j++)
                {
                    if (tile[i, j] == '#')
                    {
                        r++;
                    }
                }
            }

            return r;
        }
        
        private static void PrintTile(char[,] tile)
        {
            for (var i = 0; i < tile.GetLength(0); i++)
            {
                for (var j = 0; j < tile.GetLength(1); j++)
                {
                    Console.Write(tile[i, j]);
                }

                Console.WriteLine();
            }
        }
    }
}