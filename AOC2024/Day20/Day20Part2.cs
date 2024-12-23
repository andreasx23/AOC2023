using System.Diagnostics;

namespace AOC2024.Day20
{
    public class Day20Part2
    {
        private const int MAX_CHEATS = 2;

        private static readonly bool _useTestData = true;
        private static readonly string _className = "Day20";

        private char[][] _grid;

        private readonly List<(int x, int y)> _directions = new(4)
        {
            (0, 1),
            (0, -1),
            (1, 0),
            (-1, 0)
        };

        private long Solve(Stopwatch watch)
        {
            var sum = 0L;

            var start = FindStartOrTarget(true);
            var target = FindStartOrTarget(false);

            var bfs = Bfs(start.x, start.y, target.x, target.y).OrderByDescending(x => x.Value)
                                                               .ToDictionary(x => x.Key, x => x.Value);
            var max = bfs.First().Key;
            sum = bfs.Skip(1)
                     .Where(x => max - x.Key >= (!_useTestData ? 100 : 64))
                     .Sum(x => x.Value);

            return sum;
        }

        private (int x, int y) FindStartOrTarget(bool findStart)
        {
            for (int i = 0; i < _grid.Length; i++)
            {
                for (int j = 0; j < _grid[i].Length; j++)
                {
                    if ((findStart && _grid[i][j] == 'S') || (!findStart && _grid[i][j] == 'E'))
                    {
                        _grid[i][j] = '.';
                        return (i, j);
                    }
                }
            }

            throw new Exception("Impossible");
        }

        private Dictionary<int, int> Bfs(int x, int y, int targetX, int targetY)
        {
            PriorityQueue<(int x, int y, int steps, HashSet<(int x, int y)> visisted, bool hasCheated, int isCheating), int> queue = new();

            queue.Enqueue((x, y, 0, new HashSet<(int x, int y)>() { (x, y) }, false, 0), ManhattenDistance(x, y, targetX, targetY));

            Dictionary<int, int> counter = new();
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current.x == targetX && current.y == targetY)
                {
                    if (!counter.ContainsKey(current.steps))
                    {
                        counter[current.steps] = 0;
                    }

                    counter[current.steps] += 1;
                    continue;
                }

                if (!current.hasCheated)
                {
                    foreach (var item in PossibleDirections(current.x, current.y, _grid, true))
                    {
                        if (!current.visisted.Contains(item))
                        {
                            if (current.isCheating < MAX_CHEATS)
                            {
                                var distanceToTarget = ManhattenDistance(item.x, item.y, targetX, targetY);
                                var newVisted = new HashSet<(int x, int y)>(current.visisted) { item };
                                queue.Enqueue((item.x, item.y, current.steps + 1, newVisted, false, current.isCheating + 1), distanceToTarget);

                                if (_grid[item.x][item.y] == '.')
                                {
                                    var newVisted2 = new HashSet<(int x, int y)>(current.visisted) { item };
                                    queue.Enqueue((item.x, item.y, current.steps + 1, newVisted2, true, current.isCheating + 1), distanceToTarget);
                                }
                            }
                            else if (current.isCheating == MAX_CHEATS && _grid[item.x][item.y] == '.')
                            {
                                var distanceToTarget = ManhattenDistance(item.x, item.y, targetX, targetY);
                                var newVisted = new HashSet<(int x, int y)>(current.visisted) { item };
                                queue.Enqueue((item.x, item.y, current.steps + 1, newVisted, true, current.isCheating), distanceToTarget);
                            }
                        }
                    }
                }

                if (current.hasCheated && current.isCheating <= MAX_CHEATS)
                {
                    foreach (var item in PossibleDirections(current.x, current.y, _grid, false))
                    {
                        if (!current.visisted.Contains(item))
                        {
                            var distanceToTarget = ManhattenDistance(item.x, item.y, targetX, targetY);
                            var newVisted = new HashSet<(int x, int y)>(current.visisted) { item };
                            queue.Enqueue((item.x, item.y, current.steps + 1, newVisted, current.hasCheated, current.isCheating), distanceToTarget);
                        }
                    }
                }
            }

            return counter;
        }

        private List<(int x, int y)> PossibleDirections(int currentX, int currentY, char[][] grid, bool isCheating)
        {
            List<(int x, int y)> possibleMoves = new(4);
            foreach (var (x, y) in _directions)
            {
                var dx = x + currentX;
                var dy = y + currentY;
                if (dx < 0
                    || dx >= grid.Length
                    || dy < 0
                    || dy >= grid.First().Length)
                {
                    continue;
                }

                if (!isCheating && grid[dx][dy] == '#')
                {
                    continue;
                }

                possibleMoves.Add((dx, dy));
            }

            return possibleMoves;
        }

        private int ManhattenDistance(int x, int y, int targetX, int targetY)
        {
            return Math.Abs(x - targetX) + Math.Abs(y - targetY);
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
            var data = lines.ToArray();
            _grid = new char[data.Length][];
            for (int i = 0; i < data.Length; i++)
            {
                _grid[i] = new char[data[i].Length];
                for (int j = 0; j < data[i].Length; j++)
                {
                    _grid[i][j] = data[i][j];
                }
            }
        }
    }
}
