using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day22
{
    public class Day22Part1
    {
        class Brick
        {
            public char Id { get; set; }
            public int LeftX { get; set; }
            public int LeftY { get; set; }
            public int LeftZ { get; set; }
            public int RightX { get; set; }
            public int RightY { get; set; }
            public int RightZ { get; set; }
        }

        private static readonly bool _useTestData = true;
        private static readonly string _className = "Day22";
        private List<Brick> _bricks = new();

        public long Solve(Stopwatch watch)
        {
            long sum = 0;

            // NOT SOLVED

            var grid = CreateGrid();

            while (true)
            {
                bool isDone = true;
                for (int z = grid[0][0].Count - 1; z > 0; z--)
                {
                    bool isValid = false;

                    for (int x = 0; x < grid.Count; x++)
                    {
                        for (int y = 0; y < grid[x].Count; y++)
                        {
                            isValid = IsFloorBelowUs(grid, x, y, z);

                            if (isValid)
                            {
                                break;
                            }
                        }

                        if (isValid)
                        {
                            break;
                        }
                    }

                    if (!isValid)
                    {
                        for (int x = 0; x < grid.Count; x++)
                        {
                            for (int y = 0; y < grid[x].Count; y++)
                            {
                                grid[x][y][z] = ' ';
                            }
                        }

                        isDone = false;
                        break;
                    }
                }

                if (isDone)
                {
                    break;
                }
            }

            //Print(grid);

            return sum;
        }

        private bool IsFloorBelowUs(List<List<List<char>>> grid, int x, int y, int z)
        {
            List<(int x, int y, int z)> dirs = new(9)
            {
                (0, 0, -1),

                (0, 1, -1),
                (0, -1, -1),
                (1, 0, -1),
                (-1, 0, -1),

                (-1, -1, -1),
                (-1, 1, -1),
                (1, -1, -1),
                (1, 1, -1),
            };

            foreach (var item in dirs)
            {
                var dx = item.x + x;
                var dy = item.y + y;
                var dz = item.z + z;

                if (dx < 0 || dx >= grid.Count || dy < 0 || dy >= grid[dx].Count || dz < 0 || dz >= grid[dx][dy].Count)
                {
                    continue;
                }

                if (grid[dx][dy][dz] != ' ')
                {
                    Console.WriteLine($"({dx}, {dy}, {dz}) {grid[dx][dy][dz]}");
                    return true;
                }
            }

            return false;
        }

        private List<List<List<char>>> CreateGrid()
        {
            var maxX = _bricks.Max(b => Math.Max(b.LeftX, b.RightX)) + 1;
            var maxY = _bricks.Max(b => Math.Max(b.LeftY, b.RightY)) + 1;
            var maxZ = _bricks.Max(b => Math.Max(b.LeftZ, b.RightZ)) + 1;

            List<List<List<char>>> grid = new List<List<List<char>>>();
            for (int i = 0; i < maxX; i++)
            {
                grid.Add(new List<List<char>>());
                for (int j = 0; j < maxY; j++)
                {
                    grid[i].Add(new List<char>());
                    for (int k = 0; k < maxZ; k++)
                    {
                        grid[i][j].Add(' ');
                    }
                }
            }

            foreach (var brick in _bricks)
            {
                for (int x = brick.LeftX; x <= brick.RightX; x++)
                {
                    for (int y = brick.LeftY; y <= brick.RightY; y++)
                    {
                        for (int z = brick.LeftZ; z <= brick.RightZ; z++)
                        {
                            grid[x][y][z] = brick.Id;
                        }
                    }
                }
            }

            return grid;
        }

        public void Result()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            ReadData();
            var result = Solve(stopwatch);
            Console.WriteLine($"Your answer: {result} -- Took: {stopwatch.Elapsed}");
        }

        public void ReadData()
        {
            var lines = File.ReadAllLines(@$"{_className}\{(_useTestData ? "Test" : "Data")}.txt");
            char id = 'A';
            foreach (var item in lines)
            {
                var split = item.Split('~');
                var left = split.First().Split(',').Select(int.Parse).ToList();
                var right = split.Last().Split(',').Select(int.Parse).ToList();
                _bricks.Add(new Brick()
                {
                    Id = id,
                    LeftX = left[0],
                    LeftY = left[1],
                    LeftZ = left[2],
                    RightX = right[0],
                    RightY = right[1],
                    RightZ = right[2],
                });

                id = (char)('A' + 1);
            }
        }
    }
}