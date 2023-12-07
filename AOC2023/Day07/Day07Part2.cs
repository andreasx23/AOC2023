using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day07
{
    public class Day07Part2
    {
        public class CamelCard
        {
            private const string JOKER = "J";
            private static readonly Dictionary<string, int> _cardScoreDict = new()
            {
                { "A", 14 },
                { "K", 13 },
                { "Q", 12 },
                { "T", 10 },
                { "9", 9 },
                { "8", 8 },
                { "7", 7 },
                { "6", 6 },
                { "5", 5 },
                { "4", 4 },
                { "3", 3 },
                { "2", 2 },
                { JOKER, 1 },
            };

            public string Id { get; private set; }
            public List<string> Hand { get; private set; } = new List<string>();
            public int Bid { get; private set; }
            public int HandScore { get; private set; }

            public CamelCard(string id, int bid)
            {
                Id = id;
                Bid = bid;
                foreach (var item in Id)
                {
                    Hand.Add(item.ToString());
                }
                HandScore = GetHandScore();
            }

            private (Dictionary<string, int> dict, int jokers) GetCardCountDict()
            {
                int jokers = 0;
                Dictionary<string, int> cardsDict = new();
                foreach (var item in Hand)
                {
                    if (item == JOKER)
                    {
                        jokers++;
                    }
                    else
                    {
                        if (!cardsDict.ContainsKey(item))
                        {
                            cardsDict[item] = 0;
                        }

                        cardsDict[item]++;
                    }
                }

                return (cardsDict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, v => v.Value), jokers);
            }

            public long GetBidScore(int rank)
            {
                return Bid * rank;
            }

            public bool IsFiveOfAKind((Dictionary<string, int> dict, int jokers) cards)
            {
                if (cards.jokers == 5)
                {
                    return true;
                }

                var sum = cards.dict.First().Value + cards.jokers;
                return sum == 5;
            }

            public bool IsFourOfAKind((Dictionary<string, int> dict, int jokers) cards)
            {
                var sum = cards.dict.First().Value + cards.jokers;
                return sum == 4;
            }

            public bool IsThreeOfAKind((Dictionary<string, int> dict, int jokers) cards)
            {
                var sum = cards.dict.First().Value + cards.jokers;
                return sum == 3;
            }

            public bool IsFullHouse((Dictionary<string, int> dict, int jokers) cards)
            {
                var values = cards.dict.Values.ToList();
                var sum = values.Take(2).Sum() + cards.jokers;
                return sum == 5;
            }

            public bool IsTwoPair((Dictionary<string, int> dict, int jokers) cards)
            {
                var values = cards.dict.Values.ToList();
                var sum = values.Take(2).Sum() + cards.jokers;
                return sum == 4;
            }

            public bool IsOnePair((Dictionary<string, int> dict, int jokers) cards)
            {
                return cards.dict.First().Value + cards.jokers == 2;
            }

            public bool IsHighCard((Dictionary<string, int> dict, int jokers) cards)
            {
                return cards.dict.Count == 5;
            }

            public int GetHandScore()
            {
                var cards = GetCardCountDict();

                if (IsFiveOfAKind(cards))
                {
                    return 100;
                }
                else if (IsFourOfAKind(cards))
                {
                    return 99;
                }
                else if (IsFullHouse(cards))
                {
                    return 98;
                }
                else if (IsThreeOfAKind(cards))
                {
                    return 97;
                }
                else if (IsTwoPair(cards))
                {
                    return 96;
                }
                else if (IsOnePair(cards))
                {
                    return 95;
                }
                else if (IsHighCard(cards))
                {
                    return 94;
                }

                throw new InvalidOperationException("Every hand is exactly ONE type");
            }

            public bool IsMyHandStronger(CamelCard otherPlayer)
            {
                for (int i = 0; i < otherPlayer.Hand.Count; i++)
                {
                    var myCard = Hand[i];
                    var myCardScore = _cardScoreDict[myCard];

                    var otherCard = otherPlayer.Hand[i];
                    var otherCardScore = _cardScoreDict[otherCard];
                    if (myCardScore == otherCardScore)
                    {
                        continue;
                    }
                    else if (myCardScore > otherCardScore)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                throw new InvalidOperationException("We should never reach this point!");
            }
        }

        public class CamelCardComparer : IComparer<CamelCard>
        {
            public int Compare(CamelCard x, CamelCard y)
            {
                if (x.HandScore == y.HandScore)
                {
                    return x.IsMyHandStronger(y) ? 1 : -1;
                }
                else if (x.HandScore > y.HandScore)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }

        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day07";
        private List<CamelCard> _camelCards = new();

        public long Solve()
        {
            var comparer = new CamelCardComparer();
            var sortedCamelCards = _camelCards.OrderBy(x => x, comparer).ToList();

            var sum = 0L;
            for (int i = 0; i < sortedCamelCards.Count; i++)
            {
                sum += sortedCamelCards[i].GetBidScore(i + 1);
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

            for (int i = 0; i < lines.Length; i++)
            {
                string? line = lines[i];
                var split = line.Split(' ').ToList();
                _camelCards.Add(new CamelCard(split[0], int.Parse(split[1])));
            }
        }
    }
}
