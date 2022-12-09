using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pr2
{
    class Program
    {
        private const string ROC = "Rock";
        private const string PAP = "Paper";
        private const string SCI = "Scissors";

        static void Main(string[] args)
        {
            var d1 = new Dictionary<string, string>
            {
                { "A", ROC },
                { "B", PAP },
                { "C", SCI },
            };

            var d2 = new Dictionary<string, string>
            {
                { "X", ROC },
                { "Y", PAP },
                { "Z", SCI },
            };

            var points = new Dictionary<string, int>
            {
                { ROC, 1 },
                { PAP, 2 },
                { SCI, 3 },
            };

            var part2 = new Dictionary<string, int>
            {
                { "X", 0 },
                { "Y", 3 },
                { "Z", 6 },
            };

            var total = 0;
            File.ReadAllLines("TextFile1.txt").ToList().ForEach(line =>
            {
                var splits = line.Split(' ');
                var him = d1[splits[0]];
                var me = d2[splits[1]];

                var outcome = rps(him, me);
                total += outcome + points[me];
            });

            var part2total = 0;
            File.ReadAllLines("TextFile1.txt").ToList().ForEach(line =>
            {
                var splits = line.Split(' ');
                var him = d1[splits[0]];
                var neededOutcome = part2[splits[1]];

                var mine = new[] { ROC, PAP, SCI }.First(x => rps(him, x) == neededOutcome);

                part2total += neededOutcome + points[mine];
            });
        }

        private static int rps(string him, string me)
        {
            if (him == me)
                return 3;

            if (me == PAP && him == ROC ||
                me == ROC && him == SCI ||
                me == SCI && him == PAP)
                return 6;

            return 0;
        }
    }
}
