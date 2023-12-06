using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023.Day03
{
    internal class Day03Part1
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

                string number = string.Empty;
                List<int> js = new List<int>();
                for (int j = 0; j < currentStr.Length; j++)
                {
                    var currentChar = currentStr[j];
                    if (char.IsDigit(currentChar))
                    {
                        number += currentChar.ToString();
                        js.Add(j);
                    }
                    else
                    {
                        sum = CheckJsAndAddToSum(sum, i, number, js);
                        number = string.Empty;
                        js.Clear();
                    }
                }
                sum = CheckJsAndAddToSum(sum, i, number, js);
            }

            return sum;
        }

        private int CheckJsAndAddToSum(int sum, int i, string number, List<int> js)
        {
            if (js.Any())
            {
                bool found = false;
                foreach (var currentJ in js)
                {
                    var validNeighbours = ValidNeighbours(i, currentJ);

                    if (validNeighbours.Any(x => _data[x.x][x.y] != '.' && !char.IsDigit(_data[x.x][x.y])))
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    sum += int.Parse(number);
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
            var lines = File.ReadAllLines(@$"..\..\..\{_className}\{(_useTestData ? "Test" : "Data")}.txt");
            _data = lines.ToList();
        }
    }
}
