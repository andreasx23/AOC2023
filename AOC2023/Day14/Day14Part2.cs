using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day14
{
    public class Day14Part2
    {
        enum Direction
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }

        private static readonly bool _useTestData = true;
        private static readonly string _className = "Day14";
        private List<List<char>> _data = new();

        public long Solve(Stopwatch watch)
        {
            long sum = 0;

            // Currently not done with this one yet!

            var cycles = _useTestData ? 1 : 1000000000;
            for (int i = 0; i < cycles; i++)
            {
                Tilt(Direction.UP);
                Tilt(Direction.LEFT);
                Tilt(Direction.DOWN);
                Tilt(Direction.RIGHT);
            }

            foreach (var item in _data)
            {
                Console.WriteLine(string.Join("", item));
            }

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
                                {
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
                                }
                                break;
                            case Direction.DOWN:
                                {
                                    var k = i;
                                    while (k + 1 < _data.Count)
                                    {
                                        if (_data[k + 1][j] == '.')
                                        {
                                            k++;
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
                                }
                                break;
                            case Direction.LEFT:
                                {
                                    var k = j;
                                    while (k - 1 >= 0)
                                    {
                                        if (_data[i][k - 1] == '.')
                                        {
                                            k--;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    if (j != k)
                                    {
                                        _data[i][k] = 'O';
                                        _data[i][j] = '.';
                                    }
                                }
                                break;
                            case Direction.RIGHT:
                                {
                                    var k = j;
                                    while (k + 1 < _data.First().Count)
                                    {
                                        if (_data[i][k + 1] == '.')
                                        {
                                            k++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    if (j != k)
                                    {
                                        _data[i][k] = 'O';
                                        _data[i][j] = '.';
                                    }
                                }
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