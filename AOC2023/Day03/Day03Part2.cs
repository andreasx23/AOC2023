using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023.Day03
{
    internal class Day03Part2
    {
        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day03";
        private List<string> _data = new();

        public int Solve()
        {
            var sum = 0;

            for (int i = 0; i < _data.Count; i++)
            {
                var currentStr = _data[i];

                List<int> numbers = new List<int>();
                for (int j = 0; j < currentStr.Length; j++)
                {
                    var currentChar = currentStr[j];
                    if (currentChar == '*')
                    {
                        var validNeighbours = ValidNeighbours(i, j);
                        var digits = validNeighbours.Where(x => char.IsDigit(_data[x.x][x.y])).ToList();
                        var groups = digits.GroupBy(x => x.x);

                        foreach (var group in groups)
                        {
                            HashSet<(int x, int y)> seen = new HashSet<(int x, int y)>();
                            foreach (var (x, y) in group)
                            {
                                if (seen.Add((x, y)))
                                {
                                    var leftY = y;
                                    while (leftY >= 0 && char.IsDigit(_data[x][leftY]))
                                    {
                                        leftY--;
                                    }
                                    leftY++;

                                    var rightY = y;
                                    while (rightY < currentStr.Length && char.IsDigit(_data[x][rightY]))
                                    {
                                        rightY++;
                                    }
                                    rightY--;

                                    var number = string.Empty;
                                    for (int k = leftY; k <= rightY; k++)
                                    {
                                        number += _data[x][k].ToString();
                                        seen.Add((x, k));
                                    }

                                    numbers.Add(int.Parse(number));
                                }
                            }
                        }

                        if (numbers.Count == 2)
                        {
                            sum += numbers[0] * numbers[1];
                        }

                        numbers.Clear();
                    }
                }

                if (numbers.Count == 2)
                {
                    sum += numbers[0] * numbers[1];
                }
            }

            return sum;
        }

        private List<(int x, int y)> ValidNeighbours(int x, int y)
        {
            List<(int x, int y)> directions = new List<(int x, int y)>()
            {
                (-1, 0),
                (1, 0),
                (0, -1),
                (0, 1),
                (-1, -1),
                (-1, 1),
                (1, -1),
                (1, 1),
            };

            var height = _data.Count;
            var width = _data.First().Length;
            List<(int x, int y)> validNeighbours = new List<(int x, int y)>();
            foreach (var item in directions)
            {
                var dx = item.x + x;
                var dy = item.y + y;
                if (dx < 0 || dx >= height || dy < 0 || dy >= width)
                {
                    continue;
                }
                validNeighbours.Add((dx, dy));
            }
            return validNeighbours;
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
            var lines = File.ReadAllLines(@$"..\..\..\{_className}\{(_useTestData ? "Test2" : "DataVilly")}.txt");
            _data = lines.ToList();
        }
    }
}
