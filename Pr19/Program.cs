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
#else
            var lines = File.ReadAllLines("TextFile2.txt");
#endif
            var blueprints = lines.Select(line =>
            {
                //Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.
                var splits = line.Split(new[] { "costs ", " ore", "Blueprint ", ": Each", "and ", " clay", " obsidian" }, StringSplitOptions.None);

                return new BluePrint
                {
                    Name = int.Parse(splits[1]),
                    Prices = new[]
                    {
                        new Price
                        {
                            Ore = int.Parse(splits[4]),
                            Item = ItemEnum.Ore,
                        },
                        new Price
                        {
                            Ore = int.Parse(splits[7]),
                            Item = ItemEnum.Clay,
                        },
                        new Price
                        {
                            Ore = int.Parse(splits[10]),
                            Clay = int.Parse(splits[12]),
                            Item = ItemEnum.Obsidian,
                        },
                        new Price
                        {
                            Ore = int.Parse(splits[14]),
                            Obsidian = int.Parse(splits[16]),
                            Item = ItemEnum.Geode,
                        }
                    }.OrderByDescending(x => x.Item).ToArray(),
                };
            }).ToList();

            blueprints.ForEach(blueprint =>
            {
                letsgo(blueprint);
            });

            var result = blueprints.Sum(x => x.Value);
            Console.WriteLine(result);
        }

        private static void letsgo(BluePrint blueprint)
        {
            var maxDays = 24;
            var dayStates = Enumerable.Range(1, maxDays + 2).Select(x => new List<DayState>()).ToArray();
            dayStates[1].Add(new DayState
            {
                RobotOre = 1
            });

            for (var days = 1; days <= maxDays; days++)
            {
                fixStates(dayStates[days]);
                dayStates[days].ForEach(daystate =>
                {
                    // what can we buy?
                    blueprint.Prices.ToList().ForEach(price =>
                    {
                        var nextDay = daystate.Clone();
                        if (nextDay.CanBuy(price))
                        {
                            nextDay.WorkRobots(); // before buying robots
                            nextDay.Buy(price);

                            //if (!dayStates[days + 1].Any() || dayStates[days + 1].Any(x => isSlightlyWorse(x, nextDay))) // cannot really remove all that is worse
                                dayStates[days + 1].Add(nextDay); // should remove all that are really worse (like -1)
                        }
                    });

                    // not buying anything
                    var nextDay = daystate.Clone();
                    nextDay.WorkRobots();
                    dayStates[days + 1].Add(nextDay);
                });
            }
            blueprint.Max = dayStates[maxDays + 1].Max(x => x.Geode);
            Console.WriteLine(blueprint.Max);
        }

        private static void fixStates(List<DayState> dayStates)
        {
            if (dayStates.Any(x => x.RobotObsidian > 0))
                dayStates.RemoveAll(x => x.RobotObsidian == 0);
            
            if (dayStates.Any(x => x.RobotGeode > 0))
                dayStates.RemoveAll(x => x.RobotGeode == 0);
            else
                dayStates.RemoveAll(x => dayStates.Any(y =>
                    y != x &&
                    x.Ore <= y.Ore && 
                    x.Clay <= y.Clay && 
                    x.Obsidian <= y.Obsidian && 
                    x.Geode <= y.Geode && 
                    x.RobotOre <= y.RobotOre && 
                    x.RobotClay <= y.RobotClay && 
                    x.RobotGeode <= y.RobotGeode &&
                    x.RobotObsidian <= y.RobotObsidian));
        }

        /// <summary>
        /// x < y
        /// </summary>
        private static bool isSlightlyWorse(DayState x, DayState y)
        {
            return x.Obsidian < y.Obsidian || x.Ore < y.Ore || x.Clay < y.Clay ||
                x.RobotClay < y.RobotClay || x.RobotGeode < y.RobotGeode || x.RobotObsidian < y.RobotObsidian || x.RobotOre < y.RobotOre;
        }
    }

    internal class DayState
    {
        internal int Ore;
        internal int Clay;
        internal int Obsidian;
        internal int Geode;

        internal int RobotOre;
        internal int RobotClay;
        internal int RobotObsidian;
        internal int RobotGeode;

        internal void Buy(Price price)
        {
            Clay = this.Clay - price.Clay;
            Obsidian = this.Obsidian - price.Obsidian;
            Ore = this.Ore - price.Ore;

            RobotClay = this.RobotClay + (price.Item == ItemEnum.Clay ? 1 : 0);
            RobotOre = this.RobotOre + (price.Item == ItemEnum.Ore ? 1 : 0);
            RobotObsidian = this.RobotObsidian + (price.Item == ItemEnum.Obsidian ? 1 : 0);
            RobotGeode = this.RobotGeode + (price.Item == ItemEnum.Geode ? 1 : 0);
        }

        internal bool CanBuy(Price price) => price.Ore <= this.Ore && price.Clay <= this.Clay && price.Obsidian <= this.Obsidian;

        internal DayState Clone() => new DayState
        {
            Clay = Clay,
            Obsidian = Obsidian,
            Geode = Geode,
            Ore = Ore,
            RobotClay = RobotClay,
            RobotGeode = RobotGeode,
            RobotObsidian = RobotObsidian,
            RobotOre = RobotOre,
        };

        internal void WorkRobots()
        {
            this.Clay += this.RobotClay;
            this.Ore += this.RobotOre;
            this.Obsidian += this.RobotObsidian;
            this.Geode += this.RobotGeode;
        }
    }

    internal class BluePrint
    {
        internal Price[] Prices;

        internal int Name;

        internal int Max;
        internal int Value => Name * Max;
    }

    internal class Price
    {
        internal int Ore;
        internal int Clay;
        internal int Obsidian;
        internal ItemEnum Item;
    }

    internal enum ItemEnum
    {
        Ore = 1, 
        Clay = 2,
        Obsidian = 3,
        Geode = 4
    }
}
