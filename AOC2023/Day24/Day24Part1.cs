using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day24
{
    public class Day24Part1
    {
        private static readonly bool _useTestData = true;
        private static readonly string _className = "Day24";
        private List<(double px, double py, double pz, double vx, double vy, double vz)> _data = new();

        // 1553 To low
        public long Solve(Stopwatch watch)
        {
            long sum = 0;

            var min = _useTestData ? 7D : 200000000000000D;
            var max = _useTestData ? 27D : 400000000000000D;
            for (int i = 0; i < _data.Count; i++)
            {
                var hailstone1 = _data[i];
                for (int j = i + 1; j < _data.Count; j++)
                {
                    var hailstone2 = _data[j];
                    if (CheckIntersection(hailstone1, hailstone2, (min, max), (min, max)))
                    {
                        sum++;
                    }

                    return 0;
                }
            }

            return sum;
        }

        private bool CheckIntersection((double px, double py, double pz, double vx, double vy, double vz) hailstone1,
                                       (double px, double py, double pz, double vx, double vy, double vz) hailstone2,
                                       (double min, double max) range,
                                       (double min, double max) testArea)
        {
            var hailstone1FuturePositionStart = FuturePosition(hailstone1, range.min);
            var hailstone1FuturePositionEnd = FuturePosition(hailstone1, range.max);

            var hailstone2FuturePositionStart = FuturePosition(hailstone2, range.min);
            var hailstone2FuturePositionEnd = FuturePosition(hailstone2, range.max);



            return false;
        }

        //private bool CheckIntersection((double px, double py, double pz, double vx, double vy, double vz) hailstone1,
        //                               (double px, double py, double pz, double vx, double vy, double vz) hailstone2,
        //                               (double min, double max) range,
        //                               (double min, double max) testArea)
        //{
        //    var hailstone1FuturePositionStart = FuturePosition(hailstone1, range.min);
        //    var hailstone1FuturePositionEnd = FuturePosition(hailstone1, range.max);

        //    var hailstone2FuturePositionStart = FuturePosition(hailstone2, range.min);
        //    var hailstone2FuturePositionEnd = FuturePosition(hailstone2, range.max);

        //    //if (IsIntersection((hailstone1FuturePositionStart.x, hailstone1FuturePositionEnd.x), (hailstone2FuturePositionStart.x, hailstone2FuturePositionEnd.x), testArea)
        //    //    && IsIntersection((hailstone1FuturePositionStart.y, hailstone1FuturePositionEnd.y), (hailstone2FuturePositionStart.y, hailstone2FuturePositionEnd.y), testArea))
        //    //{
        //    //    return true;
        //    //}

        //    return false;
        //}

        private (double x, double y, double z) FuturePosition((double px, double py, double pz, double vx, double vy, double vz) hailstone, double time)
        {
            var dx = hailstone.px + hailstone.vx * time;
            var dy = hailstone.py + hailstone.vy * time;
            var dz = hailstone.pz + hailstone.vz * time;
            return (dx, dy, dz);
        }

        private bool IsIntersection((double start, double end) value1, (double start, double end) value2, (double min, double max) testArea)
        {
            var min1 = Math.Min(value1.start, value1.end);
            var max1 = Math.Max(value1.start, value1.end);

            var min2 = Math.Min(value2.start, value2.end);
            var max2 = Math.Max(value2.start, value2.end);

            if (min1 <= max2 && min2 <= max1 && IsInsideTestArea((min1, max1), testArea) && IsInsideTestArea((min2, max2), testArea))
            {
                return true;
            }

            return false;
        }

        private bool IsInsideTestArea((double start, double end) futurePosition, (double min, double max) testArea)
        {
            return testArea.min <= futurePosition.start && futurePosition.end <= testArea.max;
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