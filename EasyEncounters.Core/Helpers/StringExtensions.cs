using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEncounters.Core.Helpers;
public static class StringExtensions
{
    /// <summary>
    /// Splits a string based on the specified delimiters, returning an array containing the splits as well as all instances of
    /// the delimiters.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="separators"></param>
    /// <returns></returns>
    public static string[] SplitInclusive(this string str, char[] separators)
    {
        if (str.Length == 1)
        {
            return new string[] { str };
        }

        var results = new List<string>();
        var lastSplit = 0;
        for(var i = 0; i < str.Length; i++)
        {
            if (separators.Contains(str[i]))
            {
                results.Add(str[lastSplit..i]);
                results.Add(str.Substring(i, 1));
                lastSplit = i;
            }
        }
        results.Add(str[(lastSplit + 1)..]);
        return results.ToArray();
    }

    /// <summary>
    /// Splits an array based on a specified delimiter, without discarding the instances of that delimiter
    /// </summary>
    /// <param name="str"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string[] SplitInclusive(this string str, char separator)
    {
        if (str.Length == 1)
        {
            return new string[] { str };
        }

        var results = new List<string>();
        var lastSplit = 0;
        for (var i = 0; i < str.Length; i++)
        {
            if (str[i] == separator)
            {
                results.Add(str[lastSplit..i]);
                results.Add(str.Substring(i, 1));
                lastSplit = i;
            }
        }
        results.Add(str[(lastSplit + 1)..]);
        return results.ToArray();
    }
}
