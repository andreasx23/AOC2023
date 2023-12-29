using Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AOC2018.Day17
{
    public class Day17Part1
    {
        enum Element
        {
            SAND = '.',
            WALL = '#',
            STILL_WATER = '~',
            RUNNING_WATER = '|'
        }

        private static readonly bool _useTestData = true;
        private static readonly string _className = "Day17";
        private Dictionary<(int x, int y), Element> _data = new();
        //private HashSet<(int x, int y)> _data = new();

        // https://adventofcode.com/2018/day/17
        public long Solve(Stopwatch watch)
        {
            var sum = 0L;

            (int x, int y) spring = (0, 500);
            var targetX = _data.Max(data => data.Key.x) + 1;

            HashSet<(int x, int y)> seen = new();
            seen.Add((0, 0));

            Fall(spring.x, spring.y);
            Bfs(spring.x, spring.y);

            Print(targetX, seen);

            return sum;
        }

        private void Print(int targetX, HashSet<(int x, int y)> seen)
        {
            var maxY = Math.Max(_data.Max(x => x.Key.y), seen.Max(x => x.y)) + 1;
            char[][] grid = new char[targetX][];
            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = new char[maxY];
                for (int j = 0; j < grid[i].Length; j++)
                {
                    grid[i][j] = '.';
                }
            }

            foreach (var item in _data)
            {
                grid[item.Key.x][item.Key.y] = (char)item.Value;
            }

            foreach (var item in seen.Where(temp => temp.x >= 0 && temp.y >= 0))
            {
                grid[item.x][item.y] = 'W';
            }

            if (_useTestData)
            {
                foreach (var item in grid.Take(200))
                {
                    Console.WriteLine($"{string.Join("", item.Skip(494))}.");
                }
            }
            else
            {
                //foreach (var item in grid.Take(120))
                //{
                //    Console.WriteLine(string.Join("", item.Skip(475)));
                //}

                foreach (var item in grid)
                {
                    Console.WriteLine(string.Join("", item.Skip(400)));
                }
            }

            Console.WriteLine();
        }

        private void Fall(int x, int y)
        {
            if (!_data.ContainsKey((x, y)))
            {
                _data[(x, y)] = Element.RUNNING_WATER;
            }

            var down = x + 1;
            while (!_data.ContainsKey((down, y)))
            {
                _data[(down, y)] = Element.RUNNING_WATER;
                down++;
            }
        }

        private void SpreadWater(int x, int y)
        {
            _data[(x, y)] = Element.STILL_WATER;

            var left = y - 1;
            while (!_data.TryGetValue((x, left), out var leftElement) || leftElement != Element.WALL)
            {
                _data[(x, left)] = Element.STILL_WATER;
                left--;
            }

            var right = y + 1;
            while (!_data.TryGetValue((x, right), out var rightElement) || rightElement != Element.WALL)
            {
                _data[(x, right)] = Element.STILL_WATER;
                right++;
            }
        }

        private void Bfs(int x, int y)
        {
            if (_data[(x, y)] != Element.RUNNING_WATER)
            {
                throw new Exception();
            }

            Queue<(int x, int y, bool isUpAndDown, bool isEndOfLine)> queue = new();
            HashSet<(int x, int y)> seen = new();
            queue.Enqueue((x, y, true, false));
            seen.Add((x, y));

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current.isEndOfLine)
                {
                    if (current.isUpAndDown)
                    {
                        SpreadWater(current.x, current.y);
                    }
                    else
                    {
                        Fall(current.x, current.y);
                    }

                    continue;
                }

                var leftAndRight = GetLeftAndRight(current.x, current.y);
                var down = GetDown(current.x, current.y);
                if (leftAndRight.Count > 0)
                {
                    foreach (var item in leftAndRight)
                    {
                        if (seen.Add(item))
                        {
                            queue.Enqueue((item.x, item.y, false, false));
                        }
                    }
                }
                else if (down.Count > 0)
                {
                    foreach (var item in down)
                    {
                        if (seen.Add(item))
                        {
                            queue.Enqueue((item.x, item.y, true, false));
                        }
                    }
                }
                else
                {
                    queue.Enqueue((current.x, current.y, current.isUpAndDown, true));
                }
            }
        }

        private List<(int x, int y)> GetLeftAndRight(int x, int y)
        {
            var dirs = new List<(int x, int y)>(2)
            {
                (x, y + 1),
                (x, y - 1),
            };

            var validDirs = new List<(int x, int y)>(2);
            foreach (var item in dirs)
            {
                if (_data.TryGetValue(item, out var element) && element == Element.RUNNING_WATER)
                {
                    validDirs.Add(item);
                }
            }

            return validDirs;
        }

        private List<(int x, int y)> GetDown(int x, int y)
        {
            var dirs = new List<(int x, int y)>(1)
            {
                (x + 1, y),
            };

            var validDirs = new List<(int x, int y)>(1);
            foreach (var item in dirs)
            {
                if (_data.TryGetValue(item, out var element) && element == Element.RUNNING_WATER)
                {
                    validDirs.Add(item);
                }
            }

            return validDirs;
        }

        public void Result()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            ReadData();
            var result = Solve(stopwatch);
            Console.WriteLine($"Your answer: {result} -- Took: {stopwatch.Elapsed}");
        }

        private void ReadData()
        {
            var lines = File.ReadAllLines(@$"{_className}\{(_useTestData ? "Test" : "Data")}.txt");
            foreach (var item in lines)
            {
                var split = item.Split(',').Select(x => x.Trim());
                var first = split.First().Split('=');
                var second = split.Last().Split('=');
                switch (first.First())
                {
                    case "x":
                        {
                            var x = int.Parse(first.Last());
                            var ys = second.Last().Split(new string[] { ".." }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                            for (int y = ys.First(); y <= ys.Last(); y++)
                            {
                                if (!_data.ContainsKey((y, x)))
                                {
                                    _data.Add((y, x), Element.WALL);
                                }
                            }
                        }
                        break;
                    case "y":
                        {
                            var y = int.Parse(first.Last());
                            var xs = second.Last().Split(new string[] { ".." }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                            for (int x = xs.First(); x <= xs.Last(); x++)
                            {
                                if (!_data.ContainsKey((y, x)))
                                {
                                    _data.Add((y, x), Element.WALL);
                                }
                            }
                        }
                        break;
                    default:
                        throw new Exception();
                }

            }
        }
    }
}