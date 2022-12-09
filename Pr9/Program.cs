using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Pr9
{
    class Program
    {
        static void Main(string[] args)
        {
            var directions = new Dictionary<string, Point>
            {
                { "U", new Point { Y = 1, X = 0 } },
                { "L", new Point { Y = 0, X = -1 } },
                { "R", new Point { Y = 0, X = 1 } },
                { "D", new Point { Y = -1, X = 0 } },
            };

            var seed = new Point { X = 0, Y = 0 };
            const int KNOTS = 10;
            var t = Enumerable.Range(0, KNOTS).Select(x => seed.Clone()).ToArray();

            var positions = new List<Point>
            {
                t.Last().Clone()
            };

            File.ReadAllLines("TextFile2.txt").ToList().ForEach(line =>
            {
                var splits = line.Split(' ');
                var direction = splits.First();
                var amount = int.Parse(splits.Last());

                Enumerable.Range(0, amount).ToList().ForEach(step =>
                {
                    t[0].Add(directions[direction]);

                    foreach(var i in Enumerable.Range(1, t.Length - 1))
                    {
                        t[i] = tail(t[i - 1], t[i]);
                    }

                    positions.Add(t.Last().Clone());
                });
            });

            var result = positions.Distinct(new Comparer()).ToList();
            var count = result.Count();
            Console.WriteLine(count);
            if (count != 2445)
                throw new Exception("!");
        }

        static Point tail(Point h, Point tail)
        {
            var t = tail.Clone();
            var dx = Math.Abs(h.X - t.X);
            var dy = Math.Abs(h.Y - t.Y);
            /*
            if ((dx, dy) == (0, 2))
                t.Y = (t.Y + h.Y) / 2;
            else if ((dx, dy) == (2, 0))
                t.X = (t.X + h.X) / 2;
            else if ((dx, dy) == (2, 1))
            {
                t.X = (t.X + h.X) / 2;
                t.Y = h.Y;
            }
            else if ((dx, dy) == (1, 2))
            {                
                t.X = h.X;
                t.Y = (t.Y + h.Y) / 2;
            }
            else if ((dx, dy) == (2, 2))
            {                
                t.X = (t.X + h.X) / 2;
                t.Y = (t.Y + h.Y) / 2;
            }
            */
            if (dx == 2)
            {
                t.X = (t.X + h.X) / 2;
                if (dy == 1)
                    t.Y = h.Y;
            }
            if (dy == 2)
            {
                t.Y = (t.Y + h.Y) / 2;
                if (dx == 1)
                    t.X = h.X;
            }            
            return t;
        }

        class Comparer : IEqualityComparer<Point>
        {
            public bool Equals([AllowNull] Point x, [AllowNull] Point y)
            {
                return (x.X, x.Y) == (y.X, y.Y);
            }

            public int GetHashCode([DisallowNull] Point obj)
            {
                return 1000 * obj.X + obj.Y; 
            }
        }

        class Point
        {
            internal int X;
            internal int Y;

            internal void Add(Point point)
            {
                this.X += point.X;
                this.Y += point.Y;
            }

            internal Point Clone()
            {
                return new Point { X = this.X, Y = this.Y };
            }
        }
    }
}
