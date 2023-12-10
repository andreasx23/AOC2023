using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day10
{
    public class Day10Part1
    {
        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day10";
        private List<List<char>> _data = new();
        private const char ANIMAL_START = 'S';

        public long Solve()
        {
            var sum = 0L;

            for (int i = 0; i < _data.Count; i++)
            {
                bool isFound = false;
                var currentLine = _data[i];
                for (int j = 0; j < currentLine.Count; j++)
                {
                    var currentChar = currentLine[j];
                    if (currentChar == ANIMAL_START)
                    {
                        var neighbours = GetNeighbours(i, j);
                        foreach (var item in neighbours)
                        {
                            HashSet<(int x, int y)> seen = new HashSet<(int x, int y)>() { (i, j) };
                            sum = Math.Max(sum, Bfs(i, j, item.x, item.y, seen));
                        }

                        sum = sum / 2 + 1;

                        isFound = true;
                        break;
                    }
                }

                if (isFound)
                {
                    break;
                }
            }

            return sum;
        }

        private int Bfs(int startX, int startY, int x, int y, HashSet<(int x, int y)> seen)
        {
            Queue<(int x, int y, int distanceFromStart)> queue = new();
            queue.Enqueue((x, y, 0));
            seen.Add((x, y));

            int lastX = -1;
            int lastY = -1;
            int maxDistanceFromStart = 0;
            while (queue.Any())
            {
                var current = queue.Dequeue();

                lastX = current.x;
                lastY = current.y;
                maxDistanceFromStart = Math.Max(maxDistanceFromStart, current.distanceFromStart);

                foreach (var item in GetDirections(current.x, current.y))
                {
                    if (seen.Add((item.x, item.y)))
                    {
                        queue.Enqueue((item.x, item.y, current.distanceFromStart + 1));
                    }
                }
            }

            var closedLoop = GetNeighbours(lastX, lastY);
            if (closedLoop.Any(dirs => dirs.x == startX && dirs.y == startY))
            {
                return maxDistanceFromStart;
            }

            return -1;
        }

        private List<(int x, int y)> GetNeighbours(int x, int y)
        {
            List<(int x, int y)> directions = new List<(int x, int y)>()
            {
                (-1, 0),
                (1, 0),
                (0, -1),
                (0, 1)
            };

            return GetValidNeighbours(x, y, directions);
        }

        private List<(int x, int y)> GetDirections(int x, int y)
        {
            var current = _data[x][y];
            List<(int x, int y)> directions = new List<(int x, int y)>();
            switch (current)
            {
                case '|': // | is a vertical pipe connecting north and south.
                    directions.Add((-1, 0));
                    directions.Add((1, 0));
                    break;
                case '-': // - is a horizontal pipe connecting east and west.
                    directions.Add((0, 1));
                    directions.Add((0, -1));
                    break;
                case 'L': // L is a 90-degree bend connecting north and east.
                    directions.Add((-1, 0));
                    directions.Add((0, 1));
                    break;
                case 'J': // J is a 90-degree bend connecting north and west.
                    directions.Add((-1, 0));
                    directions.Add((0, -1));
                    break;
                case '7': // 7 is a 90-degree bend connecting south and west.
                    directions.Add((1, 0));
                    directions.Add((0, -1));
                    break;
                case 'F': // F is a 90-degree bend connecting south and east.
                    directions.Add((1, 0));
                    directions.Add((0, 1));
                    break;
                case '.': // . is ground; there is no pipe in this tile.
                    break;
                case 'S': // S is the starting position of the animal; there is a pipe on this tile, but your sketch doesn't show what shape the pipe has.
                    break;
                case '#': // Custom icon used to overwrite fields on the 2D grid
                    break;
                default:
                    throw new InvalidOperationException("Invalid");
            }

            return GetValidNeighbours(x, y, directions);
        }

        private List<(int x, int y)> GetValidNeighbours(int x, int y, List<(int x, int y)> directions)
        {
            List<(int x, int y)> validDirections = new List<(int x, int y)>();
            foreach (var item in directions)
            {
                var dx = item.x + x;
                var dy = item.y + y;
                if (dx < 0 || dx >= _data.Count || dy < 0 || dy >= _data.First().Count)
                {
                    continue;
                }

                validDirections.Add((dx, dy));
            }

            return validDirections;
        }

        public void Result()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            ReadData();
            var result = Solve();
            Console.WriteLine($"Your answer: {result} -- Took: {stopwatch.Elapsed}");
        }

        public void ReadData()
        {
            var lines = File.ReadAllLines(@$"{_className}\{(_useTestData ? "Test3" : "Data")}.txt");
            _data = lines.Select(x => x.ToList()).ToList();
        }
    }
}