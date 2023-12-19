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

        public long Solve(Stopwatch watch)
        {
            long sum = 0;

            sum = Bfs();

            return sum;
        }

        private long Bfs()
        {
            Queue<(string workFlowName, long xl, long xh, long ml, long mh, long al, long ah, long sl, long sh)> queue = new();
            queue.Enqueue((START_WORKFLOW, 1, 4000, 1, 4000, 1, 4000, 1, 4000));

            long sum = 0;
            while (queue.Count > 0)
            {
                var (workFlowName, xl, xh, ml, mh, al, ah, sl, sh) = queue.Dequeue();

                if (xl > xh || ml > mh || al > ah || sl > sh)
                {
                    continue;
                }
                else if (workFlowName == "A")
                {
                    long s1 = xh - xl + 1;
                    long s2 = mh - ml + 1;
                    long s3 = ah - al + 1;
                    long s4 = sh - sl + 1;
                    sum += s1 * s2 * s3 * s4;
                    continue;
                }
                else if (workFlowName == "R")
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
                            queue.Enqueue((rule.SendTo, xl, xh, ml, mh, al, ah, sl, sh));
                        }
                        else
                        {
                            var ranges = CalculateRanges(rule.Name, rule.Operator, rule.Amount, xl, xh, ml, mh, al, ah, sl, sh);
                            queue.Enqueue((rule.SendTo, ranges.xl, ranges.xh, ranges.ml, ranges.mh, ranges.al, ranges.ah, ranges.sl, ranges.sh));
                            (xl, xh, ml, mh, al, ah, sl, sh) = CalculateRanges(rule.Name, rule.Operator == ">" ? "<=" : ">=", rule.Amount, xl, xh, ml, mh, al, ah, sl, sh);
                        }
                    }
                }
            }

            return sum;
        }

        private (long xl, long xh, long ml, long mh, long al, long ah, long sl, long sh) CalculateRanges(string name, string @operator, int amount, long xl, long xh, long ml, long mh, long al, long ah, long sl, long sh)
        {
            switch (name)
            {
                case "x":
                    (xl, xh) = CalculateLowAndHigh(@operator, amount, xl, xh);
                    break;
                case "m":
                    (ml, mh) = CalculateLowAndHigh(@operator, amount, ml, mh);
                    break;
                case "a":
                    (al, ah) = CalculateLowAndHigh(@operator, amount, al, ah);
                    break;
                case "s":
                    (sl, sh) = CalculateLowAndHigh(@operator, amount, sl, sh);
                    break;
                default:
                    throw new Exception();
            }

            return (xl, xh, ml, mh, al, ah, sl, sh);
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
                    //var split = item[1..^1].Split(',').Select(row => int.Parse(row.Split('=').Last().Trim())).ToList();
                    //_parts.Add(new Part()
                    //{
                    //    Name = item,
                    //    ExtremelyGoodLooking = split[0],
                    //    Musical = split[1],
                    //    Aerodynamic = split[2],
                    //    Shiny = split[3]
                    //});
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