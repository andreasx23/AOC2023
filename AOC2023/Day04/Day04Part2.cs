using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023.Day04
{
    internal class Day04Part2
    {
        class Card
        {
            public int Id { get; private set; }
            public int MatchingNumbers { get; private set; }

            public Card(int id, List<int> winningNumbers, List<int> myNumbers)
            {
                Id = id;
                MatchingNumbers = winningNumbers.Intersect(myNumbers).Count();
            }
        }

        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day04";
        private List<Card> _cards = new();
        private Dictionary<int, int> _matches = new();

        public int Solve()
        {
            foreach (var card in _cards)
            {
                var amount = _matches[card.Id];
                for (int i = 0; i < card.MatchingNumbers; i++)
                {
                    _matches[card.Id + 1 + i] += amount;
                }
            }

            return _matches.Sum(x => x.Value);
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
                _cards.Add(card);
                _matches.Add(id, 1);
                id++;
            }
        }
    }
}
