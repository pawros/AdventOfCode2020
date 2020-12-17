using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day17 : BaseDay
    {
        private readonly Dictionary<(int x, int y, int z), bool> cubes3d;
        private readonly Dictionary<(int x, int y, int z, int w), bool> cubes4d;
        private const int N = 12;
        public Day17()
        {
            var input = File.ReadAllLines(InputFilePath);
            
            cubes3d = new Dictionary<(int x, int y, int z), bool>();
            for (var i = 0; i < input.Length; i++)
            {
                for (var j = 0; j < input[i].Length; j++)
                {
                    if (input[i][j] == '#')
                    {
                        cubes3d.Add((i, j, 0), true);
                    }
                }
            }

            cubes4d = new Dictionary<(int x, int y, int z, int w), bool>();
            for (var i = 0; i < input.Length; i++)
            {
                for (var j = 0; j < input[i].Length; j++)
                {
                    if (input[i][j] == '#')
                    {
                        cubes4d.Add((i, j, 0, 0), true);
                    }
                }
            }
            
        }

        public override string Solve_1()
        {
            for (var i = 0; i < 6; i++)
            {
                var cubesToAdd = new List<(int, int, int)>();
                var cubesToRemove = new List<(int, int, int)>();
                
                for (var z = -N; z <= N; z++)
                {
                    for (var x = -N; x <= N; x++)
                    {
                        for (var y = -N; y <= N; y++)
                        {
                            var c = (x, y, z);
                            var neighbors = 0;
                            
                            for (var dz = -1; dz <= 1; dz++)
                            {
                                for (var dx = -1; dx <= 1; dx++)
                                {
                                    for (var dy = -1; dy <= 1; dy++)
                                    {
                                        if (dx == 0 && dy == 0 && dz == 0)
                                        {
                                            continue;
                                        }

                                        var dc = (x + dx, y + dy, z + dz);
                                        if (cubes3d.ContainsKey(dc))
                                        {
                                            neighbors++;
                                        }
                                    }
                                }
                            }

                            if (cubes3d.ContainsKey(c))
                            {
                                if (neighbors != 2 && neighbors != 3)
                                {
                                    cubesToRemove.Add(c);
                                }
                            }
                            else
                            {
                                if (neighbors == 3)
                                {
                                    cubesToAdd.Add(c);
                                }
                            }
                        }
                    }
                }

                foreach (var c in cubesToAdd)
                {
                    cubes3d.Add(c, true);
                }

                foreach (var c in cubesToRemove)
                {
                    cubes3d.Remove(c);
                }
            }
            return cubes3d.Count.ToString();
        }

        public override string Solve_2()
        {
            for (var i = 0; i < 6; i++)
            {
                var cubesToAdd = new List<(int, int, int, int)>();
                var cubesToRemove = new List<(int, int, int, int)>();
                
                for (var z = -N; z <= N; z++)
                {
                    for (var x = -N; x <= N; x++)
                    {
                        for (var y = -N; y <= N; y++)
                        {
                            for (var w = -N; w <= N; w++)
                            {
                                var c = (x, y, z, w);
                                var neighbors = 0;
                                
                                for (var dz = -1; dz <= 1; dz++)
                                {
                                    for (var dx = -1; dx <= 1; dx++)
                                    {
                                        for (var dy = -1; dy <= 1; dy++)
                                        {
                                            for (var dw = -1; dw <= 1; dw++)
                                            {
                                                if (dx == 0 && dy == 0 && dz == 0 && dw == 0)
                                                {
                                                    continue;
                                                }

                                                var dc = (x + dx, y + dy, z + dz, w + dw);
                                                if (cubes4d.ContainsKey(dc))
                                                {
                                                    neighbors++;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (cubes4d.ContainsKey(c))
                                {
                                    if (neighbors != 2 && neighbors != 3)
                                    {
                                        cubesToRemove.Add(c);
                                    }
                                }
                                else
                                {
                                    if (neighbors == 3)
                                    {
                                        cubesToAdd.Add(c);
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (var c in cubesToAdd)
                {
                    cubes4d.Add(c, true);
                }

                foreach (var c in cubesToRemove)
                {
                    cubes4d.Remove(c);
                }
            }
            return cubes4d.Count.ToString();
        }
    }
}