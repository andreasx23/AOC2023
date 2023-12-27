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
        private static readonly bool _useTestData = true;
        private static readonly string _className = "Day24";
        private List<(double px, double py, double pz, double vx, double vy, double vz)> _data = new();

        // 1553 To low
        // 28121 To high
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
                    Console.WriteLine();
                }

                break;
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

            Console.WriteLine(hailstone1 + " " + hailstone2);

            FindIntersection(hailstone1FuturePositionStart,
                             hailstone1FuturePositionEnd,
                             hailstone2FuturePositionStart,
                             hailstone2FuturePositionEnd,
                             out bool doesLinesIntersect,
                             out bool doesSegmentsIntersect,
                             out var intersectionPoint);

            if (doesLinesIntersect)
            {
                if (IsInsideTestArea((intersectionPoint.x, intersectionPoint.x), testArea) && IsInsideTestArea((intersectionPoint.y, intersectionPoint.y), testArea))
                {
                    Console.WriteLine($"YES: ({intersectionPoint.x}, {intersectionPoint.y})");
                    return true;
                }

                Console.WriteLine("NO");
            }

            return false;
        }

        private void FindIntersection((double x, double y, double z) p1,
                                      (double x, double y, double z) p2,
                                      (double x, double y, double z) p3,
                                      (double x, double y, double z) p4,
                                      out bool linesIntersects,
                                      out bool segmentsIntersects,
                                      out (double x, double y, double z) intersection)
        {
            // Get the segments' parameters.
            var x1 = p2.x - p1.x;
            var y1 = p2.y - p1.y;
            var x2 = p4.x - p3.x;
            var y2 = p4.y - p3.y;

            // Solve for t1 and t2
            var denominator = y1 * x2 - x1 * y2;

            var t1 = ((p1.x - p3.x) * y2 + (p3.y - p1.y) * x2) / denominator;
            if (double.IsInfinity(t1))
            {
                // The lines are parallel (or close enough to it).
                linesIntersects = false;
                segmentsIntersects = false;
                intersection = (double.NaN, double.NaN, double.NaN);
            }
            else
            {
                linesIntersects = true;

                var t2 = ((p3.x - p1.x) * y1 + (p1.y - p3.y) * x1) / -denominator;

                // Find the point of intersection.
                intersection = (p1.x + x1 * t1, p1.y + y1 * t1, double.NaN);

                // The segments intersect if t1 and t2 are between 0 and 1.
                segmentsIntersects = ((t1 >= 0) && (t1 <= 1) && (t2 >= 0) && (t2 <= 1));
            }
        }

        private (double x, double y, double z) FuturePosition((double px, double py, double pz, double vx, double vy, double vz) hailstone, double time)
        {
            var dx = hailstone.px + hailstone.vx * time;
            var dy = hailstone.py + hailstone.vy * time;
            var dz = hailstone.pz + hailstone.vz * time;
            return (dx, dy, dz);
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