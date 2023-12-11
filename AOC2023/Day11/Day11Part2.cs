using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day11
{
    public class Day11Part2
    {
        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day11";
        private List<List<char>> _data = new();
        private const char GALAXY = '#';
        private const char EMPTY_SPACE = '.';

        public long Solve(Stopwatch stopwatch)
        {
            var sum = 0L;

            ExpandGalaxy(stopwatch);
            var galaxies = FindGalaxies();

            for (int i = 0; i < galaxies.Count; i++)
            {
                var current = galaxies[i];
                for (int j = i + 1; j < galaxies.Count; j++)
                {
                    var target = galaxies[j];
                    sum += Math.Abs(current.x - target.x) + Math.Abs(current.y - target.y);
                }

                Console.WriteLine($"[{stopwatch.Elapsed}] Done with: {i + 1} / {galaxies.Count}");
            }

            return sum;
        }

        private List<(int x, int y)> FindGalaxies()
        {
            List<(int x, int y)> galaxies = new();
            for (int i = 0; i < _data.Count; i++)
            {
                for (int j = 0; j < _data[i].Count; j++)
                {
                    var current = _data[i][j];
                    if (current == GALAXY)
                    {
                        galaxies.Add((i, j));
                    }
                }
            }

            return galaxies;
        }

        private void ExpandGalaxy(Stopwatch stopwatch)
        {
            const int LENGTH = 1_000_000;

            List<int> indexes = new List<int>();
            for (int i = 0; i < _data.Count; i++)
            {
                var currentRow = _data[i];
                if (currentRow.All(x => x == EMPTY_SPACE))
                {
                    indexes.Add(i + indexes.Count);
                }
            }

            foreach (var item in indexes)
            {
                List<char> list = new List<char>();
                for (int i = 0; i < LENGTH; i++)
                {
                    list.Add(EMPTY_SPACE);
                }
                _data.Insert(item, list);
            }

            for (int i = 0; i < _data.Count; i++)
            {
                for (int j = _data[i].Count; j < LENGTH; j++)
                {
                    _data[i].Add(EMPTY_SPACE);
                }
            }

            indexes.Clear();
            for (int j = 0; j < _data[0].Count; j++)
            {
                bool isAllEmptySpace = true;
                for (int i = 0; i < _data.Count; i++)
                {
                    var current = _data[i][j];
                    if (current != EMPTY_SPACE)
                    {
                        isAllEmptySpace = false;
                        break;
                    }
                }

                if (isAllEmptySpace)
                {
                    indexes.Add(j + indexes.Count);
                }
            }

            for (int i = 0; i < indexes.Count; i++)
            {
                for (int j = 0; j < _data.Count; j++)
                {
                    _data[j].Insert(indexes[i], EMPTY_SPACE);
                }

                if ((i + 1) % 1000 == 0)
                {
                    Console.WriteLine($"[{stopwatch.Elapsed}] Done with: {i + 1} / {indexes.Count}");
                }
            }
        }

        public void Result()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            ReadData();
            var result = Solve(stopwatch);
            Console.WriteLine($"[{stopwatch}] Your answer: {result} -- Took: {stopwatch.Elapsed}");
        }

        public void ReadData()
        {
            var lines = File.ReadAllLines(@$"{_className}\{(_useTestData ? "Test" : "Data")}.txt");
            _data = lines.Select(x => x.ToList()).ToList();
        }
    }
}