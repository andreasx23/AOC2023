using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023.Day02
{
    internal class Day02Part1
    {
        private enum CubeColour
        {
            BLUE,
            RED,
            GREEN
        }

        private class Cube
        {
            public CubeColour CubeColour { get; private set; }
            public int Amount { get; private set; }

            public Cube(CubeColour cubeColour, int amount)
            {
                CubeColour = cubeColour;
                Amount = amount;
            }
        }

        private class Game
        {
            public int GameId { get; private set; }
            public Dictionary<int, List<Cube>> Sets { get; private set; }

            public Game(int gameId)
            {
                GameId = gameId;
                Sets = new Dictionary<int, List<Cube>>();
            }
        }

        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day02";
        private List<Game> _data = new();

        public int Solve()
        {
            const int MAX_RED = 12;
            const int MAX_GREEN = 13;
            const int MAX_BLUE = 14;

            var sum = 0;
            foreach (var game in _data)
            {
                bool isPossible = true;
                foreach (var set in game.Sets)
                {
                    foreach (var cube in set.Value)
                    {
                        switch (cube.CubeColour)
                        {
                            case CubeColour.BLUE:
                                if (cube.Amount > MAX_BLUE)
                                {
                                    isPossible = false;
                                    break;
                                }
                                break;
                            case CubeColour.RED:
                                if (cube.Amount > MAX_RED)
                                {
                                    isPossible = false;
                                    break;
                                }
                                break;
                            case CubeColour.GREEN:
                                if (cube.Amount > MAX_GREEN)
                                {
                                    isPossible = false;
                                    break;
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    if (!isPossible)
                    {
                        break;
                    }
                }

                if (isPossible)
                {
                    sum += game.GameId;
                }
            }

            return sum;
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
            var lines = File.ReadAllLines(@$"..\..\..\{_className}\{(_useTestData ? "Test" : "Data")}.txt");
            int gameId = 1;
            foreach (var line in lines)
            {
                var split = line.Split(':');
                var sets = split.Last().Split(';').Select(x => x.Trim()).ToList();
                Game game = new Game(gameId);
                int setId = 1;
                foreach (var set in sets)
                {
                    var setData = set.Split(',').Select(x => x.Trim()).ToList();
                    List<Cube> cubes = new List<Cube>();
                    foreach (var item in setData)
                    {
                        var test = item.Split(' ').Select(x => x.Trim()).ToList();
                        var amount = int.Parse(test.First());
                        var color = CubeColour.BLUE;
                        switch (test.Last())
                        {
                            case "blue":
                                color = CubeColour.BLUE;
                                break;
                            case "red":
                                color = CubeColour.RED;
                                break;
                            case "green":
                                color = CubeColour.GREEN;
                                break;
                        }
                        var cube = new Cube(color, amount);
                        cubes.Add(cube);
                    }
                    game.Sets.Add(setId, cubes);
                    setId++;
                }
                _data.Add(game);
                gameId++;
            }
        }
    }
}
