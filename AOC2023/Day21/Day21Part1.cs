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
    public class Day21Part1
    {
        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day21";
        private List<List<char>> _data = new();

        public long Solve(Stopwatch watch)
        {
            long sum = 0;

            int maxSteps = _useTestData ? 6 : 64;
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
            Queue<(int x, int y, int steps)> queue = new();
            HashSet<(int x, int y, int steps)> seen = new();

            queue.Enqueue((x, y, 0));
            seen.Add((x, y, 0));

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
                    if (seen.Add((item.x, item.y, current.steps + 1)))
                    {
                        queue.Enqueue((item.x, item.y, current.steps + 1));
                    }
                }
            }

            Console.WriteLine(visitCount);
        }

        private List<(int x, int y)> GetNeighbours(int x, int y)
        {
            List<(int x, int y)> dirs = new()
            {
                (1, 0),
                (-1, 0),
                (0, 1),
                (0, -1)
            }, validDirs = new();

            foreach (var item in dirs)
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