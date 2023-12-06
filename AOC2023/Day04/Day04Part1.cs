using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023.Day04
{
    internal class Day04Part1
    {
        class Card
        {
            public int Id { get; private set; }
            public List<int> WinningNumbers { get; private set; }
            public List<int> MyNumbers { get; private set; }

            public Card(int id, List<int> winningNumbers, List<int> myNumbers)
            {
                Id = id;
                WinningNumbers = winningNumbers;
                MyNumbers = myNumbers;
            }
        }

        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day04";
        private List<Card> _data = new();

        public int Solve()
        {
            var sum = 0;

            foreach (var item in _data)
            {
                var intersection = item.MyNumbers.Intersect(item.WinningNumbers).ToList();
                if (intersection.Any())
                {
                    var score = 1;
                    for (int i = 1; i < intersection.Count; i++)
                    {
                        score *= 2;
                    }
                    sum += score;
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
            var lines = File.ReadAllLines(@$"..\..\..\{_className}\{(_useTestData ? "Test" : "Data")}.txt");

            var id = 1;
            foreach (var item in lines)
            {
                var split = item.Split(':', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
                var numbers = split.Last().Split('|', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
                var winningNumbers = numbers.First().Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x.Trim())).ToList();
                var myNumbers = numbers.Last().Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x.Trim())).ToList();
                Card card = new Card(id, winningNumbers, myNumbers);
                _data.Add(card);
                id++;
            }
        }
    }
}
