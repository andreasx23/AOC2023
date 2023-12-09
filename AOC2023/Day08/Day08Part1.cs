using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day08
{
    public class Day08Part1
    {
        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day08";
        private List<string> _directions = new();
        private Dictionary<string, List<string>> _nodes = new();
        private const string START = "AAA";
        private const string GOAL = "ZZZ";

        public long Solve()
        {
            var sum = 0;

            var childs = _nodes[START];
            bool isFound = false;
            while (!isFound)
            {
                foreach (var direction in _directions)
                {
                    var key = GetNextKey(direction, childs);
                    childs = _nodes[key];
                    sum++;

                    if (key == GOAL)
                    {
                        isFound = true;
                        break;
                    }
                }
            }

            return sum;
        }

        private string GetNextKey(string direction, List<string> childs)
        {
            return direction == "L" ? childs[0] : childs[1];
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
            var lines = File.ReadAllLines(@$"{_className}\{(_useTestData ? "Test2" : "Data")}.txt");

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