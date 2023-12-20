using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace AOC2023.Day20
{
    public class Day20Part1
    {
        class Module
        {
            public string Name { get; set; }
            public string Operator { get; set; }
            public string FullName { get; set; }
            public Dictionary<string, Module> Childs { get; set; }

            public Module()
            {
                Childs = new();
            }
        }

        class FlipFlop : Module
        {
            public bool IsOn { get; set; }
            public string Pulse { get; set; }

            public FlipFlop()
            {
                IsOn = false;
            }

            public bool HandlePulse(string pulse)
            {
                if (pulse == LOW_PULSE)
                {
                    if (IsOn)
                    {
                        pulse = LOW_PULSE;
                    }
                    else
                    {
                        pulse = HIGH_PULSE;
                    }

                    Pulse = pulse;
                    IsOn = !IsOn;
                    return true;
                }

                return false;
            }

            public List<Module> SendPulse()
            {
                List<Module> childs = new();
                foreach (var item in Childs)
                {
                    var child = item.Value;
                    switch (child)
                    {
                        case Conjunction conjunction:
                            if (conjunction.HandlePulse(Name, Pulse))
                            {
                                if (Pulse == HIGH_PULSE)
                                {
                                    _highPusles++;
                                }
                                else
                                {
                                    _lowPusles++;
                                }

                                childs.Add(child);
                            }
                            break;
                        case FlipFlop flipFLip:
                            if (flipFLip.HandlePulse(Pulse))
                            {
                                if (Pulse == HIGH_PULSE)
                                {
                                    _highPusles++;
                                }
                                else
                                {
                                    _lowPusles++;
                                }

                                childs.Add(child);
                            }
                            break;
                        default:
                            throw new Exception("Invalid child");
                    }
                }

                return childs;
            }
        }

        class Conjunction : Module
        {
            public Dictionary<string, string> Pulses { get; set; }

            public Conjunction()
            {
                Pulses = new();
            }

            public bool HandlePulse(string parentName, string pulse)
            {
                Pulses[parentName] = pulse;
                return true;
            }

            public List<Module> SendPulse()
            {
                List<Module> childs = new();
                var pulse = Pulses.All(x => x.Value == HIGH_PULSE) ? HIGH_PULSE : LOW_PULSE;
                foreach (var item in Childs)
                {
                    var child = item.Value;
                    switch (child)
                    {
                        case Conjunction conjunction:
                            if (conjunction.HandlePulse(Name, pulse))
                            {
                                if (pulse == HIGH_PULSE)
                                {
                                    _highPusles++;
                                }
                                else
                                {
                                    _lowPusles++;
                                }

                                childs.Add(child);
                            }
                            break;
                        case FlipFlop flipFLip:
                            if (flipFLip.HandlePulse(pulse))
                            {
                                if (pulse == HIGH_PULSE)
                                {
                                    _highPusles++;
                                }
                                else
                                {
                                    _lowPusles++;
                                }

                                childs.Add(child);
                            }
                            break;
                        default:
                            throw new Exception("Invalid child");
                    }
                }

                return childs;
            }
        }

        class Broadcaster : Module
        {
            public List<Module> SendPulse(string pulse)
            {
                List<Module> childs = new();
                foreach (var item in Childs)
                {
                    var child = item.Value;
                    switch (child)
                    {
                        case Conjunction conjunction:
                            if (conjunction.HandlePulse(Name, pulse))
                            {
                                if (pulse == HIGH_PULSE)
                                {
                                    _highPusles++;
                                }
                                else
                                {
                                    _lowPusles++;
                                }

                                childs.Add(child);
                            }
                            break;
                        case FlipFlop flipFLip:
                            if (flipFLip.HandlePulse(pulse))
                            {
                                if (pulse == HIGH_PULSE)
                                {
                                    _highPusles++;
                                }
                                else
                                {
                                    _lowPusles++;
                                }

                                childs.Add(child);
                            }
                            break;
                        default:
                            throw new Exception("Invalid child");
                    }
                }

                return childs;
            }
        }

        private static readonly bool _useTestData = true;
        private static readonly string _className = "Day20";
        private List<Module> _modules = new();

        private const string BROADCASTER = "broadcaster";
        private const string LOW_PULSE = "low";
        private const string HIGH_PULSE = "high";

        private static long _lowPusles = 0L;
        private static long _highPusles = 0L;

        public long Solve(Stopwatch watch)
        {
            long sum = 0;

            var initial = _modules.First(x => x.Name == BROADCASTER);
            Queue<Module> queue = new();
            queue.Enqueue(initial);

            for (int i = 0; i < 1; i++)
            {
                queue.Enqueue(initial);

                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();

                    switch (current)
                    {
                        case Broadcaster broadcaster:
                            {
                                var childs = broadcaster.SendPulse(LOW_PULSE);
                                foreach (var item in childs)
                                {
                                    queue.Enqueue(item);
                                }
                            }
                            break;
                        case Conjunction conjunction:
                            {
                                var childs = conjunction.SendPulse();
                                foreach (var item in childs)
                                {
                                    queue.Enqueue(item);
                                }
                            }
                            break;
                        case FlipFlop flipFLip:
                            {
                                var childs = flipFLip.SendPulse();
                                foreach (var item in childs)
                                {
                                    queue.Enqueue(item);
                                }
                            }
                            break;
                        default:
                            throw new Exception("Invalid child");
                    }
                }
            }

            Console.WriteLine(_lowPusles + " " + _highPusles);

            return sum;
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
                var split = item.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var name = split[0].Trim();
                var @operator = "";
                var isOn = true;
                if (!char.IsLetter(name.First()))
                {
                    @operator = name.First().ToString();
                    name = name[1..];

                    if (@operator == "%")
                    {
                        _modules.Add(new FlipFlop()
                        {
                            Name = name,
                            FullName = item,
                            Operator = @operator
                        });
                    }
                    else // IsConjunction
                    {
                        _modules.Add(new Conjunction()
                        {
                            Name = name,
                            FullName = item,
                            Operator = @operator
                        });
                    }
                }
                else
                {
                    // Is broadcaster

                    _modules.Add(new Broadcaster()
                    {
                        Name = name,
                        FullName = item,
                        Operator = @operator
                    });
                }

            }

            foreach (var item in lines)
            {
                var split = item.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var name = split[0].Trim();
                if (!char.IsLetter(name.First()))
                {
                    name = name[1..];
                }

                var module = _modules.First(x => x.Name == name);
                var childs = split[1].Split(',').Select(x => x.Trim());
                foreach (var childName in childs)
                {
                    var child = _modules.FirstOrDefault(x => x.Name == childName);
                    if (child != null)
                    {
                        module.Childs.Add(childName, child);
                    }
                    else
                    {
                        _modules.Add(new Module()
                        {
                            Name = childName,
                            FullName = item
                        });
                    }


                    //if (module is Conjunction conjunction)
                    //{
                    //    conjunction.Pulses.Add(childName, LOW_PULSE);
                    //}
                }
            }
        }
    }
}