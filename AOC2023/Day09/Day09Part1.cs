using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day09
{
    public class Day09Part1
    {
        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day09";
        private List<List<long>> _data = new();

        public long Solve()
        {
            var sum = 0L;

            foreach (var start in _data)
            {
                var allDataSets = new List<List<long>>();
                var currentDataSet = new List<long>(start);
                bool isDone = false;
                while (!isDone)
                {
                    allDataSets.Add(currentDataSet);
                    currentDataSet = GenerateNextDataSet(currentDataSet);
                    if (currentDataSet.All(x => x == 0))
                    {
                        allDataSets.Add(currentDataSet);
                        isDone = true;
                    }
                }

                var result = allDataSets.Last().Last() + allDataSets[^2].Last();

                allDataSets.Last().Add(0);
                allDataSets[^2].Add(result);

                for (int i = allDataSets.Count - 3; i >= 0; i--)
                {
                    var lastInSet = allDataSets[i].Last();
                    result = lastInSet + result;
                    allDataSets[i].Add(result);
                }

                //foreach (var item in allDataSets)
                //{
                //    Console.WriteLine(string.Join(", ", item));
                //}

                sum += result;
            }

            return sum;
        }

        private List<long> GenerateNextDataSet(List<long> currentDataSet)
        {
            List<long> nextDataSet = new(currentDataSet.Count - 1);
            for (int i = 1; i < currentDataSet.Count; i++)
            {
                nextDataSet.Add(currentDataSet[i] - currentDataSet[i - 1]);
            }

            return nextDataSet;
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
            var lines = File.ReadAllLines(@$"{_className}\{(_useTestData ? "Test3" : "Data")}.txt");
            _data = lines.Select(x => x.Split(' ').Select(x => long.Parse(x.Trim())).ToList()).ToList();
        }
    }
}