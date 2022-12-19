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

        }
    }
}
