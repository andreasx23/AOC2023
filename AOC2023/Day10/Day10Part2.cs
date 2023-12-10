using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day10
{
    public class Day10Part2
    {
        private static readonly bool _useTestData = true;
        private const string TEST_DATA_NAME = "Test5";
        private static readonly string _className = "Day10";
        private List<List<char>> _data = new();
        private const char ANIMAL_START = 'S';
        private const char OVERWRITE_CHAR = '#';
        private const char FLOODFILL_CHAR = ' ';
        private const char TILE_CHAR = '.';

        // 667 to high
        public long Solve()
        {
            var sum = 0L;

            HashSet<(int x, int y)> tiles = null;
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
                        var currentMax = 0;
                        foreach (var item in neighbours)
                        {
                            HashSet<(int x, int y)> seen = new() { (i, j) };
                            var bfs = Bfs(i, j, item.x, item.y, seen);
                            if (bfs >= currentMax)
                            {
                                currentMax = bfs;
                                tiles = new(seen);
                            }
                        }

                        isFound = true;
                        break;
                    }
                }

                if (isFound)
                {
                    break;
                }
            }

            var mapClone = _data.Select(x => x.ToList()).ToList();
            foreach (var item in tiles)
            {
                _data[item.x][item.y] = OVERWRITE_CHAR;
            }

            // Convert junk to '.'
            for (int i = 0; i < _data.Count; i++)
            {
                for (int j = 0; j < _data[i].Count; j++)
                {
                    if (_data[i][j] != OVERWRITE_CHAR)
                    {
                        _data[i][j] = TILE_CHAR;
                    }
                }
            }

            // Left right
            for (int i = 0; i < _data.Count; i++)
            {
                FloodFill(i, 0);
                FloodFill(i, _data[0].Count - 1);
            }

            // Top down
            for (int i = 0; i < _data[0].Count; i++)
            {
                FloodFill(0, i);
                FloodFill(_data.Count - 1, i);
            }

            // Reset map without edges
            for (int i = 0; i < _data.Count; i++)
            {
                for (int j = 0; j < _data[i].Count; j++)
                {
                    if (_data[i][j] != FLOODFILL_CHAR && _data[i][j] != TILE_CHAR)
                    {
                        _data[i][j] = mapClone[i][j];
                    }
                }
            }

            // Center
            for (int i = 0; i < _data.Count; i++)
            {
                for (int j = 0; j < _data[i].Count; j++)
                {
                    if (!tiles.Contains((i, j)))
                    {
                        // Found spot not a part of main loop

                    }
                }
            }

            for (int i = 0; i < _data.Count; i++)
            {
                Console.WriteLine(string.Join("", _data[i]));
                for (int j = 0; j < _data[i].Count; j++)
                {
                    if (_data[i][j] != OVERWRITE_CHAR && _data[i][j] == TILE_CHAR)
                    {
                        sum++;
                    }
                }
            }

            return sum;
        }

        private void FloodFill(int x, int y)
        {
            var current = _data[x][y];
            if (current == OVERWRITE_CHAR || current == FLOODFILL_CHAR)
            {
                return;
            }

            _data[x][y] = FLOODFILL_CHAR;

            foreach (var item in GetNeighbours(x, y))
            {
                FloodFill(item.x, item.y);
            }
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
                case TILE_CHAR: // . is ground; there is no pipe in this tile.
                    break;
                case 'S': // S is the starting position of the animal; there is a pipe on this tile, but your sketch doesn't show what shape the pipe has.
                    break;
                case OVERWRITE_CHAR: // Custom icon used to overwrite fields on the 2D grid
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
            var lines = File.ReadAllLines(@$"{_className}\{(_useTestData ? TEST_DATA_NAME : "Data")}.txt");
            _data = lines.Select(x => x.ToList()).ToList();
        }
    }
}