using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023.Day01
{
    internal class Day01Part2
    {
        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day01";
        private List<string> _data = new List<string>();
        private readonly Dictionary<string, int> _wordsAsNumbers = new Dictionary<string, int>()
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 },
        };

        public int Solve()
        {
            var sum = 0;
            foreach (var row in _data)
            {
                var left = GetNumber(row, true);
                var right = GetNumber(row, false);
                sum += int.Parse($"{left}{right}");
            }
            return sum;
        }

        private int GetNumber(string word, bool startLeft)
        {
            var strIndex = startLeft ? int.MaxValue : int.MinValue;
            var number = -1;

            var charNumber = startLeft ? word.FirstOrDefault(x => char.IsDigit(x)).ToString() : word.LastOrDefault(x => char.IsDigit(x)).ToString();
            if (!string.IsNullOrEmpty(charNumber) && charNumber != "\0")
            {
                var charIndex = startLeft ? word.IndexOf(charNumber) : word.LastIndexOf(charNumber);
                if (startLeft)
                {
                    if (strIndex > charIndex)
                    {
                        strIndex = charIndex;
                        number = int.Parse(word[charIndex].ToString());
                    }
                }
                else
                {
                    if (charIndex > strIndex)
                    {
                        strIndex = charIndex;
                        number = int.Parse(word[charIndex].ToString());
                    }
                }
            }

            foreach (var item in _wordsAsNumbers)
            {
                var index = startLeft ? word.IndexOf(item.Key) : word.LastIndexOf(item.Key);
                if (index != -1)
                {
                    if (startLeft)
                    {
                        if (strIndex > index)
                        {
                            strIndex = index;
                            number = item.Value;
                        }
                    }
                    else
                    {
                        if (index > strIndex)
                        {
                            strIndex = index;
                            number = item.Value;
                        }
                    }
                }
            }

            return number;
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
            var lines = File.ReadAllLines(@$"..\..\..\{_className}\{(_useTestData ? "Test2" : "Data")}.txt");
            _data = lines.ToList();
        }
    }
}
