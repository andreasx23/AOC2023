using Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        private static readonly bool _useTestData = true;
        private static readonly string _className = "Day17";
        private HashSet<(int x, int y)> _data = new();

        public long Solve(Stopwatch watch)
        {
            var sum = 0L;

            (int x, int y) spring = (0, 500);
            var targetX = _data.Max(data => data.x) + 1;

            var seen = new HashSet<(int x, int y, Direction direction)>();
            for (int i = 0; i < 5; i++)
            {
                seen.Clear();
                Dfs(spring.x, spring.y, targetX, seen, Direction.DOWN);
                foreach (var item in seen.Where(x => x.direction != Direction.DOWN))
                {
                    sum++;

                    if (!seen.Contains((item.x + 1, item.y, Direction.DOWN)) && !_data.Contains((item.x + 1, item.y)))
                    {
                        _data.Add((item.x, item.y));
                    }
                }

                //foreach (var item in seen.Where(x => x.direction == Direction.DOWN))
                //{
                //    _data.Remove((item.x, item.y));
                //}
            }

            char[][] grid = new char[targetX][];
            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = new char[600];
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

            foreach (var item in grid)
            {
                Console.WriteLine(string.Join("", item.Skip(450).Take(75)));
            }

            return sum;
        }

        private void Dfs(int x, int y, int targetX, HashSet<(int x, int y, Direction direction)> seen, Direction direction)
        {
            if (x == targetX || _data.Contains((x, y)) || !seen.Add((x, y, direction)))
            {
                return;
            }

            var downX = x + 1;
            if (!_data.Contains((downX, y)))
            {
                Dfs(downX, y, targetX, seen, Direction.DOWN);
                return;
            }

            var lefY = y - 1;
            if (!_data.Contains((x, lefY)))
            {
                Dfs(x, lefY, targetX, seen, Direction.LEFT);
            }

            var rightY = y + 1;
            if (!_data.Contains((x, rightY)))
            {
                Dfs(x, rightY, targetX, seen, Direction.RIGHT);
            }
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