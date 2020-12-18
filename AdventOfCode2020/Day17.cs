using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day17 : BaseDay
    {
        private Dictionary<(int x, int y, int z), bool> cubes3d;
        private Dictionary<(int x, int y, int z, int w), bool> cubes4d;
        
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
                var cubesToCheck = new List<(int x, int y, int z)>();
                foreach (var cube in cubes3d.Keys)
                {
                    cubesToCheck.AddRange(Deltas().Select(d => (cube.x + d.x, cube.y + d.y, cube.z + d.z)));
                }

                var newCubes3d = new Dictionary<(int x, int y, int z), bool>();
                foreach (var cube in cubesToCheck)
                {
                    var neighbors = Deltas().Count(d => cubes3d.ContainsKey((cube.x + d.x, cube.y + d.y, cube.z + d.z)));

                    if (cubes3d.ContainsKey(cube) && (neighbors == 2 || neighbors == 3))
                    {
                        newCubes3d[cube] = true;
                    }
                    else if (!cubes3d.ContainsKey(cube) && neighbors == 3)
                    {
                        newCubes3d[cube] = true;
                    }
                }

                cubes3d = newCubes3d;
            }
            
            return cubes3d.Count.ToString();

            static IEnumerable<(int x, int y, int z)> Deltas()
            {
                var deltas = new []{-1, 0, 1};
                foreach (var x in deltas)
                {
                    foreach (var y in deltas)
                    {
                        foreach (var z in deltas)
                        {
                            if (x == 0 && y == 0 && z == 0)
                            {
                                continue;
                            }

                            yield return (x, y, z);
                        }
                    }
                }
            }
        }

        public override string Solve_2()
        {
            for (var i = 0; i < 6; i++)
            {
                var cubesToCheck = new List<(int x, int y, int z, int w)>();
                foreach (var cube in cubes4d.Keys)
                {
                    cubesToCheck.AddRange(Deltas().Select(d => (cube.x + d.x, cube.y + d.y, cube.z + d.z, cube.w + d.w)));
                }

                var newCubes4d = new Dictionary<(int x, int y, int z, int w), bool>();
                foreach (var cube in cubesToCheck)
                {
                    var neighbors = Deltas().Count(d => cubes4d.ContainsKey((cube.x + d.x, cube.y + d.y, cube.z + d.z, cube.w + d.w)));

                    if (cubes4d.ContainsKey(cube) && (neighbors == 2 || neighbors == 3))
                    {
                        newCubes4d[cube] = true;
                    }
                    else if (!cubes4d.ContainsKey(cube) && neighbors == 3)
                    {
                        newCubes4d[cube] = true;
                    }
                }

                cubes4d = newCubes4d;
            }
            
            return cubes4d.Count.ToString();

            static IEnumerable<(int x, int y, int z, int w)> Deltas()
            {
                var deltas = new []{-1, 0, 1};
                foreach (var x in deltas)
                {
                    foreach (var y in deltas)
                    {
                        foreach (var z in deltas)
                        {
                            foreach (var w in deltas)
                            {
                                if (x == 0 && y == 0 && z == 0 && w == 0)
                                {
                                    continue;
                                }
                                
                                yield return (x, y, z, w);
                            }
                        }
                    }
                }
            }
        }
    }
}