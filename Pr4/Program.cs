using System;
using System.IO;
using System.Linq;

namespace Pr4
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = File.ReadAllLines("TextFile1.txt").Count(line =>
            {
                var splits = line.Split(new[] { '-', ',' });
                var a = int.Parse(splits[0]);
                var b = int.Parse(splits[1]);
                var c = int.Parse(splits[2]);
                var d = int.Parse(splits[3]);
                return isInside(a, b, c) || isInside(a, b, d) || isInside(c, d, a) || isInside(c, d, b); 
            });
        }

        private static bool isInside(int a, int b, int c)
        {
            return a <= c && c <= b;
        }
    }
}
