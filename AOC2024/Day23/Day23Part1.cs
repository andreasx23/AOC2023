using System.Diagnostics;
using System.Text;

namespace AOC2024.Day23
{
    public class Day23Part1
    {
        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day23";
        private readonly Dictionary<string, List<string>> _nodes = new();

        private long Solve(Stopwatch watch)
        {
            var results = new HashSet<string>();
            foreach (var key in _nodes.Keys)
            {
                foreach (var value in _nodes[key])
                {
                    DFS(key, value, 3, new List<string>() { key }, results, new HashSet<string>());
                }
            }

            return results.Sum(x => (x[0] == 't' || x[2] == 't' || x[4] == 't') ? 1 : 0);
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
