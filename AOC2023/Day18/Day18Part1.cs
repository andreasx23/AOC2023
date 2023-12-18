using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day18
{
    public class Day18Part1
    {
        class Plan
        {
            public string Direction { get; set; }
            public int Number { get; set; }
            public string Code { get; set; }
        }

        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day18";
        private List<Plan> _data = new();

        public long Solve(Stopwatch watch)
        {
            long sum = 0;

            var grid = GenerateGrid();

            //Print(grid);

            for (int i = 0; i < grid.Count; i++)
            {
                bool found = false;
                for (int j = 0; j < grid[i].Count; j++)
                {
                    if (grid[i][j] == '#')
                    {
                        Bfs(i + 1, j + 1, grid);
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    break;
                }
            }

            sum = grid.Sum(row => row.Count(c => c == '#'));

            return sum;
        }

        private void Bfs(int x, int y, List<List<char>> grid)
        {
            Queue<(int x, int y)> queue = new();
            HashSet<(int x, int y)> seen = new();

            queue.Enqueue((x, y));
            seen.Add((x, y));

            while (queue.Any())
            {
                var current = queue.Dequeue();
                grid[current.x][current.y] = '#';

                foreach (var item in GetNeighbours(current.x, current.y, grid))
                {
                    if (seen.Add(item))
                    {
                        queue.Enqueue(item);
                    }
                }
            }
        }

        private List<(int x, int y)> GetNeighbours(int x, int y, List<List<char>> grid)
        {
            List<(int x, int y)> dirs = new()
            {
                (0, 1),
                (0, -1),
                (1, 0),
                (-1, 0),
            };

            List<(int x, int y)> validDirs = new();
            foreach (var item in dirs)
            {
                var dx = x + item.x;
                var dy = y + item.y;
                if (x < 0 || x >= grid.Count || y < 0 || y >= grid[x].Count || grid[dx][dy] == '#')
                {
                    continue;
                }

                validDirs.Add((dx, dy));
            }

            return validDirs;
        }

        private void Print(List<List<char>> grid)
        {
            foreach (var item in grid)
            {
                Console.WriteLine(string.Join("", item));
            }
        }

        private List<List<char>> GenerateGrid()
        {
            List<List<char>> grid = new();
            var rights = _data.Sum(c =>
            {
                return c.Direction == "R" ? c.Number : 0;
            }) * 2;

            var down = _data.Sum(c =>
            {
                return c.Direction == "D" ? c.Number : 0;
            }) * 2;

            for (int i = 0; i < rights; i++)
            {
                grid.Add(new List<char>(down));
                for (int j = 0; j < down; j++)
                {
                    grid[i].Add(' ');
                }
            }

            int indexI = grid.Count / 2;
            int indexJ = grid.First().Count / 2;
            foreach (var plan in _data)
            {
                for (int i = 0; i < plan.Number; i++)
                {
                    grid[indexI][indexJ] = '#';
                    switch (plan.Direction)
                    {
                        case "R":
                            indexJ++;
                            break;
                        case "L":
                            indexJ--;
                            break;
                        case "U":
                            indexI--;
                            break;
                        case "D":
                            indexI++;
                            break;
                        default:
                            throw new Exception();
                    }
                }
            }

            TruncateGrid(grid);

            return grid;
        }

        private static void TruncateGrid(List<List<char>> grid)
        {
            List<int> indexes = new List<int>();
            for (int i = 0; i < grid.Count; i++)
            {
                var current = grid[i];
                if (current.All(c => c == ' '))
                {
                    indexes.Add(i - indexes.Count);
                }
            }

            foreach (var item in indexes)
            {
                grid.RemoveAt(item);
            }

            indexes.Clear();
            for (int i = 0; i < grid.First().Count; i++)
            {
                bool found = true;
                for (int j = 0; j < grid.Count; j++)
                {
                    if (grid[j][i] == '#')
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    indexes.Add(i - indexes.Count);
                }
            }

            foreach (var item in indexes)
            {
                for (int i = 0; i < grid.Count; i++)
                {
                    grid[i].RemoveAt(item);
                }
            }
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
            foreach (var item in lines)
            {
                var split = item.Split(' ');
                var direction = split[0];
                var number = int.Parse(split[1]);
                var code = split[2][1..^1];
                _data.Add(new Plan
                {
                    Direction = direction,
                    Number = number,
                    Code = code
                });
            }
        }
    }
}