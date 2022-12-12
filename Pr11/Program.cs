using System;
using System.Collections.Generic;
using System.Linq;

namespace Pr11
{
    class Program
    {
        static void Main(string[] args)
        {
            var monkeys = myInput();
            var divider = monkeys.Select(x => x.Divider).Aggregate((ulong)1, (s, n) => s * n);
            Enumerable.Range(1, 10000).ToList().ForEach(round =>
            {
                monkeys.ToList().ForEach(monkey =>
                {
                    monkey.Items.ForEach(item =>
                    {
                        monkey.Inspection++;
                        var operated = monkey.Operation(item);
                        operated %= divider;
                        if (monkey.Test1(operated))
                            monkeys[monkey.MonkeyIfTrue].Items.Add(operated);
                        else
                            monkeys[monkey.MonkeyIfFalse].Items.Add(operated);
                    });
                    monkey.Items = new List<ulong>();
                });
            });

            var ordered = monkeys.Select(x => x.Inspection).OrderByDescending(x => x).ToArray();
            var result = ordered[0] * ordered[1];
            Console.WriteLine(result);
        }

        private static Monkey[] testInput()
        {
            return new[]
            {
                new Monkey
                {
                    Number = 0,
                    Items = new ulong [] { 79, 98 }.ToList(),
                    Operation = x => x * 19,
                    Test = x => x % 23 == 0,
                    Divider = 23,
                    MonkeyIfTrue = 2,
                    MonkeyIfFalse = 3,
                },
                new Monkey
                {
                    Number = 1,
                    Items = new ulong [] { 54, 65, 75, 74 }.ToList(),
                    Operation = x => x + 6,
                    Test = x => x % 19 == 0,
                    Divider = 19,
                    MonkeyIfTrue = 2,
                    MonkeyIfFalse = 0,
                },
                new Monkey
                {
                    Number = 2,
                    Items = new ulong [] { 79, 60, 97 }.ToList(),
                    Operation = x => x * x,
                    Test = x => x % 13 == 0,
                    Divider = 13,
                    MonkeyIfTrue = 1,
                    MonkeyIfFalse = 3,
                },
                new Monkey
                {
                    Number = 3,
                    Items = new ulong [] { 74 }.ToList(),
                    Operation = x => x + 3,
                    Test = x => x % 17 == 0,
                    Divider = 17,
                    MonkeyIfTrue = 0,
                    MonkeyIfFalse = 1,
                },
            };
        }        
        
        private static Monkey[] myInput()
        {
            return new[]
            {
                new Monkey
                {
                    Number = 0,
                    Items = new ulong [] { 89, 73, 66, 57, 64, 80 }.ToList(),
                    Operation = x => x * 3,
                    Test = x => x % 13 == 0,
                    Divider = 13,
                    MonkeyIfTrue = 6,
                    MonkeyIfFalse = 2,
                },
                new Monkey
                {
                    Number = 1,
                    Items = new ulong [] { 83, 78, 81, 55, 81, 59, 69 }.ToList(),
                    Operation = x => x + 1,
                    Test = x => x % 3 == 0,
                    Divider = 3,
                    MonkeyIfTrue = 7,
                    MonkeyIfFalse = 4,
                },
                new Monkey
                {
                    Number = 2,
                    Items = new ulong [] { 76, 91, 58, 85 }.ToList(),
                    Operation = x => x * 13,
                    Test = x => x % 7 == 0,
                    Divider = 7,
                    MonkeyIfTrue = 1,
                    MonkeyIfFalse = 4,
                },
                new Monkey
                {
                    Number = 3,
                    Items = new ulong [] { 71, 72, 74, 76, 68 }.ToList(),
                    Operation = x => x * x,
                    Test = x => x % 2 == 0,
                    Divider = 2,
                    MonkeyIfTrue = 6,
                    MonkeyIfFalse = 0,
                },
                new Monkey
                {
                    Number = 4,
                    Items = new ulong [] { 98, 85, 84 }.ToList(),
                    Operation = x => x + 7,
                    Test = x => x % 19 == 0,
                    Divider = 19,
                    MonkeyIfTrue = 5,
                    MonkeyIfFalse = 7,
                },

                new Monkey
                {
                    Number = 5,
                    Items = new ulong [] { 78 }.ToList(),
                    Operation = x => x + 8,
                    Test = x => x % 5 == 0,
                    Divider = 5,
                    MonkeyIfTrue = 3,
                    MonkeyIfFalse = 0,
                },
                new Monkey
                {
                    Number = 6,
                    Items = new ulong [] { 86, 70, 60, 88, 88, 78, 74, 83 }.ToList(),
                    Operation = x => x + 4,
                    Test = x => x % 11 == 0,
                    Divider = 11,
                    MonkeyIfTrue = 1,
                    MonkeyIfFalse = 2,
                },
                new Monkey
                {
                    Number = 7,
                    Items = new ulong [] { 81, 58 }.ToList(),
                    Operation = x => x + 5,
                    Test = x => x % 17 == 0,
                    Divider = 17,
                    MonkeyIfTrue = 3,
                    MonkeyIfFalse = 5,
                },
            };
        }

        class Monkey
        {
            internal int Number;
            internal List<ulong> Items;
            internal Func<ulong, ulong> Operation;
            internal Func<ulong, bool> Test;
            internal int MonkeyIfTrue;
            internal int MonkeyIfFalse;
            internal ulong Inspection;
            internal ulong Divider;
            internal Func<ulong, bool> Test1 => x => x % Divider == 0; 
        }
    }
}
