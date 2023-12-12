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
                    Requirements = new List<int>(Requirements)
                };
            }
        }

        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day12";
        private List<Operation> _data = new();
        private const char OPERATIONAL = '.';
        private const char BROKEN = '#';
        private const char UNKNOWN = '?';

        public long Solve(Stopwatch watch)
        {
            var sum = 0L;

            object @lock = new object();
            Parallel.ForEach(_data, operation =>
            {
                List<string> permutations = new List<string>();
                GeneratePermutations(operation, 0, 0, permutations);

                var localSum = 0;
                foreach (var permutation in permutations)
                {
                    var pointer = 0;
                    var requirement = operation.Requirements[pointer];
                    int cnt = 0;
                    var isValid = true;
                    foreach (var c in permutation)
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
                        localSum++;
                    }
                }

                lock (@lock)
                {
                    sum += localSum;
                }
            });

            return sum;
        }

        private void GeneratePermutations(Operation operation, int index, int requirementIndex, List<string> permutations)
        {
            if (index >= operation.Row.Count || requirementIndex >= operation.Requirements.Count)
            {
                permutations.Add(operation.GetRowAsStr());
                return;
            }

            var currentChar = operation.Row[index];

            if (currentChar == UNKNOWN)
            {
                operation.Row[index] = OPERATIONAL;
                GeneratePermutations(operation.Clone(), index + 1, requirementIndex, permutations);

                operation.Row[index] = BROKEN;
                GeneratePermutations(operation.Clone(), index + 1, requirementIndex, permutations);
            }
            else
            {
                GeneratePermutations(operation, index + 1, requirementIndex, permutations);
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
            var lines = File.ReadAllLines(@$"{_className}\{(_useTestData ? "Test" : "Data")}.txt");

            foreach (var item in lines)
            {
                var splits = item.Split(' ');
                var operation = new Operation()
                {
                    Row = splits[0].Select(x => x).ToList(),
                    Requirements = splits[1].Trim().Split(',').Select(x => int.Parse(x.Trim())).ToList()
                };
                _data.Add(operation);
            }
        }
    }
}