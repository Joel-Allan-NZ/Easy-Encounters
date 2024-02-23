﻿using System.Linq.Expressions;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Services.Filter;

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
            if (MatchARange(item, expression, minimum, maximum))
                matches.Add(item);
        }
        return matches;
    }

    public ICollection<T> Filter<T>(ICollection<T> toFilter, Expression<Func<T, string>> expression, string subString)
    {
        var matches = new List<T>();
        foreach (var item in toFilter)
        {
            if (MatchAString(item, expression, subString))
                matches.Add(item);
        }
        return matches;
    }

    public ICollection<T> Filter<T>(ICollection<T> toFilter, FilterValues filterValues, string text)
    {
        //FilterValues should contain, or generate, a filtercriteria set
        //an issue here is that this means filtervalues must be typed
        //which is very messy with subtypes - AbilityFilter must effectively be AbilityFilter<T>  where T : AbilityViewModel
        //likely a better approach to this, TODO: investigate better approaches
        ICollection<FilterCriteria<T>> filterCriteria;
        ICollection<T> filtered = new List<T>();

        if (typeof(T) == typeof(AbilityViewModel))
        {
            var abilityFilterValues = (AbilityFilter)filterValues;
            filterCriteria = (ICollection<FilterCriteria<T>>)abilityFilterValues.GenerateFilterCriteria(text);
            filtered = Filter(toFilter, filterCriteria);

            abilityFilterValues.SearchSuggestions = (List<AbilityViewModel>)filtered;
        }
        else if (typeof(T) == typeof(CreatureViewModel))
        {
            var creatureFilterValues = (CreatureFilter)filterValues;
            filterCriteria = (ICollection<FilterCriteria<T>>)creatureFilterValues.GenerateFilterCriteria(text);
            filtered = Filter(toFilter, filterCriteria);

            creatureFilterValues.SearchSuggestions = (List<CreatureViewModel>)filtered;
        }
        else if (typeof(T) == typeof(EncounterData))
        {
            var encounterFilterValues = (EncounterFilter)filterValues;
            filterCriteria = (ICollection<FilterCriteria<T>>)encounterFilterValues.GenerateFilterCriteria(text);
            filtered = Filter(toFilter, filterCriteria);

            encounterFilterValues.SearchSuggestions = (List<EncounterData>)filtered;
        }

        return filtered;
    }

    public FilterValues GetFilterValues<T>()
    {
        if (typeof(T) == typeof(AbilityViewModel))
        {
            return new AbilityFilter();
        }
        else if (typeof(T) == typeof(EncounterData))
        {
            return new EncounterFilter();
        }
        else if (typeof(T) == typeof(CreatureViewModel))
        {
            return new CreatureFilter();
        }
        //default:
        throw new ArgumentException($"{nameof(T)} is not a supported FilterValue<T> type");

        //todo: something more robust, but this will meet requirements for now
    }

    private void ApplyFilter<T>(ICollection<T> toFilter, FilterCriteria<T> filter)
    {
        var noMatch = new List<T>();
        foreach (var item in toFilter)
        {
            if (!filter.IsMatch(item))
            {
                noMatch.Add(item);
            }
        }
        foreach (var nonMatch in noMatch)
        {
            toFilter.Remove(nonMatch);
        }
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
}

//todo: abstract class with subclasses
public class FilterCriteria<T>
{
    internal Expression<Func<T, IComparable>> Expression;
    internal string? MatchString;
    internal IComparable? Maximum;
    internal IComparable? Minimum;

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

    public bool IsMatch(T item)
    {
        if (item != null)
        {
            if (MatchString != null)
                return MatchAString(item);
            else
                return MatchARange(item);
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

    private bool MatchAString(T item)
    {
        if (String.IsNullOrEmpty(MatchString))
        {
            return true;
        }

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
}