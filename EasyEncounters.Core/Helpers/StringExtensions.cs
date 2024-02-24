using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEncounters.Core.Helpers;
public static class StringExtensions
{
    public static string[] SplitInclusive(this string str, char[] separators)
    {
        if (str.Length == 1)
        {
            return new string[] { str };
        }

        List<string> results = new List<string>();
        int lastSplit = 0;
        for(var i = 0; i < str.Length; i++)
        {
            if (separators.Contains(str[i]))
            {
                results.Add(str.Substring(lastSplit, i - lastSplit));
                results.Add(str.Substring(i, 1));
                lastSplit = i;
            }
        }
        results.Add(str.Substring(lastSplit+1));
        return results.ToArray();
    }

    public static string[] SplitInclusive(this string str, char separator)
    {
        if (str.Length == 1)
        {
            return new string[] { str };
        }

        List<string> results = new List<string>();
        int lastSplit = 0;
        for (var i = 0; i < str.Length; i++)
        {
            if (str[i] == separator)
            {
                results.Add(str.Substring(lastSplit, i - lastSplit));
                results.Add(str.Substring(i, 1));
                lastSplit = i;
            }
        }
        results.Add(str.Substring(lastSplit + 1));
        return results.ToArray();
    }
}
