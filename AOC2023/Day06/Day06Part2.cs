using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day06
{
    internal class Day06Part2
    {
        private class Race
        {
            public long Time { get; set; }
            public long Record { get; set; }
        }

        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day06";
        private Race _race = new();

        public int Solve()
        {
            var sum = 0;

            for (long speed = 0; speed <= _race.Time; speed++)
            {
                var timeLeft = _race.Time - speed;
                var totalDistanceTraveled = timeLeft * speed;
                if (totalDistanceTraveled > _race.Record)
                {
                    sum++;
                }
            }

            return sum;
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
            var lines = File.ReadAllLines(@$"{_className}\{(_useTestData ? "Test" : "Data")}.txt");
            var times = lines[0].Split(':')[1];
            times = Regex.Replace(times, @"\s+", "").Trim();

            var distances = lines[1].Split(':')[1];
            distances = Regex.Replace(distances, @"\s+", "").Trim();
            _race = new Race()
            {
                Time = long.Parse(times),
                Record = long.Parse(distances)
            };
        }
    }
}
