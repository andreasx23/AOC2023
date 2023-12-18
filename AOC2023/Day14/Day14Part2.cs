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

        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day14";
        private List<List<char>> _data = new();
        private Dictionary<string, List<List<char>>> _cache = new();

        public long Solve(Stopwatch watch)
        {
            long sum = 0;

            var cycles = 1000000000;
            bool firstCacheHit = true;
            for (int i = 0; i < cycles; i++)
            {
                var key = GetKey();
                if (_cache.TryGetValue(key, out var value))
                {
                    if (firstCacheHit)
                    {
                        var cyclesLeft = cycles % _cache.Count;
                        i = cycles - cyclesLeft;
                        firstCacheHit = false;
                    }

                    sum = Math.Max(sum, Sum());

                    _data = value;
                    continue;
                }

                Tilt(Direction.UP);
                Tilt(Direction.LEFT);
                Tilt(Direction.DOWN);
                Tilt(Direction.RIGHT);

                _cache[key] = Clone();
            }

            return sum;
        }

        private long Sum()
        {
            var sum = 0L;
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

        private string GetKey()
        {
            StringBuilder sb = new();
            foreach (var row in _data)
            {
                foreach (var c in row)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        private List<List<char>> Clone()
        {
            return _data.Select(row => row.ToList()).ToList();
        }

        private void Print()
        {
            foreach (var item in _data)
            {
                Console.WriteLine(string.Join("", item));
            }

            Console.WriteLine();
        }

        private void Tilt(Direction direction)
        {
            switch (direction)
            {
                case Direction.UP:
                    TiltUp();
                    break;
                case Direction.DOWN:
                    TiltDown();
                    break;
                case Direction.LEFT:
                    TiltLeft();
                    break;
                case Direction.RIGHT:
                    TiltRight();
                    break;
            }
        }

        private void TiltUp()
        {
            for (int i = 1; i < _data.Count; i++)
            {
                for (int j = 0; j < _data[i].Count; j++)
                {
                    if (_data[i][j] == 'O')
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
                }
            }
        }

        private void TiltDown()
        {
            for (int i = _data.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < _data[i].Count; j++)
                {
                    if (_data[i][j] == 'O')
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
                }
            }
        }

        private void TiltLeft()
        {
            for (int i = 0; i < _data.Count; i++)
            {
                for (int j = 0; j < _data[i].Count; j++)
                {
                    if (_data[i][j] == 'O')
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
                }
            }
        }

        private void TiltRight()
        {
            for (int i = 0; i < _data.Count; i++)
            {
                for (int j = _data[i].Count - 1; j >= 0; j--)
                {
                    if (_data[i][j] == 'O')
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