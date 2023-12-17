using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day14
{
    public class Day14Part1
    {
        enum Direction
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }

        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day14";
        private List<List<char>> _data = new();

        public long Solve(Stopwatch watch)
        {
            long sum = 0;

            Tilt(Direction.UP);

            for (int i = 0; i < _data.Count; i++)
            {
                for (int j = 0; j < _data[i].Count; j++)
                {
                    if (_data[i][j] == 'O')
                    {
                        sum += _data.Count - i;
                    }
                }
            }

            return sum;
        }

        private void Tilt(Direction direction)
        {
            for (int i = 1; i < _data.Count; i++)
            {
                for (int j = 0; j < _data[i].Count; j++)
                {
                    if (_data[i][j] == 'O')
                    {
                        switch (direction)
                        {
                            case Direction.UP:
                                var k = i;
                                while (k - 1 >= 0)
                                {
                                    if (_data[k - 1][j] == '.')
                                    {
                                        k--;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                if (i != k)
                                {
                                    _data[k][j] = 'O';
                                    _data[i][j] = '.';
                                }
                                break;
                            case Direction.DOWN:
                                break;
                            case Direction.LEFT:
                                break;
                            case Direction.RIGHT:
                                break;
                        }
                    }
                }
            }
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