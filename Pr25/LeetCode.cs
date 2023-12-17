using System.Collections.Generic;
using System.Linq;

public class LeetCode
{
    public IList<string> GenerateParenthesis(int n)
    {
        const int MAX = 8;
        var results = new List<string>[MAX + 1];

        results[1] = new[] { "()" }.ToList();

        for (var i = 2; i <= n; i++)
        { 
            results[i] = results[i - 1].Select(x => "(" + x + ")")
                .Union(Enumerable.Range(1, i - 1).SelectMany(d =>
                {
                    var first = results[i - d];
                    var second = results[d];

                    return first.SelectMany(f => second.SelectMany(s => new[] { s + f, f + s }));
                }))
                .Distinct()
                .ToList();
        }

        return results[n];
    }


    public IList<string> Recurse(int n)
    {
        if (n == 1)
            return new[] { "()" }.ToList();

        return Recurse(n - 1).Select(x => "(" + x + ")")
            .Union(Enumerable.Range(1, n - 1).SelectMany(d =>
            {
                var first = Recurse(n - d);
                var second = Recurse(d);

                return first.SelectMany(f => second.SelectMany(s => new[] { s + f, f + s }));
            }))
            .Distinct()
            .ToList();
    }
}