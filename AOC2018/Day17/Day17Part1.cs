﻿using Extensions;
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
            RUNNING_WATER = '|',
            SPRING = '+'
        }

        private static readonly bool _useTestData = true;
        private static readonly string _className = "Day17";
        private Dictionary<(int x, int y), Element> _data = new();

        // 30295 To low
        public long Solve(Stopwatch watch)
        {
            var sum = 0L;

            (int x, int y) spring = (0, 500);
            var targetX = _data.Max(data => data.Key.x) + 1;

            Fall(spring.x, spring.y, targetX);
            var isDone = false;
            while (!isDone)
            {
                isDone = Bfs(spring.x, spring.y, targetX);
            }

            for (int i = 0; i < 20; i++)
            {
                Bfs(spring.x, spring.y, targetX);
            }

            CleanupWronglyPlacedWater(spring);

            sum = _data.Sum(row =>
            {
                if (row.Value == Element.STILL_WATER || row.Value == Element.RUNNING_WATER)
                {
                    return 1;
                }

                return 0;
            });

            Print(targetX);

            return sum;
        }

        private void CleanupWronglyPlacedWater((int x, int y) spring)
        {
            // Place spring
            _data[spring] = Element.SPRING;

            // Fix wrongly placed water between running water
            var groups = _data.GroupBy(d => d.Key.x);
            foreach (var group in groups)
            {
                var runningWater = group.Where(d => d.Value == Element.RUNNING_WATER)
                                        .OrderBy(d => d.Key.y)
                                        .ToList();
                for (int i = 1; i < runningWater.Count; i++)
                {
                    var current = runningWater[i - 1];
                    var next = runningWater[i];
                    List<(int x, int y)> stillWaterIndexes = new();
                    for (int y = current.Key.y + 1; y < next.Key.y; y++)
                    {
                        if (!_data.TryGetValue((current.Key.x, y), out var element) || element != Element.STILL_WATER)
                        {
                            stillWaterIndexes.Clear();
                            break;
                        }

                        stillWaterIndexes.Add((current.Key.x, y));
                    }

                    if (stillWaterIndexes.Count > 0)
                    {
                        foreach (var item in stillWaterIndexes)
                        {
                            if (_data.TryGetValue((item.x - 1, item.y), out var element) && element == Element.RUNNING_WATER)
                            {
                                _data[item] = Element.RUNNING_WATER;
                            }
                            else
                            {
                                _data[item] = Element.SAND;
                            }
                        }
                    }
                }
            }

            // Fix wrongly placed running water at the last row
            var lastX = _data.Max(kv => kv.Key.x);
            var lastXGroup = groups.First(g => g.Key == lastX);
            foreach (var item in lastXGroup)
            {
                if (item.Value != Element.RUNNING_WATER)
                {
                    continue;
                }

                if (!_data.TryGetValue((item.Key.x - 1, item.Key.y), out var element) || element != Element.RUNNING_WATER)
                {
                    _data[(item.Key.x, item.Key.y)] = Element.SAND;
                }
            }
        }

        private void Print(int targetX)
        {
            var maxY = _data.Max(x => x.Key.y) + 1;
            char[][] grid = new char[targetX][];
            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = new char[maxY];
                for (int j = 0; j < grid[i].Length; j++)
                {
                    grid[i][j] = (char)Element.SAND;
                }
            }

            foreach (var item in _data)
            {
                grid[item.Key.x][item.Key.y] = (char)item.Value;
            }

            if (_useTestData)
            {
                foreach (var item in grid.Take(200))
                {
                    Console.WriteLine($"{string.Join("", item.Skip(494))}");
                }
            }
            else
            {
                foreach (var item in grid)
                {
                    Console.WriteLine(string.Join("", item.Skip(350)));
                }
            }
        }

        private bool Fall(int x, int y, int targetX)
        {
            bool forceSpreadWater = true;
            if (!_data.ContainsKey((x, y)))
            {
                forceSpreadWater = false;
                _data[(x, y)] = Element.RUNNING_WATER;
            }

            var down = x + 1;
            while (!_data.ContainsKey((down, y)) && down != targetX)
            {
                forceSpreadWater = false;
                _data[(down, y)] = Element.RUNNING_WATER;
                down++;
            }

            return forceSpreadWater;
        }

        private void SpreadWater(int x, int y)
        {
            _data[(x, y)] = Element.STILL_WATER;

            HashSet<(int x, int y)> templeft = new();
            var left = y - 1;
            bool hasLeftFoundDown = false;
            while (!_data.TryGetValue((x, left), out var leftElement) || leftElement != Element.WALL && leftElement != Element.RUNNING_WATER)
            {
                templeft.Add((x, left));

                if (!_data.ContainsKey((x + 1, left)))
                {
                    hasLeftFoundDown = true;
                    break;
                }

                left--;
            }

            foreach (var item in templeft)
            {
                _data[(item.x, item.y)] = hasLeftFoundDown ? Element.RUNNING_WATER : Element.STILL_WATER;
            }

            HashSet<(int x, int y)> tempRight = new();
            var right = y + 1;
            bool hasRightFoundDown = false;
            while (!_data.TryGetValue((x, right), out var rightElement) || rightElement != Element.WALL && rightElement != Element.RUNNING_WATER)
            {
                tempRight.Add((x, right));

                if (!_data.ContainsKey((x + 1, right)))
                {
                    hasRightFoundDown = true;
                    break;
                }

                right++;
            }

            foreach (var item in tempRight)
            {
                _data[(item.x, item.y)] = hasRightFoundDown ? Element.RUNNING_WATER : Element.STILL_WATER;
            }

            if (hasLeftFoundDown || hasRightFoundDown)
            {
                _data[(x, y)] = Element.RUNNING_WATER;
            }
        }

        private bool Bfs(int x, int y, int targetX)
        {
            if (_data[(x, y)] != Element.RUNNING_WATER)
            {
                throw new Exception();
            }

            Queue<(int x, int y, bool isUpAndDown, bool isEndOfLine)> queue = new();
            HashSet<(int x, int y)> seen = new();
            queue.Enqueue((x, y, true, false));
            seen.Add((x, y));

            List<int> lastXs = new();
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current.isEndOfLine)
                {
                    if (current.isUpAndDown)
                    {
                        var nextDownX = current.x + 1;
                        if (_data.TryGetValue((nextDownX, current.y), out var element) && element == Element.STILL_WATER || element == Element.RUNNING_WATER)
                        {
                            var nextLeftY = current.y - 1;
                            while (_data.TryGetValue((nextDownX, nextLeftY), out var leftElement) && leftElement == Element.STILL_WATER)
                            {
                                nextLeftY--;
                            }

                            var nextRightY = current.y + 1;
                            while (_data.TryGetValue((nextDownX, nextRightY), out var rightElement) && rightElement == Element.STILL_WATER)
                            {
                                nextRightY++;
                            }

                            var isLeftWall = _data[(nextDownX, nextLeftY)] == Element.WALL;
                            var isRigthWall = _data[(nextDownX, nextRightY)] == Element.WALL;

                            if (isLeftWall && isRigthWall)
                            {
                                SpreadWater(current.x, current.y);
                            }
                        }
                        else
                        {
                            SpreadWater(current.x, current.y);
                        }
                    }
                    else
                    {
                        lastXs.Add(current.x);

                        var forceSpreadWater = Fall(current.x, current.y, targetX);
                        if (forceSpreadWater)
                        {
                            SpreadWater(current.x, current.y);
                        }
                    }

                    continue;
                }

                var leftAndRight = GetLeftAndRight(current.x, current.y).Where(item => !seen.Contains(item)).ToList();
                var down = GetDown(current.x, current.y).Where(item => !seen.Contains(item)).ToList();
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

            return lastXs.Count > 0 && lastXs.All(x => x == targetX - 1);
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