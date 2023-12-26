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
        struct Node
        {
            public int X;
            public int Y;
            public int Distance;
        }

        class Grid
        {
            char[,] cells;
            int width;
            int height;
            List<Node> nodes;
            private List<List<string>> _data;

            public Grid(List<List<string>> grid)
            {
                _data = grid;
                this.cells = new char[grid.Count, grid.First().Count];
                for (int i = 0; i < grid.Count; i++)
                {
                    for (int j = 0; j < grid[i].Count; j++)
                    {
                        cells[i, j] = Convert.ToChar(grid[i][j]);
                    }
                }

                this.width = cells.GetLength(0);
                this.height = cells.GetLength(1);
                this.nodes = new List<Node>();
            }

            public int LongestPath()
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (cells[x, y] == '.')
                        {
                            nodes.Add(new Node { X = x, Y = y, Distance = 0 });
                        }
                    }
                }

                int maxDistance = 0;
                foreach (Node node in nodes)
                {
                    int distance = BFS(node, maxDistance);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                    }
                }

                return maxDistance;
            }

            private int BFS(Node start, int maxDistance)
            {
                Queue<Node> queue = new Queue<Node>();
                queue.Enqueue(start);

                while (queue.Count > 0)
                {
                    Node node = queue.Dequeue();
                    int distance = node.Distance;

                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                    }

                    int x = node.X;
                    int y = node.Y;

                    foreach (var item in GetNeighbours(node.X, node.Y))
                    {
                        var dx = item.x + node.X;
                        var dy = item.y + node.Y;
                        if (!nodes.Any(n => n.X == dx && n.Y == dy))
                        {
                            queue.Enqueue(new Node { X = dx, Y = dy, Distance = distance + 1 });
                        }
                    }

                    //if (x - 1 >= 0 && cells[x - 1, y] != '#' && !nodes.Any(n => n.X == x - 1 && n.Y == y))
                    //{
                    //    queue.Enqueue(new Node { X = x - 1, Y = y, Distance = distance + 1 });
                    //}
                    //if (y - 1 >= 0 && cells[x, y - 1] != '#' && !nodes.Any(n => n.X == x && n.Y == y - 1))
                    //{
                    //    queue.Enqueue(new Node { X = x, Y = y - 1, Distance = distance + 1 });
                    //}
                    //if (x + 1 < width && cells[x + 1, y] != '#' && !nodes.Any(n => n.X == x + 1 && n.Y == y))
                    //{
                    //    queue.Enqueue(new Node { X = x + 1, Y = y, Distance = distance + 1 });
                    //}
                    //if (y + 1 < height && cells[x, y + 1] != '#' && !nodes.Any(n => n.X == x && n.Y == y + 1))
                    //{
                    //    queue.Enqueue(new Node { X = x, Y = y + 1, Distance = distance + 1 });
                    //}
                }

                return maxDistance;
            }

            private List<(int x, int y)> GetNeighbours(int x, int y)
            {
                List<(int x, int y)> _dirs = new()
                {
                    (0, 1),
                    (0, -1),
                    (1, 0),
                    (-1, 0),
                };

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
        }


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

            var grid = new Grid(_data);

            var t = grid.LongestPath();
            Console.WriteLine(t);
            return 0;

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
            Dictionary<(int x, int y), (int steps, int count)> seen = new();

            queue.Enqueue((x, y, 0));
            seen.Add((x, y), (0, 1));

            bool first = true;
            List<int> steps = new();
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current.x == goalX && current.y == goalY)
                {
                    if (first)
                    {
                        foreach (var item in seen)
                        {
                            _data[item.Key.x][item.Key.y] = item.Value.steps.ToString();
                        }
                        //first = false;
                    }

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
                        //_data[item.x][item.y] = "+";
                        if (current.steps > seenSteps.steps && seenSteps.count <= neighbors.Count)
                        {
                            //_data[item.x][item.y] = "@";
                            seen[item] = (current.steps + 1, seenSteps.count + 1);
                            queue.Enqueue((item.x, item.y, current.steps + 1));
                        }
                        //else if (seenSteps.count < neighbors.Count)
                        //{
                        //    seen[item] = (current.steps + 1, seenSteps.count + 1);
                        //    queue.Enqueue((item.x, item.y, current.steps + 1));
                        //}
                    }
                    else
                    {
                        //_data[item.x][item.y] = "X";
                        seen[item] = (current.steps + 1, 1);
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