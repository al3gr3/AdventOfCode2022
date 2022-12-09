using System;
using System.IO;
using System.Linq;

namespace Pr4
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = File.ReadAllText("TextFile1.txt");
            var length = 14;
            for (var i = 0; i < s.Length - length; i++)
            {
                var sub = s.Substring(i, length);
                if (sub.GroupBy(x => x).All(grp => grp.Count() == 1))
                {
                    Console.WriteLine(i + length);
                    break;
                }
            }
        }
    }
}
