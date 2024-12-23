using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AOC2021.Day18
{
    public class Day18Part1
    {
        private static readonly bool _useTestData = true;
        private static readonly string _className = "Day18";
        private List<string> _data = new();

        private long Solve(Stopwatch watch)
        {
            var sum = 0L;

            var first = _data.First();
            Dfs(first, 0, 0);

            return sum;
        }

        private void Dfs(string snailfish, int index, int startBracketCount)
        {
            var c = snailfish[index];

            if (startBracketCount >= 4)
            {
                // [[[[[9,8],1],2],3],4]
                var reduced = Reduce(snailfish, index);
                // ("[0,9]", false, false)

                var start = snailfish.Substring(0, index);
                var next = $"{start}{reduced.reduce}{snailfish.Substring(start.Length + reduced.reduce.Length)}";

                //var end = index;
                //while (end < snailfish.Length && snailfish[end] != ']')
                //{
                //    end++;
                //}
                //snailfish.Replace()
            }
            else if (c == '[')
            {
                Dfs(snailfish, index + 1, startBracketCount + 1);
            }
        }

        private string Add(string snailfish1, string snailfish2)
        {
            return $"[{snailfish1},{snailfish2}]";
        }

        private (string reduce, bool isLeftAbove10, bool isRightAbove10) Reduce(string snailfish, int index)
        {
            List<string> list = new List<string>();
            var number = index + 1;
            while (number >= 0 && snailfish[number] != ']')
            {
                list.Add(snailfish[number].ToString());
                number++;
            }
            //list.Reverse();

            var left = index;
            while (left >= 0 && !char.IsDigit(snailfish[left]))
            {
                left--;
            }

            bool isLeftAbove10 = false;
            if (left >= 0)
            {
                var current = int.Parse(list[0]);
                var leftNum = int.Parse(snailfish[left].ToString());
                var sum = current + leftNum;
                list[0] = sum.ToString();
                isLeftAbove10 = sum >= 10;
            }
            else
            {
                list[0] = "0";
            }

            var right = number;
            while (right < snailfish.Length && !char.IsDigit(snailfish[right]))
            {
                right++;
            }

            bool isRightAbove10 = false;
            if (right < snailfish.Length)
            {
                var current = int.Parse(list[2]);
                var rightNum = int.Parse(snailfish[right].ToString());
                var sum = current + rightNum;
                list[2] = sum.ToString();
                isRightAbove10 = sum >= 10;
            }
            else
            {
                list[0] = "2";
            }

            return ($"[{list.First()},{list.Last()}]", isLeftAbove10, isRightAbove10);
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
            var lines = File.ReadAllLines(@$"{_className}\{(_useTestData ? "Test2" : "Data")}.txt");
            _data = lines.ToList();
        }
    }
}