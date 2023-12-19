using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace AOC2023.Day19
{
    public class Day19Part2
    {
        class Workflow
        {
            public string Name { get; set; } // E.g. Name = in
            public string FullName { get; set; } // E.g. Name = in
            public List<Rule> Rules { get; set; } = new();
        }

        class Rule
        {
            // E.g. Rule "x>10:one": If the part's x is more than 10, send the part to the workflow named one.
            public string RuleStr { get; set; } // x>10:one
            public string Operator { get; set; } // >
            public int Amount { get; set; } // 10
            public string Name { get; set; } // x
            public string SendTo { get; set; } // one
            public bool IsSpecialRule => string.IsNullOrEmpty(Operator);
        }

        private static readonly bool _useTestData = true;
        private static readonly string _className = "Day19";
        private Dictionary<string, Workflow> _workflows = new();

        private const string START_WORKFLOW = "in";
        private const string ACCEPTED = "A";
        private const string REJECTED = "R";

        public long Solve(Stopwatch watch)
        {
            long sum = 0;

            sum = Bfs();

            return sum;
        }

        private long Bfs()
        {
            Queue<(string workFlowName, long xLow, long xHigh, long mLow, long mHigh, long aLow, long aHigh, long sLow, long sHigh)> queue = new();
            queue.Enqueue((START_WORKFLOW, 1, 4000, 1, 4000, 1, 4000, 1, 4000));

            long sum = 0;
            while (queue.Count > 0)
            {
                var (workFlowName, xLow, xHigh, mLow, mHigh, aLow, aHigh, sLow, sHigh) = queue.Dequeue();

                if (xLow > xHigh || mLow > mHigh || aLow > aHigh || sLow > sHigh)
                {
                    continue;
                }
                else if (workFlowName == ACCEPTED)
                {
                    long xScore = xHigh - xLow + 1;
                    long mScore = mHigh - mLow + 1;
                    long aScore = aHigh - aLow + 1;
                    long hScore = sHigh - sLow + 1;
                    sum += xScore * mScore * aScore * hScore;
                    continue;
                }
                else if (workFlowName == REJECTED)
                {
                    continue;
                }
                else
                {
                    var workflow = _workflows[workFlowName];
                    foreach (var rule in workflow.Rules)
                    {
                        if (rule.IsSpecialRule)
                        {
                            queue.Enqueue((rule.SendTo, xLow, xHigh, mLow, mHigh, aLow, aHigh, sLow, sHigh));
                            break;
                        }
                        else
                        {
                            var ranges = CalculateRanges(rule.Name, rule.Operator, rule.Amount, xLow, xHigh, mLow, mHigh, aLow, aHigh, sLow, sHigh);
                            queue.Enqueue((rule.SendTo, ranges.xLow, ranges.xHigh, ranges.mLow, ranges.mHigh, ranges.aLow, ranges.aHigh, ranges.sLow, ranges.sHigh));
                            (xLow, xHigh, mLow, mHigh, aLow, aHigh, sLow, sHigh) = CalculateRanges(rule.Name, rule.Operator == ">" ? "<=" : ">=", rule.Amount, xLow, xHigh, mLow, mHigh, aLow, aHigh, sLow, sHigh);
                        }
                    }
                }
            }

            return sum;
        }

        private (long xLow, long xHigh, long mLow, long mHigh, long aLow, long aHigh, long sLow, long sHigh) CalculateRanges(string name, string @operator, int amount, long xLow, long xHigh, long mLow, long mHigh, long aLow, long aHigh, long sLow, long sHigh)
        {
            switch (name)
            {
                case "x":
                    (xLow, xHigh) = CalculateLowAndHigh(@operator, amount, xLow, xHigh);
                    break;
                case "m":
                    (mLow, mHigh) = CalculateLowAndHigh(@operator, amount, mLow, mHigh);
                    break;
                case "a":
                    (aLow, aHigh) = CalculateLowAndHigh(@operator, amount, aLow, aHigh);
                    break;
                case "s":
                    (sLow, sHigh) = CalculateLowAndHigh(@operator, amount, sLow, sHigh);
                    break;
                default:
                    throw new Exception();
            }

            return (xLow, xHigh, mLow, mHigh, aLow, aHigh, sLow, sHigh);
        }

        private (long low, long high) CalculateLowAndHigh(string @operator, int amount, long low, long high)
        {
            switch (@operator)
            {
                case ">":
                    low = Math.Max(low, amount + 1);
                    break;
                case "<":
                    high = Math.Min(high, amount - 1);
                    break;
                case ">=":
                    low = Math.Max(low, amount);
                    break;
                case "<=":
                    high = Math.Min(high, amount);
                    break;
            }

            return (low, high);
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
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }

                if (item.StartsWith("{"))
                {
                    break;
                }
                else
                {
                    var split = item.Split('{');
                    var name = split[0];
                    var rules = split[1][0..^1].Split(',');
                    Workflow workflow = new Workflow()
                    {
                        Name = name,
                        FullName = item
                    };

                    foreach (var ruleStr in rules)
                    {
                        var rule = new Rule()
                        {
                            RuleStr = ruleStr
                        };

                        if (ruleStr.Contains(">"))
                        {
                            var ruleSplit = ruleStr.Split(">");
                            var ruleName = ruleSplit[0];
                            var op = ">";
                            var ruleSplit2 = ruleSplit[1].Split(':');
                            var amount = int.Parse(ruleSplit2[0]);
                            var sendTo = ruleSplit2[1];

                            rule.Operator = op;
                            rule.Name = ruleName;
                            rule.Amount = amount;
                            rule.SendTo = sendTo;
                        }
                        else if (ruleStr.Contains("<"))
                        {
                            var ruleSplit = ruleStr.Split("<");
                            var ruleName = ruleSplit[0];
                            var op = "<";
                            var ruleSplit2 = ruleSplit[1].Split(':');
                            var amount = int.Parse(ruleSplit2[0]);
                            var sendTo = ruleSplit2[1];
                            rule.Operator = op;
                            rule.Name = ruleName;
                            rule.Amount = amount;
                            rule.SendTo = sendTo;
                        }
                        else
                        {
                            rule.Name = ruleStr;
                            rule.SendTo = ruleStr;
                        }

                        workflow.Rules.Add(rule);
                    }

                    _workflows.Add(workflow.Name, workflow);
                }
            }
        }
    }
}