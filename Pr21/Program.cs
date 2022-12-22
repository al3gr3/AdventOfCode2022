using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pr15
{
    class Program
    {
        static string[] lines;

        static void Main(string[] args)
        {
#if TEST
            lines = File.ReadAllLines("TextFile1.txt");
#else
            lines = File.ReadAllLines("TextFile2.txt");
#endif

            //FirstStar(lines);

            var result = Find("humn");
            Console.WriteLine(result);
        }

        private static long Find(string v)
        {
            var equation = lines.First(x => x.Contains(v) && !x.StartsWith(v));
            
            var splits = equation.Split(' ');

            var left = splits[1];
            var right = splits.Last();

            var total = splits[0].Split(':').First();

            var oprator = splits[2];

            var other = v == left ? right : left;
            var calculatedOther = calculate(lines.First(x => x.StartsWith(other + ": ")));

            if (total == "root")
                return calculatedOther;
            
            var findTotal = Find(total);

            if (oprator == "+")
                return findTotal - calculatedOther;

            if (oprator == "*")
                return findTotal / calculatedOther;

            if (oprator == "/")
            {
                if (v == left)
                    return findTotal * calculatedOther;
                else
                    return calculatedOther / findTotal;
            }

            if (oprator == "-")
            {
                if (v == left)
                    return findTotal + calculatedOther;
                else
                    return calculatedOther - findTotal;
            }

            throw new InvalidOperationException();
        }

        private static void FirstStar(string[] lines)
        {
            var result = calculate(lines.First(x => x.StartsWith("root" + ": ")));
            Console.WriteLine(result);
        }

        private static long calculate(string v)
        {
            if (v.StartsWith("humn:"))
                Console.WriteLine("yes");

            var splits = v.Split(' ');

            if (splits.Length == 2)
                return long.Parse(splits.Last());

            var left = calculate(lines.First(x => x.StartsWith(splits[1] + ": ")));
            var right = calculate(lines.First(x => x.StartsWith(splits.Last() + ": ")));

            var oprator = splits[2];

            if (oprator == "+")
                return left + right;

            if (oprator == "*")
                return left * right;

            if (oprator == "/")
                return left / right;
            
            if (oprator == "-")
                return left - right;

            throw new InvalidOperationException();
        }
    }
}
