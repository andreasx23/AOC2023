using AOC2023.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day24
{
    public class Day24Part1
    {
        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day24";
        private List<(double px, double py, double pz, double vx, double vy, double vz)> _data = new();

        public long Solve(Stopwatch watch)
        {
            long sum = 0;

            var min = _useTestData ? 7D : 200000000000000D;
            var max = _useTestData ? 27D : 400000000000000D;
            for (int i = 0; i < _data.Count; i++)
            {
                var current = _data[i];
                for (int j = i + 1; j < _data.Count; j++)
                {
                    var next = _data[j];

                    (double x1, double x2) = CalculateLocation(current.px, current.vx);
                    (double x3, double x4) = CalculateLocation(next.px, next.vx);
                    (double y1, double y2) = CalculateLocation(current.py, current.vy);
                    (double y3, double y4) = CalculateLocation(next.py, next.vy);

                    var denominator = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
                    if (denominator != 0D)
                    {
                        var px = ((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4)) / denominator;
                        var py = ((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4)) / denominator;
                        var validA = (px > x1) == (x2 > x1);
                        var validB = (px > x3) == (x4 > x3);

                        if (px.IsInsideRange(min, max) && py.IsInsideRange(min, max) && validA && validB)
                        {
                            sum++;
                        }
                    }
                }
            }

            return sum;
        }

        private (double p, double pv) CalculateLocation(double p, double v)
        {
            return (p, p + v);
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
                var split = item.Split('@').Select(x => x.Trim()).ToList();
                var p = split.First().Split(',').Select(x => double.Parse(x.Trim())).ToList();
                var v = split.Last().Split(',').Select(x => double.Parse(x.Trim())).ToList();
                _data.Add((p[0], p[1], p[2], v[0], v[1], v[2]));
            }
        }
    }
}