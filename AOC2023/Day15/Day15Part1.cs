using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day15
{
    public class Day15Part1
    {
        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day15";
        private List<string> _data = new List<string>();

        public long Solve(Stopwatch watch)
        {
            var sum = 0L;

            foreach (var line in _data)
            {
                var localSum = 0L;
                foreach (var item in line)
                {
                    var ascii = item + localSum;
                    ascii *= 17;
                    ascii %= 256;
                    localSum = ascii;
                }
                sum += localSum;
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
            _data = lines[0].Split(',').ToList();
        }
    }
}