using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AOC2023.Day23
{
    public class Day23Part2
    {
        private static readonly bool _useTestData = true;
        private static readonly string _className = "Day23";
        private List<List<string>> _data = new();
        private List<(int x, int y)> _dirs = new()
        {
            (0, 1),
            (0, -1),
            (1, 0),
            (-1, 0),
        };

        public long Solve(Stopwatch watch)
        {
            long sum = 0;

            int startY = 0;
            int endY = 0;
            for (int i = 0; i < _data.First().Count; i++)
            {
                if (_data[0][i] == ".")
                {
                    startY = i;
                }

                if (_data[_data.Count - 1][i] == ".")
                {
                    endY = i;
                }
            }

            sum = Bfs(0, startY, _data.Count - 1, endY);

            Print();

            return sum;
        }

        private void Print()
        {
            foreach (var item in _data)
            {
                Console.WriteLine(string.Join("", item));
            }
        }

        private int Bfs(int x, int y, int goalX, int goalY)
        {
            Queue<(int x, int y, int steps)> queue = new();
            Dictionary<(int x, int y), int> seen = new Dictionary<(int x, int y), int>();

            queue.Enqueue((x, y, 0));
            seen.Add((x, y), 0);

            List<int> steps = new();
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current.x == goalX && current.y == goalY)
                {
                    Console.WriteLine(current);
                    steps.Add(current.steps);
                    continue;
                }

                var neighbors = GetNeighbours(current.x, current.y);
                foreach (var item in neighbors)
                {
                    if (seen.ContainsKey(item))
                    {
                        var seenSteps = seen[item];
                        if (current.steps - 1 > seenSteps)
                        {
                            seen[item] = current.steps + 1;
                            queue.Enqueue((item.x, item.y, current.steps + 1));
                        }
                    }
                    else
                    {
                        seen[item] = current.steps + 1;
                        queue.Enqueue((item.x, item.y, current.steps + 1));
                    }
                }
            }

            return steps.Max();
        }

        private List<(int x, int y)> GetNeighbours(int x, int y)
        {
            List<(int x, int y)> validDirs = new();
            foreach (var item in _dirs)
            {
                var dx = item.x + x;
                var dy = item.y + y;
                if (dx < 0 || dx >= _data.Count || dy < 0 || dy >= _data[x].Count || _data[dx][dy] == "#")
                {
                    continue;
                }

                validDirs.Add((dx, dy));
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
            var lines = File.ReadAllLines(@$"{_className}\{(_useTestData ? "Test2" : "Data")}.txt");
            _data = lines.Select(row => row.Select(c => c.ToString()).ToList()).ToList();
        }
    }
}