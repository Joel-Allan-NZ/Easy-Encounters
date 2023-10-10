using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Helpers;
using Microsoft.AppCenter.Channel;
using Windows.Networking.NetworkOperators;
using Windows.Security.ExchangeActiveSyncProvisioning;
using static System.Net.Mime.MediaTypeNames;

namespace EasyEncounters.Services;
public class FilteringService : IFilteringService
{
    //private Dictionary<object, Dictionary<string, >>
    //private Dictionary<object, List<Dictionary>> (nested dictionaries of generic types; string, FilterCriteria<T>
    public ICollection<T> Filter<T>(ICollection<T> toFilter, ICollection<FilterCriteria<T>> filters)
    {
        List<T> filtered = new(toFilter);

        foreach (var filter in filters)
        {
            ApplyFilter(filtered, filter);
        }
        return filtered;
    }

    public ICollection<T> Filter<T>(ICollection<T> toFilter, FilterCriteria<T> filter)
    {
        List<T> filtered = new(toFilter);
        ApplyFilter(filtered, filter);
        return filtered;
    }

    public ICollection<T> Filter<T>(ICollection<T> toFilter, Expression<Func<T, IComparable>> expression, IComparable minimum, IComparable maximum)
    {
        var matches = new List<T>();
        foreach (var item in toFilter)
        {
            if(MatchARange(item, expression, minimum, maximum))
                matches.Add(item);
        }
        return matches;
    }

    public ICollection<T> Filter<T>(ICollection<T> toFilter, Expression<Func<T, string>> expression, string subString)
    {
        var matches = new List<T>();
        foreach(var item in toFilter)
        {
            if (MatchAString(item, expression, subString))
                matches.Add(item);
        }
        return matches;
    }

    private bool MatchARange<T>(T item, Expression<Func<T, IComparable>> expression, IComparable minimum, IComparable maximum)
    {
        if (expression is LambdaExpression lambdaBody)
        {
            var compiled = lambdaBody.Compile();
            var value = (IComparable?)compiled?.DynamicInvoke(item);

            if (value != null)
            {
                return value.CompareTo(minimum) >= 0 && value.CompareTo(maximum) <= 0;
            }
        }
        return false;
    }

    private bool MatchAString<T>(T item, Expression<Func<T, string>> expression, string subString)
    {
        if (expression is LambdaExpression lambdaBody)
        {
            var compiled = lambdaBody.Compile();
            var value = (string?)compiled?.DynamicInvoke(item);

            if (value != null && value.Contains(subString, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    private void ApplyFilter<T>(ICollection<T> toFilter, FilterCriteria<T> filter)
    {
        var noMatch = new List<T>();
        foreach(var item in toFilter)
        {
            if (!filter.IsMatch(item))
            {
                noMatch.Add(item);
            }
        }
        foreach(var nonMatch in noMatch)
        {
            toFilter.Remove(nonMatch);
        }
    }
}

//todo: abstract class with subclasses
public class FilterCriteria<T>
{
    internal Expression<Func<T, IComparable>> Expression;
    internal IComparable Minimum;
    internal IComparable Maximum;
    internal string MatchString;

    public FilterCriteria(Expression<Func<T, IComparable>> expression, IComparable minimum, IComparable maximum)
    {
        Expression = expression;
        Minimum = minimum;
        Maximum = maximum;
    }

    public FilterCriteria(Expression<Func<T, IComparable>> expression, string matchString)
    {
        Expression = expression;
        MatchString = matchString;
    }

    private bool MatchAString(T item)
    {
        if (Expression is LambdaExpression lambdaBody)
        {
            var compiled = lambdaBody.Compile();
            var value = (string?)compiled?.DynamicInvoke(item);

            if (value != null && value.Contains(MatchString, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }
    private bool MatchARange(T item)
    {
        if (Expression is LambdaExpression lambdaBody)
        {
            var compiled = lambdaBody.Compile();
            var value = (IComparable?)compiled?.DynamicInvoke(item);

            if (value != null)
            {
                return value.CompareTo(Minimum) >= 0 && value.CompareTo(Maximum) <= 0;
            }
        }
        return false;
    }

    public bool IsMatch(T item)
    {
        if(item != null)
        {
            if (MatchString != null)
                return MatchAString(item);
            else
                return MatchARange(item);

        }
        return false;
    }
}


