using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Pr15
{
    class Program
    {
        static string[] lines;

        static void Main(string[] args)
        {
#if TEST
            lines = File.ReadAllLines("TextFile1.txt");
#else
            lines = File.ReadAllLines("TextFile2.txt");
#endif

            var lastLine = lines.Last();
            lines = lines.Take(lines.Length - 2).ToArray();

            var width = lines.Max(x => x.Length);
            var height = lines.Length;

            for (var i = 0; i < lines.Length; i++)
                lines[i] += string.Concat(Enumerable.Range(0, width - lines[i].Length).Select(r => ' '));

            var facets = Enumerable.Range(1, 6).Select(x => new string[] { }).ToArray();
            int size;
#if TEST
            size = 4;
            facets[0] =                lines.Take(size).Select(x => string.Concat(x.Skip(size * 2).Take(size))).ToArray();
            facets[1] =     lines.Skip(size).Take(size).Select(x => string.Concat(x.Skip(size * 2).Take(size))).ToArray();
            facets[2] = lines.Skip(2 * size).Take(size).Select(x => string.Concat(x.Skip(size * 2).Take(size))).ToArray();

            facets[3] = lines.Skip(2 * size).Take(size).Select(x => string.Concat(x.Skip(size * 3).Take(size))).ToArray();

#else
            size = 50;
            facets[0] =                lines.Take(size).Select(x => string.Concat(x.Skip(size).Take(size))).ToArray(); 
            facets[1] =     lines.Skip(size).Take(size).Select(x => string.Concat(x.Skip(size).Take(size))).ToArray();
            facets[2] = lines.Skip(2 * size).Take(size).Select(x => string.Concat(x.Skip(size).Take(size))).ToArray();

            facets[3] = lines.Take(size).Select(x => string.Concat(x.Skip(2 * size).Take(size))).ToArray();
#endif

            var pos = new Position
            {
                Direction = new Point
                {
                    X = 1
                },
                P = new Point
                {
                    X = lines.First().IndexOf('.'),
                    Y = 0,
                }
            };

            var instuctions = Regex.Split(lastLine, "([LR])");
            foreach (var match in instuctions)
            {
                if (match == "L")
                    pos.Direction.Left();
                else if (match == "R")
                    pos.Direction.Right();
                else
                {
                    var amount = int.Parse(match);
                    var next = pos.P.Clone();

                    Enumerable.Range(1, amount).ToList().ForEach(step =>
                    {
                        var candidate = Move(width, height, pos, next);
                        if (candidate != null)
                            next = candidate;
                    });
                    pos.P = next;
                }
            }

            Console.WriteLine(pos.Score);
        }

        private static Point Move(int width, int height, Position pos, Point next1)
        {
            var next = next1.Clone();
            next.Add(pos.Direction);
            
            if (next.Y > height - 1)
                next.Y = 0;
            if (next.X > width - 1)
                next.X = 0;

            if (next.Y < 0)
                next.Y = height - 1;
            if (next.X < 0)
                next.X = width - 1;

            if (lines[next.Y][next.X] == ' ')
                return Move(width, height, pos, next);
            else if (lines[next.Y][next.X] == '.')
                return next;
            else if (lines[next.Y][next.X] == '#')
                return null;

            else throw new InvalidOperationException();
        }

        private static Point Move2(int width, int height, Position pos, Point next1)
        {
            var next = next1.Clone();
            next.Add(pos.Direction);

            if (next.Y > height - 1)
                next.Y = 0;
            if (next.X > width - 1)
                next.X = 0;

            if (next.Y < 0)
                next.Y = height - 1;
            if (next.X < 0)
                next.X = width - 1;

            if (lines[next.Y][next.X] == ' ')
                return Move(width, height, pos, next);
            else if (lines[next.Y][next.X] == '.')
                return next;
            else if (lines[next.Y][next.X] == '#')
                return null;

            else throw new InvalidOperationException();
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

        internal void Left()
        {
            if ((X, Y) == (0, 1))
                (X, Y) = (1, 0);
            else if ((X, Y) == (0, -1))
                (X, Y) = (-1, 0);
            else if ((X, Y) == (1, 0))
                (X, Y) = (0, -1);
            else if ((X, Y) == (-1, 0))
                (X, Y) = (0, 1);
            else
                throw new InvalidOperationException();
        }

        internal void Right()
        {
            Left();
            Left();
            Left();
        }

        internal Point Clone()
        {
            return new Point { X = this.X, Y = this.Y };
        }

        internal bool IsEqual(Point p) => (this.X, this.Y ) == (p.X, p.Y);
    }

    class Position
    {
        internal Point P;
        internal Point Direction;

        // The final password is the sum of 1000 times the row, 4 times the column, and the facing.
        internal int Score => 1000 * (P.Y + 1) + 4 * (P.X + 1) + DirectionScore();

        // Facing is 0 for right (>), 1 for down (v), 2 for left (<), and 3 for up (^). 
        internal int DirectionScore()
        {
            if ((Direction.X, Direction.Y) == (1, 0))
                return 0;
            if ((Direction.X, Direction.Y) == (0, 1))
                return 1;
            if ((Direction.X, Direction.Y) == (-1, 0))
                return 2;
            if ((Direction.X, Direction.Y) == (0, -1))
                return 3;

            throw new InvalidOperationException();
        }
    }
}
