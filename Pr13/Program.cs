using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pr13
{
    class Program
    {
        private const string MARKER_2 = "[[2]]";
        private const string MARKER_6 = "[[6]]";

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("TextFile1.txt");

            firstStar(lines);
            secondStar(lines.Where(x => !string.IsNullOrEmpty(x)).ToList());
        }

        private static void secondStar(List<string> list)
        {
            list.Add(MARKER_2);
            list.Add(MARKER_6);

            list.Sort((x, y) => compare(parse(x), parse(y))); // todo: redundant parsing

            var result = (list.IndexOf(MARKER_2) + 1) * (list.IndexOf(MARKER_6) + 1);
            Console.WriteLine(result);
        }

        private static void firstStar(string[] lines)
        {
            var index = 1;
            var result = 0;
            for (var i = 0; i < lines.Length; i += 3)
            {
                var left = parse(lines[i]);
                var right = parse(lines[i + 1]);
                var c = compare(left, right);
                if (c <= 0)
                    result += index;

                index++;
            }
            Console.WriteLine(result);
        }

        private static int compare(Item left, Item right)
        {
            if (left is Ynteger && right is Ynteger)
            {
                return (left as Ynteger).Value - (right as Ynteger).Value;
            }
            else if (left is Lyst && right is Lyst)
            {
                for (var i = 0; i < (left as Lyst).Items.Count(); i++)
                {
                    if (i > (right as Lyst).Items.Count() - 1)
                    {
                        return 1;
                    }
                    var result = compare((left as Lyst).Items[i], (right as Lyst).Items[i]);
                    if (result != 0)
                        return result;
                }

                return (left as Lyst).Items.Count() < (right as Lyst).Items.Count()
                    ? -1
                    : 0;
            }
            else if (left is Lyst && right is Ynteger)
                return compare(left, new Lyst { Items = new List<Item> { right } });
            else if (left is Ynteger && right is Lyst)
                return compare(new Lyst { Items = new List<Item> { left } }, right);
            else
                throw new InvalidOperationException("");
        }

        private static Item parse(string v)
        {
            if (string.IsNullOrEmpty(v))
                return null;
            if (v.StartsWith("["))
            {
                v = v.Substring(1, v.Length - 2);

                var correctSplits = new List<string>();

                var run = "";
                var depth = 0;
                v.ToList().ForEach(ch =>
                {
                    if (ch == ',' && depth == 0)
                    {
                        correctSplits.Add(run);
                        run = "";
                    }
                    else if (ch == '[')
                    {
                        depth++;
                        run += ch;
                    }
                    else if (ch == ']')
                    {
                        depth--;
                        run += ch;
                    }
                    else
                        run += ch;
                });

                correctSplits.Add(run);

                var result = new Lyst
                {
                    Items = correctSplits.Select(x => parse(x)).Where(x => x != null).ToList()
                };

                return result;
            }
            else
            {
                return new Ynteger { Value = int.Parse(v) }; 
            }
        }
    }

    class Item
    {

    }

    class Lyst : Item
    {
        internal List<Item> Items;
    }

    class Ynteger : Item
    {
        internal int Value;
    }
}
