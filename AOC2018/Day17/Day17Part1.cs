using Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2018.Day17
{
    public class Day17Part1
    {
        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day18";
        private List<string> _data = new();

        public long Solve(Stopwatch watch)
        {
            var sum = 0L;

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