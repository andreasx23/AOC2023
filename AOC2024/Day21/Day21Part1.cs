﻿using System.Diagnostics;
using System.Text;

namespace AOC2024.Day21
{
    public class Day21Part1
    {
        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day21";
        private List<string> _data = new();

        private readonly char[][] _numericKeypad = [
        ['7', '8', '9'],
        ['4', '5', '6'],
        ['1', '2', '3'],
        [' ', '0', 'A'],
        ];

        private readonly char[][] _directionalKeypad = [
        [' ', '^', 'A'],
        ['<', 'v', '>'],
        ];

        private readonly List<(int x, int y, char direction)> _directions = new(4)
        {
            (0, 1, '>'),
            (0, -1, '<'),
            (1, 0, 'v'),
            (-1, 0, '^')
        };

        private readonly object _lockRobot3 = new();

        private long Solve(Stopwatch watch)
        {
            var sum = 0L;

            var targetNumericKeypad = GenerateTargetDict(_numericKeypad);
            var targetDirectionalKeypad = GenerateTargetDict(_directionalKeypad);

            foreach (var row in _data)
            {
                var newWatch = Stopwatch.StartNew();
                var robot1Results = Bfs(_numericKeypad.Length - 1, _numericKeypad.Last().Length - 1, row, _numericKeypad, targetNumericKeypad, newWatch);

                var robot2Results = new List<string>();
                foreach (var robot1 in robot1Results)
                {
                    var robot2 = Bfs(0, _directionalKeypad.First().Length - 1, robot1, _directionalKeypad, targetDirectionalKeypad, newWatch);
                    robot2Results.AddRange(robot2);
                }

                var robot3Result = int.MaxValue;
                Parallel.ForEach(robot2Results, robot2 =>
                {
                    var robot3 = Bfs(0, _directionalKeypad.First().Length - 1, robot2, _directionalKeypad, targetDirectionalKeypad, newWatch);
                    lock (_lockRobot3)
                    {
                        if (robot3.Any())
                        {
                            var minRobot3 = robot3.Min(x => x.Length);
                            if (robot3Result > minRobot3)
                            {
                                robot3Result = minRobot3;
                                Console.WriteLine($"Current low: {robot3Result} -- Running: {row}");
                            }
                        }
                    }
                });

                sum += robot3Result * int.Parse(row.Substring(0, 3));
                Console.WriteLine($"Current sum: {sum} -- Done: {row} -- Total elapsed: {watch.Elapsed} -- Lowest val: {robot3Result}");
            }

            return sum;
        }

        private Dictionary<char, (int x, int y)> GenerateTargetDict(char[][] grid)
        {
            Dictionary<char, (int x, int y)> targetDict = new();
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    targetDict.Add(grid[i][j], (i, j));
                }
            }

            return targetDict;
        }

        private List<string> Bfs(int x, int y, string target, char[][] grid, Dictionary<char, (int x, int y)> targetCoordinates, Stopwatch watch)
        {
            PriorityQueue<(int x, int y, int moves, HashSet<(int x, int y)> seen, int index, StringBuilder stringOfMoves), int> queue = new();
            var seen = new HashSet<(int x, int y)>()
            {
                (x, y)
            };
            queue.Enqueue((x, y, 0, seen, 0, new StringBuilder()), 0);

            int lowestMoveCount = int.MaxValue;
            int total = 0;
            List<StringBuilder> possibleMoves = new();
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (grid[current.x][current.y] == target[current.index])
                {
                    current.seen.Clear();
                    current.stringOfMoves.Append("A");

                    if (current.index == target.Length - 1 && target[current.index] == 'A')
                    {
                        if (lowestMoveCount > current.moves)
                        {
                            lowestMoveCount = current.moves;
                            total = 1;
                            possibleMoves.Clear();
                            possibleMoves.Add(current.stringOfMoves);
                        }
                        else if (lowestMoveCount == current.moves)
                        {
                            total++;
                            possibleMoves.Add(current.stringOfMoves);
                        }

                        if (watch.Elapsed.TotalMinutes >= 2)
                        {
                            break;
                        }

                        continue;
                    }

                    if (watch.Elapsed.TotalMinutes >= 2)
                    {
                        break;
                    }

                    current.index++;
                    if (target[current.index - 1] == target[current.index])
                    {
                        var targetCoordinate = targetCoordinates[target[current.index]];
                        var lowestDistanceToTarget = ManhattenDistance(current.x, current.y, targetCoordinate.x, targetCoordinate.y);
                        queue.Enqueue((current.x, current.y, current.moves + 1, new HashSet<(int x, int y)>(current.seen), current.index, current.stringOfMoves), lowestDistanceToTarget);
                    }
                }

                foreach (var item in PossibleDirections(current.x, current.y, grid))
                {
                    if (current.seen.Add((item.x, item.y)))
                    {
                        var targetCoordinate = targetCoordinates[target[current.index]];
                        var lowestDistanceToTarget = ManhattenDistance(item.x, item.y, targetCoordinate.x, targetCoordinate.y);
                        var nextStringOfMoves = new StringBuilder(current.stringOfMoves.ToString());
                        nextStringOfMoves.Append(item.direction);
                        queue.Enqueue((item.x, item.y, current.moves + 1, new HashSet<(int x, int y)>(current.seen), current.index, nextStringOfMoves), lowestDistanceToTarget);
                    }
                }
            }

            return possibleMoves.Select(x => x.ToString()).ToList();
        }

        private List<(int x, int y, char direction)> PossibleDirections(int currentX, int currentY, char[][] grid)
        {
            List<(int x, int y, char direction)> possibleMoves = new(4);
            foreach (var (x, y, direction) in _directions)
            {
                var dx = x + currentX;
                var dy = y + currentY;
                if (dx < 0
                    || dx >= grid.Length
                    || dy < 0
                    || dy >= grid.First().Length
                    || (grid[dx][dy] == ' '))
                {
                    continue;
                }

                possibleMoves.Add((dx, dy, direction));
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
            _data = lines.ToList();
        }
    }
}
