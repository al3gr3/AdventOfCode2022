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
                    },
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
            var dayStates = Enumerable.Range(1, 26).Select(x => new List<DayState>()).ToArray();
            dayStates[1].Add(new DayState
            {
                RobotOre = 1
            });
            for (var days = 1; days <= 24; days++)
            {
                dayStates[days].ForEach(daystate =>
                {
                    var nextDay = new DayState
                    {
                        Clay = daystate.Clay + daystate.RobotClay,
                        Ore = daystate.Ore + daystate.RobotOre,
                        Obsidian = daystate.Obsidian + daystate.RobotObsidian,
                        RobotClay = daystate.RobotClay,
                        RobotOre = daystate.RobotOre,
                        RobotObsidian = daystate.RobotObsidian,
                        RobotGeode = daystate.RobotGeode,
                    };

                    // not buying anything
                    dayStates[days + 1].Add(nextDay.Clone());

                    // what can we buy?
                    blueprint.Prices.ToList().ForEach(price =>
                    {
                        if (price.Ore <= nextDay.Ore && price.Clay <= nextDay.Clay && price.Obsidian <= nextDay.Obsidian)
                        {
                            var newState = new DayState
                            {
                                Clay = nextDay.Clay - price.Clay,
                                Obsidian = nextDay.Obsidian - price.Obsidian,
                                Ore = nextDay.Ore - price.Ore,
                                Geode = nextDay.Geode,
                                RobotClay = nextDay.RobotClay + (price.Item == ItemEnum.Clay ? 1 : 0),
                                RobotOre = nextDay.RobotOre + (price.Item == ItemEnum.Ore ? 1 : 0),
                                RobotObsidian = nextDay.RobotObsidian + (price.Item == ItemEnum.Obsidian ? 1 : 0),
                                RobotGeode = nextDay.RobotGeode + (price.Item == ItemEnum.Geode ? 1 : 0),
                            };
                            if (dayStates[days + 1].Any(x => isWorse(x, newState))) // cannot really remove all that is worse
                                dayStates[days + 1].Add(newState); // should remove all that are really worse (like -1)
                        }
                    });

                });
            }
            blueprint.Max = dayStates[24].Max(x => x.RobotGeode);
        }

        /// <summary>
        /// Y < X
        /// </summary>
        private static bool isWorse(DayState y, DayState x)
        {
            return y.Obsidian < x.Obsidian || y.Ore < x.Ore || y.Geode < x.Geode || y.Clay < x.Clay ||
                y.RobotClay < x.RobotClay || y.RobotGeode < x.RobotGeode || y.RobotObsidian < x.RobotObsidian || y.RobotOre < x.RobotOre;

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

        internal DayState Clone()
        {
            return new DayState
            {
                Ore = Ore,
                Clay = Clay,
                Geode = Geode,
                Obsidian = Obsidian,
                RobotClay = RobotClay,
                RobotGeode = RobotGeode,
                RobotObsidian = RobotObsidian,
                RobotOre = RobotOre,
            };
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
        Ore, 
        Clay,
        Obsidian,
        Geode
    }
}
