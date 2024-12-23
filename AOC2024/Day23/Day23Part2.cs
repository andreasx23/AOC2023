using System.Diagnostics;
using System.Text;

namespace AOC2024.Day23
{
    public class Day23Part2
    {
        private static readonly bool _useTestData = true;
        private static readonly string _className = "Day23";
        private readonly Dictionary<string, List<string>> _nodes = new();

        private string Solve(Stopwatch watch)
        {
            string sum = string.Empty;
            var best = int.MinValue;
            for (int i = 3; i <= _nodes.Count; i++)
            {
                var results = new HashSet<string>();
                foreach (var key in _nodes.Keys)
                {
                    foreach (var value in _nodes[key])
                    {
                        DFS(key, value, i, new List<string>() { key }, results, new HashSet<string>());
                    }
                }

                if (results.Count > best)
                {
                    best = results.Count;

                    Dictionary<string, int> temp = new Dictionary<string, int>();
                    foreach (var result in results)
                    {
                        for (int j = 1; j < result.Length; j += 2)
                        {
                            var prev = result[j - 1];
                            var current = result[j];
                            var key = $"{prev}{current}";
                            if (!temp.TryGetValue(key, out var value))
                            {
                                temp.Add(key, 1);
                            }
                            else
                            {
                                temp[key] += 1;
                            }
                        }
                    }

                    var top4 = temp.OrderByDescending(x => x.Value)
                                   .Take(4)
                                   .Select(x => x.Key)
                                   .OrderBy(x => x)
                                   .ToList();
                    sum = string.Join(",", top4);
                    Console.WriteLine($"Best: {i} -- {sum}");
                }
            }

            return sum;
        }

        private void DFS(string initial, string current, int targetDepth, List<string> path, HashSet<string> results, HashSet<string> visited)
        {
            path.Add(current);

            if (path.Count == targetDepth)
            {
                if (_nodes[current].Contains(initial))
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in path.OrderBy(x => x))
                    {
                        sb.Append(item);
                    }

                    results.Add(sb.ToString());
                }

                path.RemoveAt(path.Count - 1);
                return;
            }

            visited.Add(current);

            if (_nodes.TryGetValue(current, out var childs))
            {
                foreach (var child in childs)
                {
                    if (initial == child)
                    {
                        continue;
                    }

                    if (!visited.Contains(child))
                    {
                        DFS(initial, child, targetDepth, path, results, visited);
                    }
                }
            }

            visited.Remove(current);
            path.RemoveAt(path.Count - 1);
        }

        public void Result()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            ReadData();
            var result = Solve(stopwatch);
            Console.WriteLine($"Your answer: {result} -- Took: {stopwatch.Elapsed}");
        }

        private void ReadData()
        {
            var lines = File.ReadAllLines(@$"{_className}\{(_useTestData ? "Test" : "Data")}.txt");
            var data = lines.ToList();
            foreach (var line in data)
            {
                var split = line.Split('-');
                if (!_nodes.TryGetValue(split.First(), out var firstChilds))
                {
                    _nodes[split.First()] = new List<string>()
                    {
                        split.Last()
                    };
                }
                else
                {
                    _nodes[split.First()].Add(split.Last());
                }

                if (!_nodes.TryGetValue(split.Last(), out var lastChilds))
                {
                    _nodes[split.Last()] = new List<string>()
                    {
                        split.First()
                    };
                }
                else
                {
                    _nodes[split.Last()].Add(split.First());
                }
            }
        }
    }
}
