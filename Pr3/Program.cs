using System.IO;
using System.Linq;

namespace Pr3
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = 0;
            var s = File.ReadAllLines("TextFile1.txt");
            
            for (var i = 0; i < s.Count(); i += 3)
            {
                var intersection = s[i].Intersect(s[i + 1]).Intersect(s[i + 2]).ToList();
                intersection.ForEach(ch =>
                {
                    result += f(ch);
                });
            }
        }

        private static int f(char ch)
        {
            if (("" + ch).ToUpper() == "" + ch)
                return (int)ch - 38;
            else
                return (int)ch - 96;
        }
    }
}
