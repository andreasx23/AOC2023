using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023.Day01
{
    internal class Day03Part1
    {
        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day01";
        private List<string> _data = new List<string>();

        public int Solve()
        {
            var sum = 0;
            foreach (var row in _data)
            {
                var leftDigit = row.First(char.IsDigit).ToString();
                var rightDigit = row.Last(char.IsDigit).ToString();
                var digit = int.Parse($"{leftDigit}{rightDigit}");
                sum += digit;
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
            var lines = File.ReadAllLines(@$"..\..\..\{_className}\{(_useTestData ? "Test" : "Data")}.txt");
            _data = lines.ToList();
        }
    }
}
