using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pr1
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("TextFile1.txt");
            var current = 0;
            var max = 0;
            var index = 0;
            var indexOfMax = 0;
            var all = new List<int>();
            lines.ToList().ForEach(line =>
            {
                if (!string.IsNullOrEmpty(line))
                {
                    current += int.Parse(line);
                }
                else
                {
                    all.Add(current);
                    if (max < current)
                    {
                        max = current;
                        indexOfMax = index;
                    }
                    current = 0;
                }
                index++;
            });
            Console.WriteLine($"MAX {max} index {indexOfMax}");

            Console.WriteLine(all.OrderByDescending(x => x).Take(3).Sum());
            Console.ReadLine();
        }
    }
}
