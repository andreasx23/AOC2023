using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2023.Day15
{
    public class Day15Part2
    {
        private static readonly bool _useTestData = false;
        private static readonly string _className = "Day15";
        private List<string> _data = new List<string>();
        private Dictionary<long, List<(string label, string line)>> _boxes = new();

        public long Solve(Stopwatch watch)
        {
            for (int i = 0; i < 256; i++)
            {
                _boxes[i] = new();
            }

            foreach (var line in _data)
            {
                var key = 0L;
                var label = GetLabel(line);
                foreach (var item in label)
                {
                    var ascii = item + key;
                    ascii *= 17;
                    ascii %= 256;
                    key = ascii;
                }

                var currentBox = _boxes[key];
                if (line[^1] == '-')
                {
                    var index = GetIndex(currentBox, label);
                    if (index != -1)
                    {
                        currentBox.RemoveAt(index);
                    }
                }
                else
                {
                    var index = GetIndex(currentBox, label);
                    if (index != -1)
                    {
                        currentBox[index] = (label, line);
                    }
                    else
                    {
                        currentBox.Add((label, line));
                    }
                }
            }

            var sum = 0L;
            foreach (var kv in _boxes)
            {
                var slotNum = 1;
                foreach (var item in kv.Value)
                {
                    var num = int.Parse(item.line.Last().ToString());
                    var localsum = (kv.Key + 1) * slotNum * num;
                    sum += localsum;
                    slotNum++;
                }
            }

            return sum;
        }

        private string GetLabel(string line)
        {
            StringBuilder label = new StringBuilder();
            foreach (var item in line)
            {
                if (char.IsLetter(item))
                {
                    label.Append(item);
                }
            }
            return label.ToString();
        }

        private int GetIndex(List<(string label, string line)> box, string label)
        {
            for (int i = 0; i < box.Count; i++)
            {
                if (box[i].label == label)
                {
                    return i;
                }
            }

            return -1;
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
            _data = lines[0].Split(',').ToList();
        }
    }
}