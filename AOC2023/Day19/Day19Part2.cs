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
        private List<Part> _parts = new();

        private const string START_WORKFLOW = "in";

        public long Solve(Stopwatch watch)
        {
            long sum = 0;

            sum = Bfs();

            return sum;
        }

        // 788846272
        // 1361750208
        // 2073604890
        // 1410324064
        // 376801888
        // 167409079868000
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
                    sum += (xh - xl + 1)
                           * (mh - ml + 1)
                           * (ah - al + 1)
                           * (sh - sl + 1);
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
                                    //if (rule.CheckIfValid(xl, xh))
                                    //{
                                    //    var (min, max) = CalculateMinMax(rule.Operator, rule.Amount, xl, xh);
                                    //    queue.Enqueue((rule.SendTo, min, max, ml, mh, al, ah, sl, sh));

                                    //    (min, max) = CalculateMinMax(rule.Operator == ">" ? "<=" : ">=", rule.Amount, xl, xh);
                                    //    xl = min;
                                    //    xh = max;
                                    //}
                                    break;
                                case "m":
                                    //if (rule.CheckIfValid(ml, mh))
                                    //{
                                    //    var (min, max) = CalculateMinMax(rule.Operator, rule.Amount, ml, mh);
                                    //    queue.Enqueue((rule.SendTo, xl, xh, min, max, al, ah, sl, sh));

                                    //    (min, max) = CalculateMinMax(rule.Operator == ">" ? "<=" : ">=", rule.Amount, ml, mh);
                                    //    ml = min;
                                    //    mh = max;
                                    //}
                                    {
                                        var (min, max) = CalculateMinMax(rule.Operator, rule.Amount, ml, mh);
                                        queue.Enqueue((rule.SendTo, xl, xh, min, max, al, ah, sl, sh));

                                        (min, max) = CalculateMinMax(rule.Operator == ">" ? "<=" : ">=", rule.Amount, ml, mh);
                                        ml = min;
                                        mh = max;
                                    }
                                    break;
                                case "a":
                                    //if (rule.CheckIfValid(al, ah))
                                    //{
                                    //    var (min, max) = CalculateMinMax(rule.Operator, rule.Amount, al, ah);
                                    //    queue.Enqueue((rule.SendTo, xl, xh, ml, mh, min, max, sl, sh));

                                    //    (min, max) = CalculateMinMax(rule.Operator == ">" ? "<=" : ">=", rule.Amount, al, ah);
                                    //    al = min;
                                    //    ah = max;
                                    //}
                                    {
                                        var (min, max) = CalculateMinMax(rule.Operator, rule.Amount, al, ah);
                                        queue.Enqueue((rule.SendTo, xl, xh, ml, mh, min, max, sl, sh));

                                        (min, max) = CalculateMinMax(rule.Operator == ">" ? "<=" : ">=", rule.Amount, al, ah);
                                        al = min;
                                        ah = max;
                                    }
                                    break;
                                case "s":
                                    //if (rule.CheckIfValid(sl, sh))
                                    //{
                                    //    var (min, max) = CalculateMinMax(rule.Operator, rule.Amount, sl, sh);
                                    //    queue.Enqueue((rule.SendTo, xl, xh, ml, mh, sl, sh, min, max));

                                    //    (min, max) = CalculateMinMax(rule.Operator == ">" ? "<=" : ">=", rule.Amount, sl, sh);
                                    //    sl = min;
                                    //    sh = max;
                                    //}
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

        private (int min, int max) CalculateMinMax(string @operator, int amount, int min, int max)
        {
            switch (@operator)
            {
                case ">":
                    min = Math.Max(min, amount + 1);
                    break;
                case "<":
                    max = Math.Min(max, amount - 1);
                    break;
                case ">=":
                    min = Math.Max(min, amount);
                    break;
                case "<=":
                    max = Math.Min(max, amount);
                    break;
            }

            return (min, max);
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
                        Name = name
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