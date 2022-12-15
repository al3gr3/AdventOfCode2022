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
            var lineToCheck = 10;
            var x_from = -10;
            var x_to = 30;

#else
            var lines = File.ReadAllLines("TextFile2.txt");
            var lineToCheck = 2_000_000;
            var x_from = -1_000_0000;
            var x_to = 5_000_0000;
#endif
            var pairs = lines.Select(line =>
            {
                var splits = line.Split(new[] { '=', ',', ':' });
                return new Pair
                {
                    Sensor = new Point
                    {
                        X = int.Parse(splits[1]),
                        Y = int.Parse(splits[3])
                    },
                    Beacon = new Point
                    {
                        X = int.Parse(splits[5]),
                        Y = int.Parse(splits.Last())
                    }
                };
            }).ToList();

            // FirstStar(lineToCheck, x_from, x_to, pairs);
            SecondStar(pairs);
        }

        /// <summary>
        /// LOL
        /// </summary>
        /// <param name="pairs"></param>
        private static void SecondStar(List<Pair> pairs)
        {
            pairs.ForEach(p1 =>
            {
                pairs.ForEach(p2 =>
                {
                    var epsilon = new Pair { Sensor = p1.Sensor, Beacon = p2.Sensor }.Manhattan - (p1.Manhattan + p2.Manhattan);
                    if (0 < epsilon && epsilon <= 2)
                    {
                        Console.WriteLine(p1.Print());
                        Console.WriteLine(p2.Print());
                        Console.WriteLine();
                    }
                });
            });

            var try1 = pairs.First(x => x.Sensor.Equals(new Point { X = 3048293, Y = 3598671 }));
            var starting = new Point { Y = try1.Sensor.Y, X = try1.Sensor.X - try1.Manhattan - 1 };
            for (var i = 0; i < 4_000_000; i++)
            {
                var next = new Point { X = starting.X + i, Y = starting.Y - i };
                if (!pairs.Any(x => x.Forbids(next)))
                {
                    Console.WriteLine(TuningFreq(next));

                }
            }
        }

        private static void FirstStar(int lineToCheck, int x_from, int x_to, List<Pair> pairs)
        {
            var result = 0;
            for (var x = x_from; x < x_to; x++)
            {
                if (pairs.Any(pair => pair.Forbids(new Point { X = x, Y = lineToCheck })))
                    result++;
            }

            Console.WriteLine(result);
        }

        static decimal TuningFreq(Point p) => 4000000M * p.X + p.Y;
    }

    class Pair
    {
        internal Point Sensor;
        internal Point Beacon;

        internal int Manhattan => Math.Abs(Sensor.X - Beacon.X) + Math.Abs(Sensor.Y - Beacon.Y);

        internal bool Forbids(Point point)
        {
            if (Sensor.Equals(point))
                return true;
            if (Beacon.Equals(point))
                return false;
            return this.Manhattan >= new Pair { Sensor = Sensor, Beacon = point }.Manhattan;
        }

        internal string Print() => $"Beacon: {Beacon.Print()}{Environment.NewLine}Sensor: {Sensor.Print()}";
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

        internal bool Equals(Point p) => (this.X, this.Y) == (p.X, p.Y);

        internal object Print() => $"X: {X}, Y: {Y}";
    }
}
