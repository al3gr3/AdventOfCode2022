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
            var s = "";
            var spritePos = 1;
            var caret = 0;
            foreach (var line in lines)
            {
                var words = line.Split(' ');
                if (words.First() == "addx")
                {
                    write(ref s, spritePos, ref caret);

                    write(ref s, spritePos, ref caret);

                    var amount = int.Parse(words.Last());
                    spritePos += amount;
                }
                else if (words.First() == "noop")
                {
                    write(ref s, spritePos, ref caret);
                }
            }

            Console.WriteLine(s);
        }

        private static void write(ref string s, int spritePos, ref int caret)
        {
            s += Math.Abs(caret - spritePos) <= 1 ? "#" : ".";
            caret++;
            if (caret == 40)
            {
                caret = 0;
                s += Environment.NewLine;
            }
        }

        private static void First(string[] lines)
        {
            var sum = 1;
            var cpus = 0;
            var result = 0;
            foreach (var line in lines)
            {
                var words = line.Split(' ');
                if (words.First() == "addx")
                {
                    cpus++;
                    result += Check(sum, cpus);
                    cpus++;
                    result += Check(sum, cpus);

                    var amount = int.Parse(words.Last());

                    sum += amount;
                }
                else if (words.First() == "noop")
                {
                    cpus++;
                    result += Check(sum, cpus);
                }
            }
            Console.WriteLine(result);

            Console.ReadLine();
        }

        private static int Check(int sum, int cpus)
        {
            if (new[] { 20, 60, 100, 140, 180, 220, 260 }.Any(x => x == cpus))
            {
                return sum * cpus;
            }

            return 0;
        }
    }
}
