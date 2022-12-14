using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pr14
{
    class Program
    {
        const int MAX = 1200;
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("TextFile1.txt");
            var testData = new[]
            {
                "498,4 -> 498,6 -> 496,6",
                "503,4 -> 502,4 -> 502,9 -> 494,9",
                //"480,11 -> 522,11"
            };
            //lines = testData;

            var list = lines.ToList();

            //list.Add($"0,179 -> {MAX - 1},179");

            var allPoints = list.SelectMany(line =>
            {
                var points = line.Split(new[] { " -> " }, StringSplitOptions.RemoveEmptyEntries).Select(pos =>
                {
                    var splits = pos.Split(',');
                    return new Point
                    {
                        X = int.Parse(splits.First()),
                        Y = int.Parse(splits.Last()),
                    };
                });
                
                var prevPoint = points.First();
                points.Skip(1).ToList().ForEach(p =>
                {
                    draw(prevPoint, p);
                    prevPoint = p;
                });
                return points;
            });

            var left = new Point { X = allPoints.Min(x => x.X), Y = 0 };
            var right = new Point { X = allPoints.Max(x => x.X), Y = allPoints.Max(x => x.Y) };

            var directions = new []
            {
                new Point { Y = 1, X = 0 },
                new Point { Y = 1, X = -1 },
                new Point { Y = 1, X = 1 },
            };

            var count = 0;
            while (true)
            {
                var pos = new Point { X = 500, Y = 0 };
                if (directions.Select(x => pos.Clone().Add(x)).All(x => M[x.X][x.Y] == 2))
                {
                    count++;
                    goto labelout;
                }

                while (true)
                {
                    var nextPos = directions.Select(x => pos.Clone().Add(x)).FirstOrDefault(x => M[x.X][x.Y] == 0);
                    if (nextPos == null)
                    {
                        M[pos.X][pos.Y] = 2;
                        break;
                    }
                    else
                    {
                        pos = nextPos;
                        if (nextPos.Y > (right.Y + 10))
                            goto labelout;
                    }
                }
                count++;
            }

            labelout: 
            write(left, right);
            Console.ReadLine();
        }

        private static void write(Point point1, Point point2)
        {
            M[500][0] = 3;
            
            var chars = new Dictionary<int, char>
            {
                { 1, '#' },
                { 0, '.' },
                { 2, 'o' },
                { 3, '+' },
            };
            for (var y = point1.Y; y <= point2.Y; y++)
            {
                var s = Enumerable.Range(point1.X, point2.X - point1.X + 1).Select(x => chars[M[x][y]]);
                
                Console.WriteLine(string.Concat(s));
            }
        }

        static int[][] M = Enumerable.Range(0, MAX).Select(x => new int[MAX]).ToArray();

        private static void draw(Point prevPoint, Point p)
        {
            if (prevPoint.X == p.X)
            {
                for (var i = Math.Min(prevPoint.Y, p.Y); i <= Math.Max(prevPoint.Y, p.Y); i++)
                    M[p.X][i] = 1;
            }
            else
            {
                for (var i = Math.Min(prevPoint.X, p.X); i <= Math.Max(prevPoint.X, p.X); i++)
                    M[i][p.Y] = 1;
            }
        }
    }

    class Point
    {
        internal int X;
        internal int Y;

        internal Point Add(Point point)
        {
            this.X += point.X;
            this.Y += point.Y;
            return this;
        }

        internal Point Clone()
        {
            return new Point { X = this.X, Y = this.Y };
        }
    }
}
