using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day12
{
    public class Day12Part2
    {
        class Operation
        {
            public List<char> Row { get; set; } = new();
            public List<int> Requirements { get; set; } = new();

            public string GetRowAsStr()
            {
                return new string(Row.ToArray());
            }

            public Operation Clone()
            {
                return new Operation()
                {
                    Row = new List<char>(Row),
                    Requirements = Requirements
                };
            }
        }

        private static readonly bool _useTestData = true;
        private static readonly string _className = "Day12";
        private List<Operation> _data = new();
        private const char OPERATIONAL = '.';
        private const char BROKEN = '#';
        private const char UNKNOWN = '?';
        private object _lock = new object();
        private long _sum = 0;

        public long Solve(Stopwatch watch)
        {
            Parallel.ForEach(_data, operation =>
            {
                Console.WriteLine($"[{watch.Elapsed}] running: {operation.GetRowAsStr()} {string.Join(", ", operation.Requirements)}");
                GeneratePermutations(operation.Clone(), 0, 0);
                Console.WriteLine($"[{watch.Elapsed}] done running: {operation.GetRowAsStr()}");
            });

            return _sum;
        }

        private void GeneratePermutations(Operation operation, int index, int requirementIndex)
        {
            if (index >= operation.Row.Count || requirementIndex >= operation.Requirements.Count)
            {
                VerifyIfStringIsValid(operation);
                return;
            }

            var currentChar = operation.Row[index];

            if (currentChar == UNKNOWN)
            {
                operation.Row[index] = OPERATIONAL;
                GeneratePermutations(operation.Clone(), index + 1, requirementIndex);

                operation.Row[index] = BROKEN;
                GeneratePermutations(operation.Clone(), index + 1, requirementIndex);
            }
            else
            {
                GeneratePermutations(operation, index + 1, requirementIndex);
            }
        }

        private void VerifyIfStringIsValid(Operation operation)
        {
            var pointer = 0;
            var requirement = operation.Requirements[pointer];
            int cnt = 0;
            var isValid = true;
            foreach (var c in operation.Row)
            {
                if (c == '#')
                {
                    cnt++;
                }
                else
                {
                    if (cnt > 0)
                    {
                        if (cnt == requirement)
                        {
                            cnt = 0;
                            pointer++;
                            requirement = pointer < operation.Requirements.Count ? operation.Requirements[pointer] : int.MaxValue;
                        }
                        else
                        {
                            isValid = false;
                            break;
                        }
                    }
                }
            }

            if (isValid && ((pointer == operation.Requirements.Count && cnt == 0) || (pointer + 1 == operation.Requirements.Count && cnt == requirement)))
            {
                lock (_lock)
                {
                    _sum++;
                }
            }
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
            var lines = File.ReadAllLines(@$"{_className}\{(_useTestData ? "Test2" : "Data")}.txt");

            foreach (var item in lines)
            {
                var splits = item.Split(' ');

                var row = splits[0].Select(x => x).ToList();
                var str = "";

                var requirements = splits[1].Trim().Split(',').Select(x => int.Parse(x.Trim())).ToList();
                var reqs = new List<int>();
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < row.Count; j++)
                    {
                        str += row[j].ToString();
                    }

                    str += "?";

                    for (int j = 0; j < requirements.Count; j++)
                    {
                        reqs.Add(requirements[j]);
                    }
                }

                var operation = new Operation()
                {
                    Row = str.Select(x => x).ToList(),
                    Requirements = reqs
                };
                _data.Add(operation);
            }
        }
    }
}