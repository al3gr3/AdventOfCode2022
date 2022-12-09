using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pr4
{
    class Program
    {
        static void Main(string[] args)
        {
            Correct2();
            //Incorrect();
        }

        private static void Incorrect()
        {
            var currentPath = new List<string>();
            var dirs = new Dictionary<string, decimal>();
            File.ReadAllLines("TextFile1.txt").ToList().ForEach(line =>
            {
                var splits = line.Split(' ');
                if (line == "$ cd ..")
                {
                    currentPath.RemoveAt(currentPath.Count() - 1);
                }
                else if (line.StartsWith("$ cd "))
                {
                    currentPath.Add(splits.Last());

                    // if not for this, folders without files are not created and thus results will not be correct
                    var path = string.Join(",", currentPath) + ",";
                    if (!dirs.ContainsKey(path))
                        dirs[path] = 0;
                }
                else if (!line.StartsWith("dir ") && !line.StartsWith("$ ls"))
                {
                    var fileSize = decimal.Parse(splits.First());
                    var path = string.Join(",", currentPath) + ",";
                    dirs[path] += fileSize; // this is not correct if same dir is listed twice
                }
            });
            // this has to be orderBy, not OrderByDescending
            var keys = dirs.Keys.Cast<string>().OrderBy(x => x.Length).ToList();

            keys.ForEach(key =>
            {
                keys.ForEach(subkey =>
                {
                    if (subkey.Length > key.Length && subkey.StartsWith(key))
                    {
                        dirs[key] += dirs[subkey];
                    }
                });
            });
            var result = keys.Select(x => dirs[x]).Where(z => z <= 100000).Sum();

            var unused = 70000000 - dirs["/,"];
            var toFree = 30000000 - unused;
            var result2 = keys.Select(x => dirs[x]).OrderBy(x => x).First(x => x >= toFree);
        }

        private static void Correct()
        {
            var currentPath = new Stack<string>();
            var files = new Dictionary<string, decimal>();
            var dirNames = new HashSet<string>();
            var SEPARATOR = ",";
            File.ReadAllLines("TextFile1.txt").ToList().ForEach(line =>
            {
                var splits = line.Split(' ');
                var path = string.Concat(currentPath.Reverse().Select(x => x + SEPARATOR));

                dirNames.Add(path);

                if (line == "$ cd /")
                {
                    currentPath = new Stack<string>();
                    currentPath.Push("/");
                }
                else if (line == "$ cd ..")
                {
                    if (currentPath.Count > 1)
                        currentPath.Pop();
                }
                else if (line.StartsWith("$ cd "))
                {
                    currentPath.Push(splits.Last());
                }
                else if (!line.StartsWith("dir ") && !line.StartsWith("$ ls"))
                {
                    var fileName = splits.Last();
                    var fileSize = decimal.Parse(splits.First());
                    files[path + fileName] = fileSize;
                }
            });

            var dirs = dirNames.ToDictionary(
                dirKey => dirKey,
                dirKey => files.Keys.Where(fileKey => fileKey.StartsWith(dirKey)).Sum(x => files[x]));

            var result = dirs.Values.Where(z => z <= 100000).Sum();

            var unused = 70_000_000 - dirs["/" + SEPARATOR];
            var toFree = 30_000_000 - unused;
            var result2 = dirs.Values.Where(x => x >= toFree).Min();

            var ok = result == 1077191 && result2 == 5649896;
        }

        private static void Correct2()
        {
            var currentPath = new Stack<string>();
            var dirs = new Dictionary<string, int>();
            var SEPARATOR = ",";
            File.ReadAllLines("TextFile1.txt").ToList().ForEach(line =>
            {
                var splits = line.Split(' ');

                if (line == "$ cd /")
                {
                    currentPath = new Stack<string>();
                    currentPath.Push("/" + SEPARATOR);
                }
                else if (line == "$ cd ..")
                {
                    if (currentPath.Count > 1)
                        currentPath.Pop();
                }
                else if (line.StartsWith("$ cd "))
                {
                    var path = currentPath.Peek() + splits.Last() + SEPARATOR;
                    currentPath.Push(path);
                }
                else if (!line.StartsWith("dir ") && !line.StartsWith("$ ls"))
                {
                    var fileSize = int.Parse(splits.First());
                    currentPath.ToList().ForEach(dir => dirs[dir] = dirs.GetValueOrDefault(dir) + fileSize);
                }
            });

            var result = dirs.Values.Where(z => z <= 100000).Sum();

            var unused = 70_000_000 - dirs["/" + SEPARATOR];
            var toFree = 30_000_000 - unused;
            var result2 = dirs.Values.Where(x => x >= toFree).Min();

            var ok = result == 1077191 && result2 == 5649896;
        }
    }
}
