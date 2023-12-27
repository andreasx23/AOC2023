using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day22
{
    public class Day22Part1
    {
        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day22";
        private List<List<(int x, int y, int z)>> _bricks = new();

        public long Solve(Stopwatch watch)
        {
            long sum = 0;

            HashSet<(int x, int y, int z)> seen = new();
            foreach (var brick in _bricks)
            {
                foreach (var item in brick)
                {
                    seen.Add(item);
                }
            }

            while (true)
            {
                var isDone = true;
                for (int i = 0; i < _bricks.Count; i++)
                {
                    List<(int x, int y, int z)> brick = _bricks[i];
                    var ok = true;
                    foreach (var item in brick)
                    {
                        if (item.z == 1 || seen.Contains((item.x, item.y, item.z - 1)) && !brick.Contains((item.x, item.y, item.z - 1)))
                        {
                            ok = false;
                            break;
                        }
                    }

                    if (ok)
                    {
                        isDone = false;

                        List<(int x, int y, int z)> newBrick = new();
                        foreach (var item in brick)
                        {
                            if (!seen.Remove(item))
                            {
                                throw new Exception("Invalid");
                            }
                            seen.Add((item.x, item.y, item.z - 1));
                            newBrick.Add((item.x, item.y, item.z - 1));
                        }

                        _bricks[i] = newBrick;
                    }
                }

                if (isDone)
                {
                    break;
                }
            }

            for (int i = 0; i < _bricks.Count; i++)
            {
                List<(int x, int y, int z)> brick = _bricks[i];
                foreach (var item in brick)
                {
                    seen.Remove(item);
                }

                bool isValid = false;
                for (int j = 0; j < _bricks.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    List<(int x, int y, int z)> b = _bricks[j];
                    var ok = true;
                    foreach (var item in b)
                    {
                        if (item.z == 1 || seen.Contains((item.x, item.y, item.z - 1)) && !b.Contains((item.x, item.y, item.z - 1)))
                        {
                            ok = false;
                            break;
                        }
                    }

                    if (ok)
                    {
                        isValid = true;
                        break;
                    }
                }

                if (!isValid) 
                {
                    sum++;
                }

                foreach (var item in brick)
                {
                    seen.Add(item);
                }
            }

            return sum;
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
            foreach (var item in lines)
            {
                var split = item.Split('~');
                var left = split.First().Split(',').Select(int.Parse).ToList();
                var right = split.Last().Split(',').Select(int.Parse).ToList();

                List<(int x, int y, int z)> brick = new List<(int x, int y, int z)>();
                (int sx, int sy, int sz) = (left[0], left[1], left[2]);
                (int ex, int ey, int ez) = (right[0], right[1], right[2]);
                if (sx == ex && sy == ey)
                {
                    for (int z = sz; z < ez + 1; z++)
                    {
                        brick.Add((sx, sy, z));
                    }
                }
                else if (sx == ex && sz == ez)
                {
                    for (int y = sy; y < ey + 1; y++)
                    {
                        brick.Add((sx, y, sz));
                    }
                }
                else if (sy == ey && sz == ez)
                {
                    for (int x = sx; x < ex + 1; x++)
                    {
                        brick.Add((x, sy, sz));
                    }
                }
                else
                {
                    throw new Exception("Invalid");
                }

                _bricks.Add(brick);
            }
        }
    }
}