using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pr15
{
    class Program
    {
        static void Main(string[] args)
        {
#if TEST
            var lines = File.ReadAllLines("TextFile1.txt");
#else
            var lines = File.ReadAllLines("TextFile2.txt");
#endif
            var numbers = lines.Select(x => new Number { X = int.Parse(x) }).ToList();

            var sorted = SecondStar(numbers);

            var zero = numbers.First(x => x.X == 0);
            var index = sorted.IndexOf(zero);

            var a1000 = sorted[(index + 1000) % numbers.Count()].X;
            var a2000 = sorted[(index + 2000) % numbers.Count()].X;
            var a3000 = sorted[(index + 3000) % numbers.Count()].X;

            var resulr = a1000 + a2000 + a3000;
            Console.WriteLine(resulr);
        }

        private static List<Number> SecondStar(List<Number> numbers)
        {
            var sorted = numbers.ToList();            

            numbers.ForEach(x => x.X *= 811589153);

            Enumerable.Range(1, 10).ToList().ForEach(round =>
            {
                Mix(numbers, sorted);
            });
            return sorted;
        }

        private static void Mix(List<Number> numbers, List<Number> sorted)
        {
            var count = numbers.Count() - 1;
            numbers.ForEach(original =>
            {
                var index = sorted.IndexOf(original);
                sorted.RemoveAt(index);

                var newindex = (index + original.X) % count;
                if (newindex < 0)
                    newindex += count;

                sorted.Insert((int)newindex, original);
            });
        }

        private static List<Number> FirstStar(List<Number> numbers)
        {
            var sorted = numbers.ToList();

            Mix(numbers, sorted);
            return sorted;
        }
    }

    internal class Number
    {
        public decimal X;
        public override string ToString() => "" + X;
    }
}
