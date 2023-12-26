using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day23
{
    public class Day23Part1
    {
        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day23";
        private List<List<char>> _data = new();
        private List<char> _slopes = new(4)
        {
            '^',
            '<',
            '>',
            'v'
        };
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
                if (_data[0][i] == '.')
                {
                    startY = i;
                }

                if (_data[_data.Count - 1][i] == '.')
                {
                    endY = i;
                }
            }

            sum = Bfs(0, startY, _data.Count - 1, endY);

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

        private int Bfs(int x, int y, int goalX, int goalY)
        {
            Queue<(int x, int y, int steps)> queue = new();
            Dictionary<(int x, int y), int> seen = new();

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

                if (_slopes.Contains(_data[current.x][current.y]))
                {
                    switch (_data[current.x][current.y])
                    {
                        case '>':
                            {
                                var right = _dirs[0];
                                var dx = right.x + current.x;
                                var dy = right.y + current.y;
                                if (seen.ContainsKey((dx, dy)))
                                {
                                    var seenSteps = seen[(dx, dy)];
                                    if (current.steps - 1 > seenSteps)
                                    {
                                        seen[(dx, dy)] = current.steps + 1;
                                        queue.Enqueue((dx, dy, current.steps + 1));
                                    }
                                }
                                else
                                {
                                    seen[(dx, dy)] = current.steps + 1;
                                    queue.Enqueue((dx, dy, current.steps + 1));
                                }
                            }
                            break;
                        case '<':
                            {
                                var left = _dirs[1];
                                var dx = left.x + current.x;
                                var dy = left.y + current.y;
                                if (seen.ContainsKey((dx, dy)))
                                {
                                    var seenSteps = seen[(dx, dy)];
                                    if (current.steps - 1 > seenSteps)
                                    {
                                        seen[(dx, dy)] = current.steps + 1;
                                        queue.Enqueue((dx, dy, current.steps + 1));
                                    }
                                }
                                else
                                {
                                    seen[(dx, dy)] = current.steps + 1;
                                    queue.Enqueue((dx, dy, current.steps + 1));
                                }
                            }
                            break;
                        case 'v':
                            {
                                var down = _dirs[2];
                                var dx = down.x + current.x;
                                var dy = down.y + current.y;
                                if (seen.ContainsKey((dx, dy)))
                                {
                                    var seenSteps = seen[(dx, dy)];
                                    if (current.steps - 1 > seenSteps)
                                    {
                                        seen[(dx, dy)] = current.steps + 1;
                                        queue.Enqueue((dx, dy, current.steps + 1));
                                    }
                                }
                                else
                                {
                                    seen[(dx, dy)] = current.steps + 1;
                                    queue.Enqueue((dx, dy, current.steps + 1));
                                }
                            }
                            break;
                        case '^':
                            {
                                var up = _dirs[3];
                                var dx = up.x + current.x;
                                var dy = up.y + current.y;
                                if (seen.ContainsKey((dx, dy)))
                                {
                                    var seenSteps = seen[(dx, dy)];
                                    if (current.steps - 1 > seenSteps)
                                    {
                                        seen[(dx, dy)] = current.steps + 1;
                                        queue.Enqueue((dx, dy, current.steps + 1));
                                    }
                                }
                                else
                                {
                                    seen[(dx, dy)] = current.steps + 1;
                                    queue.Enqueue((dx, dy, current.steps + 1));
                                }
                            }
                            break;
                    }
                }
                else
                {
                    foreach (var item in GetNeighbours(current.x, current.y))
                    {
                        if (seen.ContainsKey((item.x, item.y)))
                        {
                            var seenSteps = seen[(item.x, item.y)];
                            if (current.steps - 1 > seenSteps)
                            {
                                seen[(item.x, item.y)] = current.steps + 1;
                                queue.Enqueue((item.x, item.y, current.steps + 1));
                            }
                        }
                        else
                        {
                            seen[(item.x, item.y)] = current.steps + 1;
                            queue.Enqueue((item.x, item.y, current.steps + 1));
                        }
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
                if (dx < 0 || dx >= _data.Count || dy < 0 || dy >= _data[x].Count || _data[dx][dy] == '#')
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
            var lines = File.ReadAllLines(@$"{_className}\{(_useTestData ? "Test" : "Data")}.txt");
            _data = lines.Select(x => x.ToList()).ToList();
        }
    }
}