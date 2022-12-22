using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        }
    }
}
