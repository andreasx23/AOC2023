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
        private static readonly bool _useTestData = false;
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

            sum = Bfs(watch, 0, startY, _data.Count - 1, endY);

            //Print();

            return sum;
        }

        private void Print()
        {
            foreach (var item in _data)
            {
                Console.WriteLine(string.Join("", item));
            }
        }

        private int ManhattenDistance(int x, int y, int targetX, int targetY)
        {
            return (Math.Abs(x - targetX) + Math.Abs(y - targetY));
        }

        // 6242 to low
        // 7645 Not right answer
        private int Bfs(Stopwatch watch, int x, int y, int goalX, int goalY)
        {
            PriorityQueue<(int x, int y, int steps, HashSet<(int x, int y)> seen), int> queue = new();
            queue.Enqueue((x, y, 0, new HashSet<(int x, int y)>() { (x, y) }), ManhattenDistance(x, y, goalX, goalY));

            var steps = int.MinValue;
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current.x == goalX && current.y == goalY)
                {
                    if (current.steps > steps)
                    {
                        Console.WriteLine($"[{watch.Elapsed}] {current} -- {queue.Count}");
                        steps = current.steps;
                    }

                    continue;
                }

                foreach (var item in _dirs)
                {
                    var dx = item.x + current.x;
                    var dy = item.y + current.y;
                    var tile = (dx, dy);
                    if (dx < 0
                        || dx >= _data.Count
                        || dy < 0
                        || dy >= _data[x].Count
                        || _data[dx][dy] == "#"
                        || current.seen.Contains(tile))
                    {
                        continue;
                    }

                    //var manhatten = ManhattenDistance(tile.dx, tile.dy, goalX, goalY);
                    var manhatten = ManhattenDistance(current.x, current.y, tile.dx, tile.dy);
                    var newSeen = new HashSet<(int x, int y)>(current.seen) { tile };
                    queue.Enqueue((tile.dx, tile.dy, current.steps + 1, newSeen), manhatten);
                }
            }

            return steps;
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
            _data = lines.Select(row => row.Select(c => c.ToString()).ToList()).ToList();
        }
    }
}