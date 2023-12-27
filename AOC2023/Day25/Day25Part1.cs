using AOC2023.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day25
{
    public class Day25Part1
    {
        private static readonly bool _useTestData = true;
        private static readonly string _className = "Day25";
        private Dictionary<string, List<string>> _data = new();
        private HashSet<string> _allNames = new();

        public long Solve(Stopwatch watch)
        {
            long sum = 0;

            foreach (var kv1 in _data)
            {
                var key1 = kv1.Key;
                foreach (var kv2 in _data)
                {
                    var key2 = kv2.Key;
                    if (key1 == key2)
                    {
                        continue;
                    }

                    foreach (var kv3 in _data)
                    {
                        var key3 = kv3.Key;
                        if (key2 == key3)
                        {
                            continue;
                        }

                        foreach (var kv4 in _data)
                        {
                            var key4 = kv4.Key;
                            if (key3 == key4)
                            {
                                continue;
                            }

                            foreach (var kv5 in _data)
                            {
                                var key5 = kv5.Key;
                                if (key4 == key5)
                                {
                                    continue;
                                }

                                foreach (var kv6 in _data)
                                {
                                    var key6 = kv6.Key;
                                    if (key5 == key6)
                                    {
                                        continue;
                                    }

                                    var currentSum = Bfs((key1, key2), (key3, key4), (key5, key6));
                                    if (currentSum > sum)
                                    {
                                        sum = currentSum;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return sum;
        }

        private int Bfs((string left, string right) wire1, (string left, string right) wire2, (string left, string right) wire3)
        {
            var data = CloneData();

            var removedWire1 = CutWire(wire1, data);
            var removedWire2 = CutWire(wire2, data);
            var removedWire3 = CutWire(wire3, data);

            if (!removedWire1.isValid || !removedWire2.isValid || !removedWire3.isValid)
            {
                return -1;
            }

            var leftSet = Bfs(new List<string>() { removedWire1.names.First() }, data);
            var rightSet = Bfs(new List<string>() { removedWire1.names.Last() }, data);
            
            if (!leftSet.All(rightSet.Contains))
            {
                return leftSet.Count * rightSet.Count;
            }

            return -1;
        }

        private HashSet<string> Bfs(List<string> names, Dictionary<string, List<string>> data)
        {
            Queue<string> queue = new();
            HashSet<string> seen = new();

            foreach (var item in names)
            {
                queue.Enqueue(item);
                seen.Add(item);
            }

            while (queue.Count > 0)
            {
                var currentName = queue.Dequeue();
                var current = data[currentName];

                foreach (var child in current)
                {
                    if (seen.Add(child))
                    {
                        queue.Enqueue(child);
                    }
                }
            }

            return seen;
        }

        private Dictionary<string, List<string>> CloneData()
        {
            return _data.ToDictionary(x => x.Key, x => x.Value.ToList());
        }

        private (List<string> names, bool isValid) CutWire((string left, string right) wire, Dictionary<string, List<string>> data)
        {
            var removed1 = data[wire.left].Remove(wire.right);
            var removed2 = data[wire.right].Remove(wire.left);

            if (removed1 && removed2)
            {
                return (new List<string>() { wire.left, wire.right }, true);
            }
            else if (removed1)
            {
                return (new List<string>() { wire.right }, false);
            }
            else if (removed2)
            {
                return (new List<string>() { wire.left }, false);
            }
            else
            {
                return (new List<string>(), false);
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
                var split = item.Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
                _data[split[0]] = split[1].Split(' ').Select(x => x).ToList();
            }

            var newData = _data.ToDictionary(x => x.Key, x => x.Value.ToList());
            foreach (var item in _data)
            {
                foreach (var child in item.Value)
                {
                    if (!newData.ContainsKey(child))
                    {
                        newData[child] = new List<string>();
                    }

                    if (!newData[child].Contains(item.Key))
                    {
                        newData[child].Add(item.Key);
                    }
                }
            }

            foreach (var kv in newData)
            {
                _allNames.Add(kv.Key);
                foreach (var item in kv.Value)
                {
                    _allNames.Add(item);
                }
            }

            _data = newData;
        }
    }
}