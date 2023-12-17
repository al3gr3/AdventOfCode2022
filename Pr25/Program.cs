using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
            var result = lines.Select(x => Convert(x)).Sum();


            var s = new LeetCode().GenerateParenthesis(3);
            Console.WriteLine(result);
            Console.WriteLine(Convert(result));
        }

        private static string Convert(long num)
        {
            var result = "";
            while(num > 0)
            {
                var last = num % 5;
                if (last == 3 || last == 4)
                {
                    num += 5;
                    result += (last == 3) ? '=' : '-';
                }
                else
                {
                    result += last;
                }

                num /= 5;
            }
            return string.Concat(result.Reverse());
        }

        private static long Convert(string line)
        {
            var dict = new Dictionary<char, int>
            {
                { '2', 2 },
                { '1', 1 },
                { '0', 0 },
                { '-', -1 },
                { '=', -2 },
            };

            long power = 1;
            long result = 0;

            line.Reverse().ToList().ForEach(ch =>
            {
                result += power * dict[ch];
                power *= 5;
            });

            if (result < 0)
                throw new Exception();
            return result;
        }


    }
}
