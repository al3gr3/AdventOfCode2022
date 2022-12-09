using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pr4
{
    class Program
    {
        static void Main(string[] args)
        {
            NewMethod3();
        }

        private static void NewMethod2()
        {
            var s = File.ReadAllLines("TextFile1.txt");
            var max = 0;
            for (var index0 = 0; index0 < s.Length; index0++)
                for (var index1 = 0; index1 < s.Length; index1++)
                {
                    var lr = s[index0].Skip(index1 + 1).ToArray();
                    var rl = s[index0].Take(index1).Reverse().ToArray();
                    var column = s.Select(x => x[index1]).ToArray();
                    var ud = column.Skip(index0 + 1).ToArray();
                    var du = column.Take(index0).Reverse().ToArray();

                    var result = new[] { lr, rl, ud, du }.Select(x => scenicView(x, s[index0][index1])).Aggregate(1, (s, n) => s * n);
                    if (max < result)
                        max = result;
                }

            Console.WriteLine(max == 590824);
        }

        private static void NewMethod3()
        {
            var s = File.ReadAllLines("TextFile1.txt");
            var result = 0;
            for (var index0 = 0; index0 < s.Length; index0++)
                for (var index1 = 0; index1 < s.Length; index1++)
                {
                    var lr = s[index0].Skip(index1 + 1).ToArray();
                    var rl = s[index0].Take(index1).Reverse().ToArray();
                    var column = s.Select(x => x[index1]).ToArray();
                    var ud = column.Skip(index0 + 1).ToArray();
                    var du = column.Take(index0).Reverse().ToArray();

                    if (new[] { lr, rl, ud, du }.Any(x => isVisible(x, s[index0][index1])))
                        result++;
                }

            Console.WriteLine(result == 1719);
        }

        private static bool isVisible(char[] ud, char v)
        {
            return ud.All(x => x < v);
        }

        private static int scenicView(char[] du, char v)
        {
            var result = 0;
            for (var i = 0; i < du.Length; i++)
            {
                result++;
                if (du[i] >= v)
                {
                    break;
                }
            }
            return result;
        }

        private static void NewMethod()
        {
            var s = File.ReadAllLines("TextFile1.txt");
            var matrix = Enumerable.Range(0, s.Length).Select(x => new bool[s.Length]).ToArray();
            for (var index1 = 1; index1 < s.Length - 1; index1++)
            {
                var max = s[index1][0];
                for (var index2 = 1; index2 < s.Length - 1; index2++)
                {
                    if (s[index1][index2] > max)
                    {
                        matrix[index1][index2] = true;
                        max = s[index1][index2];
                    }
                }

                max = s[index1][s.Length - 1];
                for (var index2 = 1; index2 < s.Length - 1; index2++)
                {
                    if (s[index1][s.Length - 1 - index2] > max)
                    {
                        matrix[index1][s.Length - 1 - index2] = true;
                        max = s[index1][s.Length - 1 - index2];
                    }
                }

                max = s[0][index1];
                for (var index2 = 1; index2 < s.Length - 1; index2++)
                {
                    if (s[index2][index1] > max)
                    {
                        matrix[index2][index1] = true;
                        max = s[index2][index1];
                    }
                }

                max = s[s.Length - 1][index1];
                for (var index2 = 1; index2 < s.Length - 1; index2++)
                {
                    if (s[s.Length - 1 - index2][index1] > max)
                    {
                        matrix[s.Length - 1 - index2][index1] = true;
                        max = s[s.Length - 1 - index2][index1];
                    }
                }
            }
            var result = matrix.SelectMany(x => x).Count(x => x) + s.Length * 4 - 4;
            Console.WriteLine(result == 1719);
        }
    }
}
