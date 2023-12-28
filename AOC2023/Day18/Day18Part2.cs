using Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day18
{
    public class Day18Part2
    {
        class Plan
        {
            public string Direction { get; set; }
            public int Number { get; set; }
            public string Code { get; set; }
        }

        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day18";
        private List<Plan> _data = new();

        public double Solve(Stopwatch watch)
        {
            double sum = 0;

            var (corners, perimeter) = Generate();
            var area = corners.Shoelace();
            var interior = area.PicksTheorem(perimeter);
            sum = interior + perimeter;
            
            return sum;
        }

        private (List<(int x, int y)> corners, long perimeter) Generate()
        {
            List<(int x, int y)> corners = new();
            int x = 0;
            int y = 0;
            long perimeter = 0L;
            foreach (var plan in _data)
            {
                corners.Add((x, y));
                perimeter += plan.Number;

                switch (plan.Direction)
                {
                    case "R":
                        y += plan.Number;
                        break;
                    case "L":
                        y -= plan.Number;
                        break;
                    case "U":
                        x -= plan.Number;
                        break;
                    case "D":
                        x += plan.Number;
                        break;
                    default:
                        throw new Exception();
                }
            }

            return (corners, perimeter);
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
            for (int i = 0; i < lines.Length; i++)
            {
                string item = lines[i];
                var split = item.Split(' ');
                var direction = split[0];
                var number = int.Parse(split[1]);
                var code = split[2][1..^1];

                direction = code.Last() switch
                {
                    '0' => "R",
                    '1' => "D",
                    '2' => "L",
                    '3' => "U",
                    _ => throw new Exception(),
                };

                var temp = code[1..^1];
                number = Convert.ToInt32(temp, 16);

                _data.Add(new Plan
                {
                    Direction = direction,
                    Number = number,
                    Code = code
                });
            }
        }
    }
}