using Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2017.Day11
{
    public class Day11Part1
    {
        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day11";
        private List<string> _data = new();
       
        public long Solve(Stopwatch watch)
        {
            var sum = 0L;

            int x = 0;
            int y = 0;
            foreach (var item in _data)
            {
                var dir = DirectionsHelper.HexagonDirections[item];
                x += dir.x;
                y += dir.y;
            }

            sum = Bfs(0, 0, x, y);

            return sum;
        }

        private long Bfs(int x, int y, int targetX, int targetY)
        {
            Queue<(int x, int y, int steps)> queue = new();
            HashSet<(int x, int y)> seen = new();

            queue.Enqueue((x, y, 0));
            seen.Add((x, y));

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current.x == targetX && current.y == targetY)
                {
                    return current.steps;
                }

                foreach (var item in GetValidDirs(current.x, current.y))
                {
                    if (seen.Add(item))
                    {
                        queue.Enqueue((item.x, item.y, current.steps + 1));
                    }
                }
            }

            return -1;
        }

        private List<(int x, int y)> GetValidDirs(int x, int y)
        {
            List<(int x, int y)> dirs = new();
            foreach (var item in DirectionsHelper.HexagonDirections.Values)
            {
                var dx = item.x + x;
                var dy = item.y + y;
                dirs.Add((dx, dy));
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

        private void ReadData()
        {
            var lines = File.ReadAllLines(@$"{_className}\{(_useTestData ? "Test2" : "Data")}.txt");
            _data = lines.First().Split(',').ToList();
        }
    }
}