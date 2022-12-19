using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pr15
{
    class Program
    {
        static int MAX = 26;

        static List<Point> directions = new[]
        {
            new Point { X = 1 },
            new Point { X = -1 },
            new Point { Y = 1 },
            new Point { Y = -1 },
            new Point { Z = 1 },
            new Point { Z = -1 },
        }.ToList();

        static void Main(string[] args)
        {
            var cubes =
                Enumerable.Range(0, MAX).Select(x =>
                Enumerable.Range(0, MAX).Select(y => new byte[MAX]).ToArray()
            ).ToArray();
#if TEST
            var lines = File.ReadAllLines("TextFile1.txt");
#else
            var lines = File.ReadAllLines("TextFile2.txt");
#endif

            lines.ToList().ForEach(line =>
            {
                var splits = line.Split(',').Select(x => int.Parse(x)).ToArray();
                cubes[splits[0] + 1][splits[1] + 1][splits[2] + 1] = 1;
            });

            firstStar(cubes);

            fillWithLava(cubes);
            secondStar(cubes);
        }

        private static void secondStar(byte[][][] cubes)
        {
            var total = 0;
            for (var x = 1; x < 23; x++)
                for (var y = 1; y < 23; y++)
                    for (var z = 1; z < 23; z++)
                    {
                        if (cubes[x][y][z] == 1)
                        {
                            var sides = directions.Count(dir => cubes[x + dir.X][y + dir.Y][z + dir.Z] == 2);

                            total += sides;
                        }
                    }
            Console.WriteLine(total);
        }

        private static void firstStar(byte[][][] cubes)
        {
            var total = 0;
            for (var x = 1; x < 23; x++)
                for (var y = 1; y < 23; y++)
                    for (var z = 1; z < 23; z++)
                    {
                        if (cubes[x][y][z] == 1)
                        {
                            var sides = 6 - directions.Count(dir => cubes[x + dir.X][y + dir.Y][z + dir.Z] == 1);

                            total += sides;
                        }
                    }
            Console.WriteLine(total);
        }

        private static void fillWithLava(byte[][][] cubes)
        {
            var lavas = new Queue<Point>();
            lavas.Enqueue(new Point { });

            while(lavas.Any())
            {
                var current = lavas.Dequeue();
                directions.ForEach(dir =>
                {
                    var next = current.Clone().Add(dir);
                    if (new[] { next.X, next.Y, next.Z }.All(c => 0 <= c && c <= 23) && cubes[next.X][next.Y][next.Z] == 0)
                    {
                        cubes[next.X][next.Y][next.Z] = 2;
                        lavas.Enqueue(next);
                    }
                });
            }
        }
    }

    class Point
    {
        internal int X;
        internal int Y;
        internal int Z;

        internal Point Add(Point point)
        {
            this.X += point.X;
            this.Y += point.Y;
            this.Z += point.Z;
            return this;
        }

        internal Point Clone()
        {
            return new Point { X = this.X, Y = this.Y, Z  = this.Z };
        }

        internal bool IsEqual(Point p) => (this.X, this.Y, this.Z) == (p.X, p.Y, p.Z);
    }
}
