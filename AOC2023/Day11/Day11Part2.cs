using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day11
{
    public class Day11Part2
    {
        class Galaxy
        {
            public int Id { get; set; }
            public long X { get; set; }
            public long Y { get; set; }
        }

        class HorizontalComparer : IComparer<Galaxy>
        {
            public int Compare(Galaxy x, Galaxy y)
            {
                return Math.Sign(x.X - y.X);
            }
        }

        class VerticalComparer : IComparer<Galaxy>
        {
            public int Compare(Galaxy x, Galaxy y)
            {
                return Math.Sign(x.Y - y.Y);
            }
        }

        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day11";
        private List<List<char>> _data = new();
        private const char GALAXY = '#';

        public long Solve(Stopwatch stopwatch)
        {
            var sum = 0L;

            var galaxies = FindGalaxies();

            long expansion = 1000000 - 1;

            // Horizontal Expansion
            galaxies.Sort(new HorizontalComparer());
            long horizontalExpansion = 0;
            long lastX = 0;
            for (int i = 1; i < galaxies.Count; i++)
            {
                if (galaxies[i].X > lastX + 1)
                {
                    horizontalExpansion += Math.Clamp(galaxies[i].X - lastX - 1, 0, long.MaxValue) * expansion;
                }

                lastX = galaxies[i].X;
                galaxies[i] = new Galaxy 
                { 
                    X = galaxies[i].X + horizontalExpansion, 
                    Y = galaxies[i].Y 
                };
            }

            // Vertical Expansion
            galaxies.Sort(new VerticalComparer());
            long verticalExpansion = 0;
            long lastY = 0;
            for (int i = 1; i < galaxies.Count; i++)
            {
                if (galaxies[i].Y > lastY + 1)
                {
                    verticalExpansion += Math.Clamp(galaxies[i].Y - lastY - 1, 0, long.MaxValue) * expansion;
                }

                lastY = galaxies[i].Y;
                galaxies[i] = new Galaxy 
                { 
                    X = galaxies[i].X, 
                    Y = galaxies[i].Y + verticalExpansion 
                };
            }

            for (int i = 0; i < galaxies.Count; i++)
            {
                var current = galaxies[i];
                for (int j = i + 1; j < galaxies.Count; j++)
                {
                    var target = galaxies[j];
                    sum += Math.Abs(current.X - target.X) + Math.Abs(current.Y - target.Y);
                }

                //Console.WriteLine($"[{stopwatch.Elapsed}] Done with: {i + 1} / {galaxies.Count}");
            }

            return sum;
        }

        private List<Galaxy> FindGalaxies()
        {
            int id = 1;
            List<Galaxy> galaxies = new();
            for (int i = 0; i < _data.Count; i++)
            {
                for (int j = 0; j < _data[i].Count; j++)
                {
                    var current = _data[i][j];
                    if (current == GALAXY)
                    {
                        galaxies.Add(new Galaxy()
                        {
                            Id = id,
                            X = i,
                            Y = j,
                        });

                        id++;
                    }
                }
            }

            return galaxies;
        }

        public void Result()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            ReadData();
            var result = Solve(stopwatch);
            Console.WriteLine($"[{stopwatch}] Your answer: {result} -- Took: {stopwatch.Elapsed}");
        }

        public void ReadData()
        {
            var lines = File.ReadAllLines(@$"{_className}\{(_useTestData ? "Test" : "Data")}.txt");
            _data = lines.Select(x => x.ToList()).ToList();
        }
    }
}