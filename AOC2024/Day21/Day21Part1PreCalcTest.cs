using System.Diagnostics;
using System.Text;

namespace AOC2024.Day21
{
    public class Day21Part1PreCalcTest
    {
        private static readonly bool _useTestData = true;
        private static readonly string _className = "Day21";
        private List<string> _data = new();

        private readonly char[][] _numericKeypad = [
        ['7', '8', '9'],
        ['4', '5', '6'],
        ['1', '2', '3'],
        [' ', '0', 'A'],
        ];

        private readonly char[][] _directionalKeypad = [
        [' ', '^', 'A'],
        ['<', 'v', '>'],
        ];

        private readonly List<(int x, int y, char direction)> _directions = new(4)
        {
            (0, 1, '>'),
            (0, -1, '<'),
            (1, 0, 'v'),
            (-1, 0, '^')
        };

        private long Solve(Stopwatch watch)
        {
            var sum = 0L;

            var numericKeypadDict = GenerateFromToTargetDict(_numericKeypad);
            var directionalDict = GenerateFromToTargetDict(_directionalKeypad);
            foreach (var row in _data)
            {
                List<string> robot1Results = GenerateRobotResults(numericKeypadDict, row);

                List<string> allRobot2Results = [];
                foreach (var robot2 in robot1Results)
                {
                    var robot2Results = Bfs(0, 2, robot2, _directionalKeypad).Distinct().ToList();
                    var robot2Results2 = GenerateRobotResults(directionalDict, robot2);

                    //Console.WriteLine(string.Join(Environment.NewLine, robot2Results));
                    //Console.WriteLine(  );
                    //Console.WriteLine(string.Join(Environment.NewLine, robot2Results2));

                    allRobot2Results.AddRange(robot2Results2);
                }

                //List<string> allRobot3Results = [];
                //foreach (var robot3 in allRobot2Results)
                //{
                //    var robot3Results = GenerateRobotResults(directionalDict, robot3);
                //    allRobot3Results.AddRange(robot3Results);
                //}

                //var min = allRobot3Results.Min(x => x.Length);
                //sum += int.Parse(row[..3]) * allRobot3Results.Min(x => x.Length);

                break;
            }

            return sum;
        }

        private static List<string> GenerateRobotResults(Dictionary<(char current, char target), List<string>> numericKeypadDict, string row)
        {
            List<string> robotResults = [];
            var fromRobotChar = 'A';
            var isFirstRun = true;
            foreach (var targetChar in row)
            {
                var key = (fromRobotChar, targetChar);
                var shortestPaths = numericKeypadDict[key];
                fromRobotChar = targetChar;
                if (isFirstRun)
                {
                    isFirstRun = false;
                    robotResults.AddRange(shortestPaths);
                }
                else if (robotResults.Count == shortestPaths.Count)
                {
                    for (int i = 0; i < robotResults.Count; i++)
                    {
                        robotResults[i] += shortestPaths[i];
                    }
                }
                else if (shortestPaths.Count > robotResults.Count)
                {
                    List<string> temp = new List<string>();
                    for (int i = 0; i < shortestPaths.Count; i++)
                    {
                        for (int j = 0; j < robotResults.Count; j++)
                        {
                            temp.Add($"{robotResults[j]}{shortestPaths[i]}");
                        }
                    }
                    robotResults = temp;
                }
                else
                {
                    List<string> temp = new List<string>();
                    for (int i = 0; i < robotResults.Count; i++)
                    {
                        for (int j = 0; j < shortestPaths.Count; j++)
                        {
                            temp.Add($"{robotResults[i]}{shortestPaths[j]}");
                        }
                    }
                    robotResults = temp;
                }
            }

            return robotResults;
        }

        private Dictionary<(char current, char target), List<string>> GenerateFromToTargetDict(char[][] grid)
        {
            Dictionary<(char current, char target), List<string>> fromToTargetDict = new();
            for (int x = 0; x < grid.Length; x++)
            {
                for (int y = 0; y < grid[x].Length; y++)
                {
                    for (int targetX = 0; targetX < grid.Length; targetX++)
                    {
                        for (int targetY = 0; targetY < grid[targetX].Length; targetY++)
                        {
                            if (grid[x][y] == ' ' || grid[targetX][targetY] == ' ')
                            {
                                continue;
                            }

                            var current = grid[x][y];
                            var target = grid[targetX][targetY];
                            var key = (current, target);
                            if (!fromToTargetDict.ContainsKey(key))
                            {
                                fromToTargetDict[key] = [];
                            }

                            if (x == targetX && y == targetY)
                            {
                                fromToTargetDict[key].Add("A");
                            }
                            else
                            {
                                var shortestPaths = FindShortestPaths(x, y, targetX, targetY, grid);
                                fromToTargetDict[key].AddRange(shortestPaths);
                            }
                        }
                    }
                }
            }

            return fromToTargetDict;
        }

