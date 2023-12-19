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

            public int Min { get; set; }
            public int Max { get; set; }

            public bool IsSpecialRule => string.IsNullOrEmpty(Operator);
            public bool IsAccepted => IsSpecialRule && Name == "A"; // A = Accepted, R = Rejected
            public bool IsRejected => IsSpecialRule && Name == "R"; // A = Accepted, R = Rejected
            public bool IsGreaterThanOperator => !string.IsNullOrEmpty(Operator) && Operator == ">";

            public void SetupRange()
            {
                if (IsSpecialRule)
                {
                    return;
                }

                if (Operator == ">")
                {
                    Min = Amount;
                    Max = 4000;
                }
                else
                {
                    Min = 1;
                    Max = Amount;
                }
            }

            public bool CheckIfValid(int min, int max)
            {
                //return Min <= min && max <= Max;
                return min <= Min && Max <= max;
            }
        }

        class Part
        {
            public string Name { get; set; }
            public int ExtremelyGoodLookingMin { get; } = 1; // x
            public int ExtremelyGoodLookingMax { get; set; } // x
            public int MusicalMin { get; } = 1; // m
            public int MusicalMax { get; set; } // m
            public int AerodynamicMin { get; } = 1; // a
            public int AerodynamicMax { get; set; } // a
            public int ShinyMin { get; } = 1; // s
            public int ShinyMax { get; set; } // s

            public long Score => (ExtremelyGoodLookingMax - ExtremelyGoodLookingMin + 1)
                                 * (MusicalMax - MusicalMin + 1)
                                 * (AerodynamicMax - AerodynamicMin + 1)
                                 * (ShinyMax - ShinyMin + 1);
        }

        private static readonly bool _useTestData = true;
        private static readonly string _className = "Day19";
        private Dictionary<string, Workflow> _workflows = new();

        private const string START_WORKFLOW = "in";

        public long Solve(Stopwatch watch)
        {
            long sum = 0;

            sum = Test();
            Console.WriteLine(Bfs());

            return sum;
        }

        // 788846272
        // 1361750208
        // 2073604890
        // 1410324064
        // 376801888
        // 1743374336
        // -8383445120
        // 1039244853168000
        // 128299639868000
        // 167409079868000
        private long Test()
        {
            Queue<(string, int, int, int, int, int, int, int, int)> queue = new();
            queue.Enqueue(("in", 1, 4000, 1, 4000, 1, 4000, 1, 4000));

            long ans = 0;
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var key = current.Item1;
                var xl = current.Item2;
                var xh = current.Item3;
                var ml = current.Item4;
                var mh = current.Item5;
                var al = current.Item6;
                var ah = current.Item7;
                var sl = current.Item8;
                var sh = current.Item9;

                if (xl > xh || ml > mh || al > ah || sl > sh)
                {
                    continue;
                }
                else if (key == "A")
                {
                    long s1 = xh - xl + 1;
                    long s2 = mh - ml + 1;
                    long s3 = ah - al + 1;
                    long s4 = sh - sl + 1;
                    ans += s1 * s2 * s3 * s4;
                    continue;
                }
                else if (key == "R")
                {
                    continue;
                }
                else
                {
                    var workflow = _workflows[key];
                    foreach (var rule in workflow.FullName[(workflow.Name.Length + 1)..^1].Split(','))
                    {
                        var name = rule;
                        if (rule.Contains(':'))
                        {
                            var parts = rule.Split(':');
                            name = parts.Last();
                            var cond = parts[0];
                            var var = cond[0].ToString();
                            var op = cond[1].ToString();
                            var n = int.Parse(cond[2..]);

                            queue.Enqueue((name, xl, xh, ml, mh, al, ah, sl, sh));
                            (xl, xh, ml, mh, al, ah, sl, sh) = NewRanges(var, op == ">" ? "<=" : ">=", n, xl, xh, ml, mh, al, ah, sl, sh);
                        }
                        else
                        {
                            queue.Enqueue((name, xl, xh, ml, mh, al, ah, sl, sh));
                            break;
                        }
                    }
                }
            }

            return ans;
        }

        public static (int, int, int, int, int, int, int, int) NewRanges(string var, string op, int n, int xl, int xh, int ml, int mh, int al, int ah, int sl, int sh)
        {
            if (var == "x")
            {
                (xl, xh) = NewRange(op, n, xl, xh);
            }
            else if (var == "m")
            {
                (ml, mh) = NewRange(op, n, ml, mh);
            }
            else if (var == "a")
            {
                (al, ah) = NewRange(op, n, al, ah);
            }
            else if (var == "s")
            {
                (sl, sh) = NewRange(op, n, sl, sh);
            }

            return (xl, xh, ml, mh, al, ah, sl, sh);
        }

        public static (int, int) NewRange(string op, int n, int lo, int hi)
        {
            if (op == ">")
            {
                lo = Math.Max(lo, n + 1);
            }
            else if (op == "<")
            {
                hi = Math.Min(hi, n - 1);
            }
            else if (op == ">=")
            {
                lo = Math.Max(lo, n);
            }
            else if (op == "<=")
            {
                hi = Math.Min(hi, n);
            }
            else
            {
                throw new ArgumentException("Invalid operation");
            }

            return (lo, hi);
        }

        private long Bfs()
        {
            Queue<(string workFlowName, int xl, int xh, int ml, int mh, int al, int ah, int sl, int sh)> queue = new();
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
                            break;
                        }
                        else
                        {
                            switch (rule.Name)
                            {
                                case "x":
                                    {
                                        var (min, max) = CalculateMinMax(rule.Operator, rule.Amount, xl, xh);
                                        queue.Enqueue((rule.SendTo, min, max, ml, mh, al, ah, sl, sh));

                                        (min, max) = CalculateMinMax(rule.Operator == ">" ? "<=" : ">=", rule.Amount, xl, xh);
                                        xl = min;
                                        xh = max;
                                    }
                                    break;
                                case "m":
                                    {
                                        var (min, max) = CalculateMinMax(rule.Operator, rule.Amount, ml, mh);
                                        queue.Enqueue((rule.SendTo, xl, xh, min, max, al, ah, sl, sh));

                                        (min, max) = CalculateMinMax(rule.Operator == ">" ? "<=" : ">=", rule.Amount, ml, mh);
                                        ml = min;
                                        mh = max;
                                    }
                                    break;
                                case "a":
                                    {
                                        var (min, max) = CalculateMinMax(rule.Operator, rule.Amount, al, ah);
                                        queue.Enqueue((rule.SendTo, xl, xh, ml, mh, min, max, sl, sh));

                                        (min, max) = CalculateMinMax(rule.Operator == ">" ? "<=" : ">=", rule.Amount, al, ah);
                                        al = min;
                                        ah = max;
                                    }
                                    break;
                                case "s":
                                    {
                                        var (min, max) = CalculateMinMax(rule.Operator, rule.Amount, sl, sh);
                                        queue.Enqueue((rule.SendTo, xl, xh, ml, mh, sl, sh, min, max));

                                        (min, max) = CalculateMinMax(rule.Operator == ">" ? "<=" : ">=", rule.Amount, sl, sh);
                                        sl = min;
                                        sh = max;
                                    }
                                    break;
                                default:
                                    throw new Exception();
                            }
                        }
                    }
                }
            }

            return sum;
        }

        private (int min, int max) CalculateMinMax(string @operator, int amount, int low, int high)
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

                        rule.SetupRange();

                        workflow.Rules.Add(rule);
                    }

                    _workflows.Add(workflow.Name, workflow);
                }
            }
        }
    }
}