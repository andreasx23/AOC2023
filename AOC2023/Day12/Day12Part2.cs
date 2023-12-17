using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day12
{
    public class Day12Part2
    {
        class Operation
        {
            public string Row { get; set; }
            public List<int> Requirements { get; set; } = new();
        }

        private static readonly bool _useTestData = true;
        private static readonly string _className = "Day12";
        private List<Operation> _data = new();
        private const char OPERATIONAL = '.';
        private const char BROKEN = '#';
        private const char UNKNOWN = '?';
        private Dictionary<string, long> _cache = new();

        public long Solve(Stopwatch watch)
        {
            var operations = _data.First();

            // 1024000 to low
            long sum = 0;
            foreach (var operation in _data)
            {
                Console.WriteLine($"[{watch.Elapsed}] running: {operation.Row} {string.Join(", ", operation.Requirements)}");
                var localSum = FindArrangements(operations.Row, operations.Requirements);
                Console.WriteLine($"[{watch.Elapsed}] done running: {operation.Row}");
                Console.WriteLine($"Sum: {localSum}");
                sum += localSum;
            }

            return sum;
        }

        // Based on https://github.com/hyper-neutrino/advent-of-code/blob/main/2023/day12p2.py
        // Explanation : https://www.youtube.com/watch?v=g3Ms5e7Jdqo
        // https://github.com/bhosale-ajay/adventofcode/blob/master/2023/ts/D12.test.ts

        public long FindArrangements(string conditions, List<int> requirements)
        {
            if (string.IsNullOrEmpty(conditions))
            {
                return requirements.Count == 0 ? 0 : 1;
            }

            if (requirements.Count == 0)
            {
                return conditions.Contains('#') ? 0 : 1;
            }

            var key = $"{conditions}-{string.Join(",", requirements)}";
            if (_cache.ContainsKey(key))
            {
                return _cache[key];
            }

            long result = 0;
            var condition = conditions[0];
            if (condition == '.' || condition == '?')
            {
                result += FindArrangements(conditions.Substring(1), requirements);
            }

            var firstReq = requirements[0];
            var remainingReqs = requirements.Skip(1).ToList();
            if (condition == '#' || condition == '?')
            {
                if (firstReq <= conditions.Length
                    && !conditions.Substring(0, firstReq).Contains('.')
                    && (firstReq == conditions.Length || conditions[firstReq] != '#'))
                {
                    result += FindArrangements(conditions.Substring(firstReq + 1), remainingReqs);
                }
            }

            _cache[key] = result;
            return result;
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
                var splits = item.Split(' ');

                var row = splits[0].Select(x => x).ToList();
                var str = "";

                var requirements = splits[1].Trim().Split(',').Select(x => int.Parse(x.Trim())).ToList();
                var reqs = new List<int>();
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < row.Count; j++)
                    {
                        str += row[j].ToString();
                    }

                    str += "?";

                    for (int j = 0; j < requirements.Count; j++)
                    {
                        reqs.Add(requirements[j]);
                    }
                }

                var operation = new Operation()
                {
                    Row = str,
                    Requirements = reqs
                };
                _data.Add(operation);
            }
        }
    }
}