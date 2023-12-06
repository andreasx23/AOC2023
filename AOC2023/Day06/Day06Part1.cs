using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day06
{
    internal class Day06Part1
    {
        private class Race
        {
            public int Time { get; set; }
            public int Record { get; set; }
        }

        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day06";
        private List<Race> _races = new();

        public int Solve()
        {
            var sum = 1;
            foreach (var race in _races) 
            {
                var newRecord = 0;
                for (int speed = 0; speed <= race.Time; speed++)
                {
                    var timeLeft = race.Time - speed;
                    var totalDistanceTraveled = timeLeft * speed;
                    if (totalDistanceTraveled > race.Record)
                    {
                        newRecord++;
                    }
                }

                sum *= newRecord;
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
            times = Regex.Replace(times, @"\s+", " ").Trim();

            var distances = lines[1].Split(':')[1];
            distances = Regex.Replace(distances, @"\s+", " ").Trim();

            var timesInt = times.Split(' ').Select(int.Parse).ToList();
            var distancesInt = distances.Split(' ').Select(int.Parse).ToList();
            for (int i = 0; i < timesInt.Count; i++)
            {
                var time = timesInt[i];
                var distance = distancesInt[i];
                _races.Add(new Race
                {
                    Record = distance,
                    Time = time
                });
            }
        }
    }
}
