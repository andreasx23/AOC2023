using Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AOC2023.Day25
{
    public class Day25Part1
    {
        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day25";
        private Dictionary<string, List<string>> _data = new();
        private Dictionary<string, HashSet<string>> _dataHashset = new();
        private HashSet<string> _allNames = new();

        public long Solve(Stopwatch watch)
        {
            long sum = 0;

            var names = _allNames.ToList();
            var n = names.Count;
            long runs = 0;
            object @lock = new();
            object @resultLock = new();

            ConcurrentDictionary<(int i1, int i2, int i3, int i4, int i5, int i6), bool> seen = new();

            Parallel.For(0, n, x =>
            {
                Random random = new Random();
                while (true)
                {
                    var i1 = random.Next(0, n);
                    var i2 = random.Next(0, n);
                    var i3 = random.Next(0, n);
                    var i4 = random.Next(0, n);
                    var i5 = random.Next(0, n);
                    var i6 = random.Next(0, n);

                    //if (!seen.TryAdd((i1, i2, i3, i4, i5, i6), true))
                    //{
                    //    continue;
                    //}

                    var name1 = names[i1];
                    var name2 = names[i2];
                    var name3 = names[i3];
                    var name4 = names[i4];
                    var name5 = names[i5];
                    var name6 = names[i6];

                    lock (@lock)
                    {
                        runs++;

                        if (runs % 10_000_000 == 0)
                        {
                            Console.WriteLine($"[{watch.Elapsed} -- {runs}] Current max: {sum} -- ({i1}: {name1}), ({i2}: {name2}), ({i3}: {name3}), ({i4}: {name4}), ({i5}: {name5}), ({i6}: {name6})");
                        }
                    }

                    var currentSum = Bfs((name1, name2), (name3, name4), (name5, name6));
                    if (currentSum > sum)
                    {
                        lock (resultLock)
                        {
                            Console.WriteLine($"[{watch.Elapsed} -- {runs}] New max: {currentSum} -- ({i1}: {name1}), ({i2}: {name2}), ({i3}: {name3}), ({i4}: {name4}), ({i5}: {name5}), ({i6}: {name6})");
                            sum = currentSum;
                        }
                    }
                }
            });

            //Parallel.For(0, n, i1 =>
            //{
            //var name1 = names[i1];
            //for (int i2 = i1 + 1; i2 < n; i2++)
            //{
            //    var name2 = names[i2];
            //    for (int i3 = i2 + 1; i3 < n; i3++)
            //    {
            //        var name3 = names[i3];
            //        for (int i4 = i3 + 1; i4 < n; i4++)
            //        {
            //            var name4 = names[i4];
            //            for (int i5 = i4 + 1; i5 < n; i5++)
            //            {
            //                var name5 = names[i5];
            //                for (int i6 = i5 + 1; i6 < n; i6++)
            //                {
            //                    var name6 = names[i6];

            //                    lock (@lock)
            //                    {
            //                        runs++;

            //                        if (runs % 20_000_000 == 0)
            //                        {
            //                            Console.WriteLine($"[{watch.Elapsed} -- {runs}] Current max: {sum} -- ({i1}: {name1}), ({i2}: {name2}), ({i3}: {name3}), ({i4}: {name4}), ({i5}: {name5}), ({i6}: {name6})");
            //                        }
            //                    }

            //                    var currentSum = Bfs((name1, name2), (name3, name4), (name5, name6));
            //                    if (currentSum > sum)
            //                    {
            //                        lock (@lock)
            //                        {
            //                            Console.WriteLine($"[{watch.Elapsed} -- {runs}] New max: {currentSum} -- ({i1}: {name1}), ({i2}: {name2}), ({i3}: {name3}), ({i4}: {name4}), ({i5}: {name5}), ({i6}: {name6})");
            //                            sum = currentSum;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //});

            return sum;
        }

        private bool CheckIfCutWire((string left, string right) wire, Dictionary<string, HashSet<string>> data)
        {
            var contain1 = data[wire.left].Contains(wire.right);
            var contain2 = data[wire.right].Contains(wire.left);
            return contain1 && contain2;
        }

        private int Bfs((string left, string right) wire1, (string left, string right) wire2, (string left, string right) wire3)
        {
            if (!CheckIfCutWire(wire1, _dataHashset) || !CheckIfCutWire(wire2, _dataHashset) || !CheckIfCutWire(wire3, _dataHashset))
            {
                return -1;
            }

            var data = CloneData();
            var removedWire1 = CutWire(wire1, data);
            var removedWire2 = CutWire(wire2, data);
            var removedWire3 = CutWire(wire3, data);
            if (!removedWire1.isValid || !removedWire2.isValid || !removedWire3.isValid)
            {
                return -1;
            }

            var leftSet = GenerateSet(removedWire1.name1, data, new());
            var rightSet = GenerateSet(removedWire1.name2, data, leftSet);

            if (rightSet is null)
            {
                return -1;
            }

            if (!leftSet.All(rightSet.Contains))
            {
                return leftSet.Count * rightSet.Count;
            }

            return -1;
        }

        private HashSet<string> GenerateSet(string name, Dictionary<string, List<string>> data, HashSet<string> otherSet)
        {
            Queue<string> queue = new();
            HashSet<string> seen = new();

            queue.Enqueue(name);
            seen.Add(name);

            while (queue.Count > 0)
            {
                var currentName = queue.Dequeue();
                var current = data[currentName];

                if (otherSet.Contains(currentName))
                {
                    return null;
                }

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

        private (string name1, string name2, bool isValid) CutWire((string left, string right) wire, Dictionary<string, List<string>> data)
        {
            var removed1 = data[wire.left].Remove(wire.right);
            var removed2 = data[wire.right].Remove(wire.left);

            if (removed1 && removed2)
            {
                return (wire.left, wire.right, true);
            }
            else if (removed1)
            {
                return ("", wire.right, false);
            }
            else if (removed2)
            {
                return (wire.left, "", false);
            }
            else
            {
                return ("", "", false);
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
            _dataHashset = newData.ToDictionary(x => x.Key, x => x.Value.ToHashSet());
        }
    }
}