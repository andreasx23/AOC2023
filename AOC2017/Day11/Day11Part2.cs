using Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2017.Day11
{
    public class Day11Part2
    {
        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day11";
        private List<string> _data = new();
        
        public long Solve(Stopwatch watch)
        {
            var sum = 0L;

            int x = 0;
            int y = 0;

            List<(int x, int y)> allSteps = new();
            foreach (var item in _data)
            {
                var dir = DirectionsHelper.HexagonDirections[item];
                x += dir.x;
                y += dir.y;
                allSteps.Add((x, y));
            }

            int done = 0;
            object @lock = new();
            Parallel.ForEach(allSteps, step =>
            {
                var localSum = DistanceHelper.CalculateDistanceFor3DSpaceIn2DSpace((0, 0), step);
                lock (@lock)
                {
                    done++;

                    if (localSum > sum)
                    {
                        sum = localSum;
                        Console.WriteLine($"[{watch.Elapsed}] New max: {sum} -- Done with: {done} / {allSteps.Count}");
                    }

                    if (done % 250 == 0)
                    {
                        Console.WriteLine($"[{watch.Elapsed}] Current max: {sum} -- Done with: {done} / {allSteps.Count}");
                    }
                }
            });

            return sum;
        }

        public void Result()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            ReadData();
            var result = Solve(stopwatch);
            Console.WriteLine($"Your answer: {result} -- Took: {stopwatch.Elapsed}");
        }

        private void ReadData()
        {
            var lines = File.ReadAllLines(@$"{_className}\{(_useTestData ? "Test2" : "Data")}.txt");
            _data = lines.First().Split(',').ToList();
        }
    }
}