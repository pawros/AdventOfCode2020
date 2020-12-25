using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCHelper;

namespace AdventOfCode2020
{
    public class Day24 : BaseDay
    {
        private enum Color
        {
            White,
            Black
        }
        
        private List<List<string>> instructions = new List<List<string>>();
        private Dictionary<(int x, int y, int z), Color> tiles = new Dictionary<(int, int, int), Color>();
        
        
        public Day24()
        {
            foreach (var line in File.ReadLines(InputFilePath))
            {
                var directions = new List<string>();
                for (var i = 0; i < line.Length; i++)
                {
                    switch (line[i])
                    {
                        case 'e':
                        case 'w':
                            directions.Add(line[i].ToString());
                            break;
                        case 's':
                        case 'n':
                            directions.Add($"{line[i]}{line[i+1]}");
                            i++;
                            break;
                    }
                }
                instructions.Add(directions);
            }
        }
        
        public override string Solve_1()
        {
            foreach (var instruction in instructions)
            {
                (int x, int y, int z) p = (0, 0, 0);
                foreach (var direction in instruction)
                {
                    switch (direction)
                    {
                        case "e":
                            p.x += 1;
                            p.y += -1;
                            p.z += 0;
                            break;
                        case "w":
                            p.x += -1;
                            p.y += 1;
                            p.z += 0;
                            break;
                        case "ne":
                            p.x += 1;
                            p.y += 0;
                            p.z += -1;
                            break;
                        case "nw":
                            p.x += 0;
                            p.y += 1;
                            p.z += -1;
                            break;
                        case "se":
                            p.x += 0;
                            p.y += -1;
                            p.z += 1;
                            break;
                        case "sw":
                            p.x += -1;
                            p.y += 0;
                            p.z += 1;
                            break;
                    }
                }

                if (tiles.ContainsKey(p))
                {
                    tiles[p] = tiles[p] == Color.White 
                        ? Color.Black 
                        : Color.White;
                }
                else
                {
                    tiles[p] = Color.Black;
                }
            }

            var blacks = tiles.Count(t => t.Value == Color.Black);
            return blacks.ToString();
        }

        public override string Solve_2()
        {
            for (var d = 1; d <= 100; d++)
            {
                var blackTiles = tiles.Where(t => t.Value == Color.Black).ToList();
            
                var adjToBlack = new List<(int x, int y, int z)>();
                foreach(var ((x, y, z), _) in blackTiles)
                {
                    adjToBlack.AddRange(GetAdjacent(x, y, z));
                }
                adjToBlack = adjToBlack.Distinct().ToList();

                var newTiles = new Dictionary<(int, int, int), Color>(tiles);


                foreach (var ((x, y, z), _) in blackTiles)
                {
                    var bn = GetAdjacent(x, y, z).Count(adj => tiles.ContainsKey(adj) && tiles[adj] == Color.Black);

                    if (bn == 0 || bn > 2)
                    {
                        newTiles[(x, y, z)] = Color.White;
                    }
                }
            
                foreach (var blackNeighbor in adjToBlack)
                {
                    if (tiles.ContainsKey(blackNeighbor) && tiles[blackNeighbor] == Color.Black)
                    {
                        continue;
                    }

                    var bn = GetAdjacent(blackNeighbor.x, blackNeighbor.y, blackNeighbor.z).Count(adj => tiles.ContainsKey(adj) && tiles[adj] == Color.Black);

                    if (bn == 2)
                    {
                        newTiles[(blackNeighbor.x, blackNeighbor.y, blackNeighbor.z)] = Color.Black;
                    }
                }

                tiles = newTiles;
            }
            
            return tiles.Count(t => t.Value == Color.Black).ToString();
        }

        private List<(int, int, int)> GetAdjacent(int x, int y, int z)
        {
            var adjacent = new List<(int x, int y, int z)>
            {
                (x + 1, y - 1, z),
                (x - 1, y + 1, z),
                (x + 1, y, z - 1),
                (x - 1, y, z + 1),
                (x, y + 1, z - 1),
                (x, y - 1, z + 1)
            };
            return adjacent;
        }
    }
}