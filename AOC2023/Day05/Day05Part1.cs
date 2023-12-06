using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023.Day05
{
    internal class Day05Part1
    {
        class Range
        {
            public string Action { get; set; }
            public List<List<long>> Ranges { get; set; } = new();

            public (long start, long end) GetDistanceRange(List<long> range)
            {
                return (range[0], range[0] + range[2] - 1);
            }

            public (long start, long end) GetSourceRange(List<long> range)
            {
                return (range[1], range[1] + range[2] - 1);
            }

            public bool IsSeedWithinRange(long seed, List<long> range)
            {
                var sourceRange = GetSourceRange(range);
                return sourceRange.start <= seed && seed <= sourceRange.end;
            }

            public long GetSeedScore(long seed, List<long> range)
            {
                var sourceRange = GetSourceRange(range);
                var distanceRange = GetDistanceRange(range);
                return seed - sourceRange.start + distanceRange.start;
            }

            public long CalculateSeed(long seed)
            {
                foreach (var item in Ranges)
                {
                    if (IsSeedWithinRange(seed, item))
                    {
                        return GetSeedScore(seed, item);
                    }
                }

                //Any source numbers that aren't mapped correspond to the same destination number. So, seed number 10 corresponds to soil number 10.
                return seed;
            }
        }

        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day05";
        private List<long> _seeds = new();
        private List<Range> _ranges = new();

        public long Solve()
        {
            List<long> seeds = new();

            foreach (var seed in _seeds)
            {
                var destionationSeed = long.MinValue;
                foreach (var range in _ranges)
                {
                    destionationSeed = range.CalculateSeed(destionationSeed != long.MinValue ? destionationSeed : seed);
                }

                seeds.Add(destionationSeed);
            }

            return seeds.Min();
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
            _seeds = lines.First().Split(':').Select(x => x.Trim()).Last().Split(' ').Select(x => long.Parse(x.Trim())).ToList();
            lines = lines.Skip(1).ToArray();
            var job = new Range();
            List<List<long>> ranges = new();
            var action = "";
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    if (!string.IsNullOrEmpty(action))
                    {
                        job.Action = action.Substring(0, action.Length - 1);
                        job.Ranges = ranges;
                        _ranges.Add(job);
                    }

                    job = new Range();
                    ranges = new();
                    action = "";
                    continue;
                }

                if (line.Any(x => char.IsLetter(x)))
                {
                    action = line;
                    continue;
                }

                var split = line.Split(' ').Select(x => long.Parse(x.Trim())).ToList();

                List<long> values = new();
                ranges.Add(split);
            }

            job.Action = action.Substring(0, action.Length - 1);
            job.Ranges = ranges;
            _ranges.Add(job);
        }
    }
}
