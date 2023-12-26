using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day21
{
    public class Day21Part2
    {
        private static readonly bool _useTestData = true;
        private static readonly string _className = "Day21";
        private List<List<char>> _data = new();

        public long Solve(Stopwatch watch)
        {
            long sum = 0;

            int maxSteps = _useTestData ? 50 : 26501365;
            for (int i = 0; i < _data.Count; i++)
            {
                bool isFound = false;
                for (int j = 0; j < _data[i].Count; j++)
                {
                    if (_data[i][j] == 'S')
                    {
                        _data[i][j] = '.';
                        Bfs(i, j, maxSteps);
                        isFound = true;
                        break;
                    }
                }

                if (isFound)
                {
                    break;
                }
            }

            //Print();

            sum = _data.Sum(row => row.Count(x => x == 'O'));

            return sum;
        }

        private void Print()
        {
            foreach (var item in _data)
            {
                Console.WriteLine(string.Join("", item));
            }
        }

        private void Bfs(int x, int y, int maxSteps)
        {
            Queue<(int x, int y, bool didGoThroughWall, int steps)> queue = new();
            HashSet<(int x, int y, bool didGoThroughWall, int steps)> seen = new();
            Dictionary<int, long> gardenPlots = new Dictionary<int, long>();

            for (int i = 0; i <= maxSteps; i++)
            {
                gardenPlots[i] = 0;
            }

            queue.Enqueue((x, y, false, 0));
            seen.Add((x, y, true, 0));

            long visitCount = 0;
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                visitCount++;

                if (visitCount % 100_000 == 0)
                {
                    Console.WriteLine($"[{visitCount}] {current}");
                }

                if (current.steps == maxSteps)
                {
                    _data[current.x][current.y] = 'O';
                    continue;
                }

                _data[current.x][current.y] = '.';

                var neighbours = GetNeighbours(current.x, current.y);
                foreach (var item in neighbours.validDirs)
                {
                    _data[item.x][item.y] = 'O';
                    if (seen.Add((item.x, item.y, item.didGoThroughWall, current.steps + 1)))
                    {
                        queue.Enqueue((item.x, item.y, item.didGoThroughWall, current.steps + 1));
                    }
                }

                gardenPlots[current.steps + 1] += neighbours.count;
            }

            Console.WriteLine(visitCount + " " + gardenPlots[maxSteps]);
        }

        private (List<(int x, int y, bool didGoThroughWall)> validDirs, long count) GetNeighbours(int x, int y)
        {
            List<(int x, int y)> dirs = new(4)
            {
                (1, 0),
                (-1, 0),
                (0, 1),
                (0, -1)
            };

            long count = 0;
            List<(int x, int y, bool didGoThroughWall)> validDirs = new(4);
            foreach (var item in dirs)
            {
                bool didGoThroughWall = false;
                var dx = item.x + x;
                var dy = item.y + y;

                if (dx < 0)
                {
                    dx = _data.Count - 1;
                    didGoThroughWall = true;
                }

                if (dx >= _data.Count)
                {
                    dx = 0;
                    didGoThroughWall = true;
                }

                if (dy < 0)
                {
                    dy = _data[x].Count - 1;
                    didGoThroughWall = true;
                }

                if (dy >= _data[x].Count)
                {
                    dy = 0;
                    didGoThroughWall = true;
                }

                if (_data[dx][dy] == '#')
                {
                    continue;
                }

                count++;

                validDirs.Add((dx, dy, didGoThroughWall));
            }

            return (validDirs, count);
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
        }
    }
}