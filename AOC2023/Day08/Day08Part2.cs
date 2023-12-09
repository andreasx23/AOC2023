using AOC2023.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day08
{
    public class Day08Part2
    {
        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day08";
        private List<string> _directions = new();
        private Dictionary<string, List<string>> _nodes = new();
        private const char START = 'A';
        private const char GOAL = 'Z';

        public long Solve(Stopwatch watch)
        {
            var sum = 0L;

            Queue<(string key, List<string> childs, string previousSeenKey)> queue = new();
            var startNodes = _nodes.Where(x => x.Key.Last() == START);
            var totalAmountOfNodes = startNodes.Count();
            foreach (var item in startNodes)
            {
                queue.Enqueue((item.Key, item.Value, ""));
            }

            var isSolved = false;
            var pointer = 0;
            while (!isSolved)
            {
                Queue<(string key, List<string> childs, string previousSeenKey)> currentQueue = new();
                var currentDirection = _directions[pointer];
                bool isDone = true;
                while (queue.Any())
                {
                    var currentNode = queue.Dequeue();
                    var key = GetNextKey(currentDirection, currentNode.childs);
                    currentQueue.Enqueue((key, _nodes[key], currentNode.key));
                    if (isDone && key.Last() != GOAL)
                    {
                        isDone = false;
                    }
                }

                sum++;
                pointer = (pointer + 1) % _directions.Count;

                if (watch.Elapsed.TotalMinutes >= 1)
                {
                    Console.WriteLine($"{watch.Elapsed}");
                    return sum;
                }

                if (isDone)
                {
                    isDone = true;
                }

                queue = currentQueue;
            }

            return sum;
        }

        public long Solve2(Stopwatch watch)
        {
            var startNodes = _nodes.Where(x => x.Key.Last() == START);

            List<long> steps = new();
            foreach (var startNode in startNodes)
            {
                var childs = startNode.Value;
                bool isFound = false;
                var sum = 0L;
                while (!isFound)
                {
                    foreach (var direction in _directions)
                    {
                        var key = GetNextKey(direction, childs);
                        childs = _nodes[key];
                        sum++;

                        if (key.Last() == GOAL)
                        {
                            isFound = true;
                            break;
                        }
                    }
                }

                steps.Add(sum);
            }

            return steps.LeastCommonMultiple();
        }

        private string GetNextKey(string direction, List<string> childs)
        {
            return direction == "L" ? childs[0] : childs[1];
        }

        public void Result()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            ReadData();
            var result = Solve2(stopwatch);
            Console.WriteLine($"Your answer: {result} -- Took: {stopwatch.Elapsed}");
        }

        public void ReadData()
        {
            var lines = File.ReadAllLines(@$"{_className}\{(_useTestData ? "Test3" : "Data")}.txt");

            _directions = lines[0].Select(x => x.ToString()).ToList();

            lines = lines.Skip(2).ToArray();

            foreach (var item in lines)
            {
                var split = item.Split('=').Select(x => x.Trim()).ToList();
                var node = split[0];
                if (_nodes.ContainsKey(node))
                {
                    throw new Exception();
                }
                var childs = split[1].Substring(1, split[1].Length - 2).Split(',').Select(x => x.Trim()).ToList();
                _nodes[node] = childs;
            }
        }
    }
}