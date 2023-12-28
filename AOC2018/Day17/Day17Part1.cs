using Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AOC2018.Day17
{
    public class Day17Part1
    {
        enum Direction
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }

        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day17";
        private HashSet<(int x, int y)> _data = new();

        // https://adventofcode.com/2018/day/17
        public long Solve(Stopwatch watch)
        {
            var sum = 0L;

            (int x, int y) spring = (0, 500);
            var targetX = _data.Max(data => data.x) + 1;

            sum = Bfs(spring.x, spring.y, targetX);

            return sum;
        }

        private void Print(int targetX, HashSet<(int x, int y)> seen, int lastX, int lastY)
        {
            var maxY = Math.Max(_data.Max(x => x.y) + 1, seen.Max(x => x.y));
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
                grid[item.x][item.y] = '#';
            }

            foreach (var item in seen)
            {
                grid[item.x][item.y] = 'W';
            }

            grid[lastX][lastY] = ' ';

            if (_useTestData)
            {
                foreach (var item in grid.Take(200))
                {
                    Console.WriteLine($"{string.Join("", item.Skip(494))}.");
                }
            }
            else
            {
                foreach (var item in grid.Take(120))
                {
                    Console.WriteLine(string.Join("", item.Skip(475)));
                }
            }

            Console.WriteLine();
        }

        private long Bfs(int x, int y, int targetX)
        {
            PriorityQueue<(int x, int y, int previousX, int previousY, bool addedPrevious, bool isPrevious), int> queue = new();
            HashSet<(int x, int y)> seen = new();

            queue.Enqueue((x, y, x, y, false, false), -x);

            var lastX = 0;
            var lastY = 0;
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current.x == targetX)
                {
                    Console.WriteLine("YES");
                    continue;
                }

                if (!seen.Add((current.x, current.y)))
                {
                    continue;
                }

                lastX = current.x;
                lastY = current.y;

                var downX = current.x + 1;
                if (!_data.Contains((downX, current.y)) && (!current.isPrevious || current.x == 0))
                {
                    queue.Enqueue((downX, current.y, current.x, current.y, false, false), -downX);
                }
                else
                {
                    var leftY = current.y - 1;
                    while (!_data.Contains((current.x, leftY)) && (seen.Contains((current.x + 1, leftY)) || _data.Contains((current.x + 1, leftY))))
                    {
                        seen.Add((current.x, leftY));
                        leftY--;
                    }

                    var rightY = current.y + 1;
                    while (!_data.Contains((current.x, rightY)) && (seen.Contains((current.x + 1, rightY)) || _data.Contains((current.x + 1, rightY))))
                    {
                        seen.Add((current.x, rightY));
                        rightY++;
                    }

                    bool addLeft = false;
                    if (!_data.Contains((current.x + 1, leftY)) && !seen.Contains((current.x + 1, leftY)))
                    {
                        addLeft = true;
                    }

                    bool addRight = false;
                    if (!_data.Contains((current.x + 1, rightY)) && !seen.Contains((current.x + 1, rightY)))
                    {
                        addRight = true;
                    }

                    if ((leftY == 515 || rightY == 515))
                    {

                    }

                    if (addLeft || addRight)
                    {
                        queue.Clear();

                        if (addLeft)
                        {
                            queue.Enqueue((current.x, leftY, current.previousX, current.previousY, true, false), -current.x * 2);
                        }

                        if (addRight)
                        {
                            queue.Enqueue((current.x, rightY, current.previousX, current.previousY, true, false), -current.x * 2);
                        }
                    }
                    else
                    {
                        if (!current.addedPrevious)
                        {
                            queue.Enqueue((current.previousX, current.previousY, current.previousX - 1, current.previousY, false, true), current.previousX);
                            seen.Remove((current.previousX, current.previousY));
                        }
                    }
                }
            }

            var temp = seen.OrderByDescending(x => x.x).ToList();

            Print(targetX, seen, lastX, lastY);



            Console.WriteLine(lastX + " " + lastY);

            return seen.Count - 1;
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
                                _data.Add((y, x));
                            }
                        }
                        break;
                    case "y":
                        {
                            var y = int.Parse(first.Last());
                            var xs = second.Last().Split(new string[] { ".." }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                            for (int x = xs.First(); x <= xs.Last(); x++)
                            {
                                _data.Add((y, x));
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