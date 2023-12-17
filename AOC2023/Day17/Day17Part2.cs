using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day17
{
    public class Day17Part2
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
        private List<List<int>> _data = new();

        public long Solve(Stopwatch watch)
        {
            var sum = 0L;

            sum = Bfs(0, 0);

            return sum;
        }

        private int Bfs(int x, int y)
        {
            PriorityQueue<(int x, int y, Direction direction, int directionCount, int sum), int> queue = new();
            HashSet<(int x, int y, Direction direction, int directionCount)> seen = new();

            var sumDown = 0;
            queue.Enqueue((x, y, Direction.DOWN, 1, sumDown), sumDown);
            seen.Add((x, y, Direction.DOWN, 1));

            var sumRight = 0;
            queue.Enqueue((x, y, Direction.RIGHT, 1, sumRight), sumRight);
            seen.Add((x, y, Direction.RIGHT, 1));

            List<int> sums = new();
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current.x == _data.Count - 1 && current.y == _data.First().Count - 1)
                {
                    if (current.directionCount >= 4)
                    {
                        sums.Add(current.sum);
                    }

                    continue;
                }

                var neighbours = GetNeighbours(current.x, current.y, current.direction, current.directionCount);
                foreach (var item in neighbours)
                {
                    if (seen.Add((item.x, item.y, item.direction, item.directionCount)))
                    {
                        var sum = _data[item.x][item.y] + current.sum;
                        queue.Enqueue((item.x, item.y, item.direction, item.directionCount, sum), sum);
                    }
                }
            }

            Console.WriteLine(string.Join(", ", sums.Distinct()));

            // Possible answers: 1144, 1146, 1147, 1148, 1149, 1151, 1155, 1156, 1157, 1159, 1160, 1165
            // Right answer: 1149

            return sums.Min();
        }

        private List<(int x, int y, Direction direction, int directionCount)> GetNeighbours(int x, int y, Direction direction, int directionCount)
        {
            var dirs = GetDirs(direction, directionCount);

            List<(int x, int y, Direction direction, int directionCount)> validDirs = new();
            foreach (var item in dirs)
            {
                var dx = item.x + x;
                var dy = item.y + y;
                if (dx < 0 || dx >= _data.Count || dy < 0 || dy >= _data.First().Count)
                {
                    continue;
                }

                validDirs.Add((dx, dy, item.direction, item.directionCount));
            }

            return validDirs;
        }

        private List<(int x, int y, Direction direction, int directionCount)> GetDirs(Direction direction, int directionCount)
        {
            List<(int x, int y, Direction direction, int directionCount)> dirs = new();
            switch (direction)
            {
                case Direction.UP:
                    if (directionCount + 1 <= 4)
                    {
                        dirs.Add((-1, 0, Direction.UP, directionCount + 1));
                    }
                    else
                    {
                        if (directionCount + 1 <= 10)
                        {
                            dirs.Add((-1, 0, Direction.UP, directionCount + 1));
                        }
                        dirs.Add((0, -1, Direction.LEFT, 1));
                        dirs.Add((0, 1, Direction.RIGHT, 1));
                    }
                    break;
                case Direction.DOWN:
                    if (directionCount + 1 <= 4)
                    {
                        dirs.Add((1, 0, Direction.DOWN, directionCount + 1));
                    }
                    else
                    {
                        if (directionCount + 1 <= 10)
                        {
                            dirs.Add((1, 0, Direction.DOWN, directionCount + 1));
                        }
                        dirs.Add((0, -1, Direction.LEFT, 1));
                        dirs.Add((0, 1, Direction.RIGHT, 1));
                    }
                    break;
                case Direction.LEFT:
                    if (directionCount + 1 <= 4)
                    {
                        dirs.Add((0, -1, Direction.LEFT, directionCount + 1));
                    }
                    else
                    {
                        if (directionCount + 1 <= 10)
                        {
                            dirs.Add((0, -1, Direction.LEFT, directionCount + 1));
                        }
                        dirs.Add((1, 0, Direction.DOWN, 1));
                        dirs.Add((-1, 0, Direction.UP, 1));
                    }
                    break;
                case Direction.RIGHT:
                    if (directionCount + 1 <= 4)
                    {
                        dirs.Add((0, 1, Direction.RIGHT, directionCount + 1));
                    }
                    else
                    {
                        if (directionCount + 1 <= 10)
                        {
                            dirs.Add((0, 1, Direction.RIGHT, directionCount + 1));
                        }
                        dirs.Add((1, 0, Direction.DOWN, 1));
                        dirs.Add((-1, 0, Direction.UP, 1));
                    }
                    break;
            }

            return dirs;
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
            var lines = File.ReadAllLines(@$"{_className}\{(_useTestData ? "Test2" : "Data")}.txt");
            foreach (var item in lines)
            {
                var numbers = item.Select(x => int.Parse(x.ToString())).ToList();
                _data.Add(numbers);
            }
        }
    }
}