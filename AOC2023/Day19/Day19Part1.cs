using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace AOC2023.Day19
{
    public class Day19Part1
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

            public bool IsSpecialRule => string.IsNullOrEmpty(Operator);
            public bool IsAccepted => IsSpecialRule && Name == "A"; // A = Accepted, R = Rejected
            public bool IsRejected => IsSpecialRule && Name == "R"; // A = Accepted, R = Rejected
            public bool IsGreaterThanOperator => !string.IsNullOrEmpty(Operator) && Operator == ">";

            public bool CheckIfValid(int partAmount)
            {
                return IsGreaterThanOperator ? Amount < partAmount : Amount > partAmount;
            }
        }

        class Part
        {
            public string Name { get; set; }
            public int ExtremelyGoodLooking { get; set; } // x
            public int Musical { get; set; } // m
            public int Aerodynamic { get; set; } // a
            public int Shiny { get; set; } // s
            public int Score => ExtremelyGoodLooking + Musical + Aerodynamic + Shiny;
        }

        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day19";
        private List<Workflow> _workflows = new();
        private List<Part> _parts = new();

        private const string START_WORKFLOW = "in";

        public long Solve(Stopwatch watch)
        {
            long sum = 0;

            foreach (var part in _parts)
            {
                //Console.WriteLine($"Attempting part: {part.Name}");

                var workflow = _workflows.First(x => x.Name == START_WORKFLOW);
                bool isDone = false;
                while (!isDone)
                {
                    bool isValid = true;
                    string sendToWorkFlow = "";
                    foreach (var rule in workflow.Rules)
                    {
                        isValid = true;

                        if (rule.IsSpecialRule)
                        {
                            if (rule.IsRejected)
                            {
                                isValid = false;
                                break;
                            }
                        }
                        else
                        {
                            isValid = rule.Name switch
                            {
                                "x" => rule.CheckIfValid(part.ExtremelyGoodLooking),
                                "m" => rule.CheckIfValid(part.Musical),
                                "a" => rule.CheckIfValid(part.Aerodynamic),
                                "s" => rule.CheckIfValid(part.Shiny),
                                _ => throw new Exception("Invalid"),
                            };
                        }

                        if (isValid)
                        {
                            sendToWorkFlow = rule.SendTo;
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(sendToWorkFlow) || sendToWorkFlow == "R")
                    {
                        isValid = false;
                    }

                    if (isValid)
                    {
                        if (sendToWorkFlow == "A")
                        {
                            sum += part.Score;
                            break;
                        }
                        else
                        {
                            workflow = _workflows.First(x => x.Name == sendToWorkFlow);
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                //Console.WriteLine($"Current sum: {sum}");
            }

            return sum;
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
                    var split = item[1..^1].Split(',').Select(row => int.Parse(row.Split('=').Last().Trim())).ToList();
                    _parts.Add(new Part()
                    {
                        Name = item,
                        ExtremelyGoodLooking = split[0],
                        Musical = split[1],
                        Aerodynamic = split[2],
                        Shiny = split[3]
                    });
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
                        var rule = new Rule
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

                    _workflows.Add(workflow);
                }
            }
        }
    }
}