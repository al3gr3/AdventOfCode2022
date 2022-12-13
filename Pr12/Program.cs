using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pr11
{
    class Program
    {
        static void Main(string[] args)
        {
            var directions = new Dictionary<char, Point>
            {
                { 'v', new Point { Y = 1, X = 0 } },
                { '<', new Point { Y = 0, X = -1 } },
                { '>', new Point { Y = 0, X = 1 } },
                { '^', new Point { Y = -1, X = 0 } },
            };

            var lines = File.ReadAllLines("TextFile1.txt");
            var height = lines.Length;
            var width = lines.First().Length;

            var prev = Enumerable.Range(1, height).Select(x => new Point[width]).ToArray();
            var dist = Enumerable.Range(1, height).Select(x => Enumerable.Range(1, width).Select(y => 100000000).ToArray()).ToArray();
            var queue = Enumerable.Range(0, height).SelectMany(y => Enumerable.Range(0, width).Select(x => new Point { X = x, Y = y })).ToList();

            secondStar(directions, lines, prev, dist, queue);
            //firstStar(directions, lines, prev, dist, queue);
        }

        private static void firstStar(Dictionary<char, Point> directions, string[] lines, Point[][] prev, int[][] dist, List<Point> queue)
        {
            dist[20][0] = 0;
            while (queue.Any())
            {
                var u = queue.Aggregate(queue.First(), (min, n) => dist[min.Y][min.X] > dist[n.Y][n.X] ? n : min);
                queue.Remove(u);

                directions.Keys.ToList().ForEach(direction =>
                {
                    var newPos = u.Clone();
                    newPos.Add(directions[direction]);

                    if (0 <= newPos.X && newPos.X < lines.First().Length &&
                        0 <= newPos.Y && newPos.Y < lines.Length &&
                        okToJump(lines[u.Y][u.X], lines[newPos.Y][newPos.X])
                    )
                    {
                        var v = queue.FirstOrDefault(x => x.X == newPos.X && x.Y == newPos.Y);
                        if (v == null)
                            return;

                        var alt = dist[u.Y][u.X] + 1;
                        if (alt < dist[v.Y][v.X])
                        {
                            dist[v.Y][v.X] = alt;
                            prev[v.Y][v.X] = u.Clone();
                        }
                    }
                });
            }
            Console.WriteLine(dist[20][58]);
        }

        private static void secondStar(Dictionary<char, Point> directions, string[] lines, Point[][] prev, int[][] dist, List<Point> queue)
        {
            var result = 100000000;
            dist[20][58] = 0;
            while (queue.Any())
            {
                var u = queue.Aggregate(queue.First(), (min, n) => dist[min.Y][min.X] > dist[n.Y][n.X] ? n : min);
                queue.Remove(u);

                directions.Keys.ToList().ForEach(direction =>
                {
                    var newPos = u.Clone();
                    newPos.Add(directions[direction]);

                    if (0 <= newPos.X && newPos.X < lines.First().Length &&
                        0 <= newPos.Y && newPos.Y < lines.Length &&
                        okToJump(lines[newPos.Y][newPos.X], lines[u.Y][u.X])
                    )
                    {
                        var v = queue.FirstOrDefault(x => x.X == newPos.X && x.Y == newPos.Y);
                        if (v == null)
                            return;

                        var alt = dist[u.Y][u.X] + 1;
                        if (alt < dist[v.Y][v.X])
                        {
                            dist[v.Y][v.X] = alt;
                            prev[v.Y][v.X] = u.Clone();

                            if (lines[v.Y][v.X] == 'a')
                                result = Math.Min(result, dist[v.Y][v.X]);
                        }
                    }
                });
            }

            Console.WriteLine(result);
        }

        private static bool okToJump(char v1, char v2)
        {
            if (v2 == 'E')
                return 'z' - v1 <= 1;
            return v2 - v1 <= 1;
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
