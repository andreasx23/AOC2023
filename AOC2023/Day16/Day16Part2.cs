using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day16
{
    public class Day16Part2
    {
        enum Direction
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }

        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day16";
        private List<List<char>> _data = new();
        private List<List<char>> _data2 = new();

        public long Solve(Stopwatch watch)
        {
            long sum = 0;

            for (int i = 0; i < _data.Count; i++)
            {
                sum = Math.Max(sum, HandleDfs(i, 0, Direction.RIGHT));
                sum = Math.Max(sum, HandleDfs(i, _data.First().Count - 1, Direction.LEFT));
            }

            for (int i = 0; i < _data.First().Count; i++)
            {
                sum = Math.Max(sum, HandleDfs(0, i, Direction.DOWN));
                sum = Math.Max(sum, HandleDfs(_data.Count - 1, i, Direction.UP));
            }

            return sum;
        }

        private long HandleDfs(int x, int y, Direction direction)
        {
            ReadData();
            Dfs(x, y, direction, new HashSet<(int x, int y, Direction direction)>());
            var lights = _data2.Sum(x => x.Count(c => c == '#'));
            return lights;
        }

        private void Dfs(int x, int y, Direction direction, HashSet<(int x, int y, Direction direction)> seen)
        {
            if (x < 0 || x >= _data.Count || y < 0 || y >= _data.First().Count || !seen.Add((x, y, direction)))
            {
                return;
            }

            var current = _data[x][y];
            _data2[x][y] = '#';

            switch (current)
            {
                case '.':
                    switch (direction)
                    {
                        case Direction.UP:
                            Dfs(x - 1, y, Direction.UP, seen);
                            break;
                        case Direction.DOWN:
                            Dfs(x + 1, y, Direction.DOWN, seen);
                            break;
                        case Direction.LEFT:
                            Dfs(x, y - 1, Direction.LEFT, seen);
                            break;
                        case Direction.RIGHT:
                            Dfs(x, y + 1, Direction.RIGHT, seen);
                            break;
                    }
                    break;
                case '|':
                    switch (direction)
                    {
                        case Direction.UP:
                            Dfs(x - 1, y, Direction.UP, seen);
                            break;
                        case Direction.DOWN:
                            Dfs(x + 1, y, Direction.DOWN, seen);
                            break;
                        case Direction.LEFT:
                            Dfs(x - 1, y, Direction.UP, seen);
                            Dfs(x + 1, y, Direction.DOWN, seen);
                            break;
                        case Direction.RIGHT:
                            Dfs(x - 1, y, Direction.UP, seen);
                            Dfs(x + 1, y, Direction.DOWN, seen);
                            break;
                    }
                    break;
                case '\\':
                    switch (direction)
                    {
                        case Direction.UP:
                            Dfs(x, y - 1, Direction.LEFT, seen);
                            break;
                        case Direction.DOWN:
                            Dfs(x, y + 1, Direction.RIGHT, seen);
                            break;
                        case Direction.LEFT:
                            Dfs(x - 1, y, Direction.UP, seen);
                            break;
                        case Direction.RIGHT:
                            Dfs(x + 1, y, Direction.DOWN, seen);
                            break;
                    }
                    break;
                case '/':
                    switch (direction)
                    {
                        case Direction.UP:
                            Dfs(x, y + 1, Direction.RIGHT, seen);
                            break;
                        case Direction.DOWN:
                            Dfs(x, y - 1, Direction.LEFT, seen);
                            break;
                        case Direction.LEFT:
                            Dfs(x + 1, y, Direction.DOWN, seen);
                            break;
                        case Direction.RIGHT:
                            Dfs(x - 1, y, Direction.UP, seen);
                            break;
                    }
                    break;
                case '-':
                    switch (direction)
                    {
                        case Direction.UP:
                            Dfs(x, y - 1, Direction.LEFT, seen);
                            Dfs(x, y + 1, Direction.RIGHT, seen);
                            break;
                        case Direction.DOWN:
                            Dfs(x, y - 1, Direction.LEFT, seen);
                            Dfs(x, y + 1, Direction.RIGHT, seen);
                            break;
                        case Direction.LEFT:
                            Dfs(x, y - 1, Direction.LEFT, seen);
                            break;
                        case Direction.RIGHT:
                            Dfs(x, y + 1, Direction.RIGHT, seen);
                            break;
                    }
                    break;
                default:
                    throw new InvalidOperationException("Invalid tile");
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
            _data = lines.Select(x => x.ToList()).ToList();
            _data2 = lines.Select(x => x.ToList()).ToList();
        }
    }
}