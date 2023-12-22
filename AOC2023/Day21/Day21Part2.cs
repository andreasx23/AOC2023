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

            int maxSteps = _useTestData ? 10 : 26501365;
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
            HashSet<(int x, int y)> seen = new();

            queue.Enqueue((x, y, false, 0));
            seen.Add((x, y));

            long visitCount = 0;
            while (queue.Count > 0)
            {
                visitCount++;
                var current = queue.Dequeue();

                if (current.steps == maxSteps)
                {
                    _data[current.x][current.y] = 'O';
                    continue;
                }

                _data[current.x][current.y] = '.';

                foreach (var item in GetNeighbours(current.x, current.y))
                {
                    _data[item.x][item.y] = 'O';
                    queue.Enqueue((item.x, item.y, item.didGoThroughWall, current.steps + 1));
                }

                //var groups = queue.GroupBy(x => (x.x, x.y)).ToList();
                var groups = queue.GroupBy(x => (x.x, x.y, x.didGoThroughWall)).ToList();
                queue.Clear();
                foreach (var group in groups)
                {
                    var first = group.First();
                    if (first.didGoThroughWall)
                    {
                        //foreach (var tile in group)
                        //{
                        //    queue.Enqueue(tile);
                        //}
                    }
                    else
                    {
                        queue.Enqueue(first);
                    }
                }
            }

            Console.WriteLine(visitCount);
        }

        private List<(int x, int y, bool didGoThroughWall)> GetNeighbours(int x, int y)
        {
            List<(int x, int y)> dirs = new()
            {
                (1, 0),
                (-1, 0),
                (0, 1),
                (0, -1)
            };

            List<(int x, int y, bool didGoThroughWall)> validDirs = new();
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

                validDirs.Add((dx, dy, didGoThroughWall));
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

        public void ReadData()
        {
            var lines = File.ReadAllLines(@$"{_className}\{(_useTestData ? "Test" : "Data")}.txt");
            _data = lines.Select(x => x.ToList()).ToList();
        }
    }
}