        private List<string> FindShortestPaths(int x, int y, int targetX, int targetY, char[][] grid)
        {
            Queue<(int x, int y, int moves, HashSet<(int x, int y)> seen, int index, StringBuilder stringOfMoves)> queue = new();
            var seen = new HashSet<(int x, int y)>()
            {
                (x, y)
            };
            queue.Enqueue((x, y, 0, seen, 0, new StringBuilder()));

            int lowestMoveCount = int.MaxValue;
            int total = 0;
            List<StringBuilder> possibleMoves = new();
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current.x == targetX && current.y == targetY)
                {
                    current.seen.Clear();
                    current.stringOfMoves.Append("A");

                    if (lowestMoveCount > current.moves)
                    {
                        lowestMoveCount = current.moves;
                        total = 1;
                        possibleMoves.Clear();
                        possibleMoves.Add(current.stringOfMoves);
                    }
                    else if (lowestMoveCount == current.moves)
                    {
                        total++;
                        possibleMoves.Add(current.stringOfMoves);
                    }

                    continue;
                }

                foreach (var item in PossibleDirections(current.x, current.y, grid))
                {
                    if (current.seen.Add((item.x, item.y)))
                    {
                        var nextStringOfMoves = new StringBuilder(current.stringOfMoves.ToString());
                        nextStringOfMoves.Append(item.direction);
                        queue.Enqueue((item.x, item.y, current.moves + 1, new HashSet<(int x, int y)>(current.seen), current.index, nextStringOfMoves));
                    }
                }
            }

            return possibleMoves.Select(x => x.ToString()).ToList();
        }

        private List<string> Bfs(int x, int y, string target, char[][] grid)
        {
            PriorityQueue<(int x, int y, int moves, HashSet<(int x, int y)> seen, int index, StringBuilder stringOfMoves), int> queue = new();
            var seen = new HashSet<(int x, int y)>()
            {
                (x, y)
            };
            queue.Enqueue((x, y, 0, seen, 0, new StringBuilder()), 0);

            int lowestMoveCount = int.MaxValue;
            int total = 0;
            List<StringBuilder> possibleMoves = new();
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (grid[current.x][current.y] == target[current.index])
                {
                    current.seen.Clear();
                    current.stringOfMoves.Append("A");

                    if (current.index == target.Length - 1 && target[current.index] == 'A')
                    {
                        if (lowestMoveCount > current.moves)
                        {
                            lowestMoveCount = current.moves;
                            total = 1;
                            possibleMoves.Clear();
                            possibleMoves.Add(current.stringOfMoves);
                        }
                        else if (lowestMoveCount == current.moves)
                        {
                            total++;
                            possibleMoves.Add(current.stringOfMoves);
                        }

                        continue;
                    }

                    current.index++;
                    if (target[current.index - 1] == target[current.index])
                    {
                        queue.Enqueue((current.x, current.y, current.moves + 1, new HashSet<(int x, int y)>(current.seen), current.index, current.stringOfMoves), 0);
                    }
                }

                foreach (var item in PossibleDirections(current.x, current.y, grid))
                {
                    if (current.seen.Add((item.x, item.y)))
                    {
                        var nextStringOfMoves = new StringBuilder(current.stringOfMoves.ToString());
                        nextStringOfMoves.Append(item.direction);
                        queue.Enqueue((item.x, item.y, current.moves + 1, new HashSet<(int x, int y)>(current.seen), current.index, nextStringOfMoves), 0);
                    }
                }
            }

            return possibleMoves.Select(x => x.ToString()).ToList();
        }

        private List<(int x, int y, char direction)> PossibleDirections(int currentX, int currentY, char[][] grid)
        {
            List<(int x, int y, char direction)> possibleMoves = new(4);
            foreach (var (x, y, direction) in _directions)
            {
                var dx = x + currentX;
                var dy = y + currentY;
                if (dx < 0
                    || dx >= grid.Length
                    || dy < 0
                    || dy >= grid.First().Length
                    || (grid[dx][dy] == ' '))
                {
                    continue;
                }

                possibleMoves.Add((dx, dy, direction));
            }

            return possibleMoves;
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
            _data = lines.ToList();
        }
    }
}
