using System.IO;
using System.Linq;

namespace Pr4
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
                [S] [C]         [Z]            
            [F] [J] [P]         [T]     [N]    
            [G] [H] [G] [Q]     [G]     [D]    
            [V] [V] [D] [G] [F] [D]     [V]    
            [R] [B] [F] [N] [N] [Q] [L] [S]    
            [J] [M] [M] [P] [H] [V] [B] [B] [D]
            [L] [P] [H] [D] [L] [F] [D] [J] [L]
            [D] [T] [V] [M] [J] [N] [F] [M] [G]
             1   2   3   4   5   6   7   8   9 
            */
            var s = new[]
            {
                "FGVRJLD",
                "SJHVBMPT",
                "CPGDFMHV",
                "QGNPDM",
                "FNHLJ",
                "ZTGDQVFN",
                "LBDF",
                "NDVSBJM",
                "DLG",
            };
            /*
            s = new[]
            {
                "NZ",
                "DCM",
                "P"
            };*/
            var lines = File.ReadAllLines("TextFile1.txt");
            lines.ToList().ForEach(line =>
            {
                //move 1 from 2 to 1
                var splits = line.Split(' ');

                var amount = int.Parse(splits[1]);
                var from = int.Parse(splits[3]) - 1;
                var to = int.Parse(splits[5]) - 1;

                s[to] = string.Concat(s[from].Take(amount).Reverse()) + s[to];
                s[from] = string.Concat(s[from].Skip(amount));
            });

            var result = string.Concat(s.Select(x => x.First()));
        }
    }
}
