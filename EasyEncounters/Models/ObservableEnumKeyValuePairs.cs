using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.FileProviders.Physical;

namespace EasyEncounters.Models;


/// <summary>
/// The more project friendly equivalent of an IEnumerable<TKey,UValue> where both enums have the [flags] attribute.
/// T MUST have a backing flags type of int.
/// </summary>
public partial class ObservableEnumKeyValuePairs<T, U> : ObservableObject where T : struct, Enum
                                                                        where U : Enum
{
    private readonly ICollection<U> _values = Enum.GetValues(typeof(U)).Cast<U>().ToList();
    public ICollection<U> ValuesRange => _values;


    public ObservableCollection<ObservableOptionKVP<T, U>> Pairs
    {
        get; set;
    } = new();

    /// <summary>
    /// Initializes the ObservableEnumKeyValuePairs class, using a collection of ordered T enums representing U enum values.
    /// EG: T Fruit U FruitQuality. Model: T GoodFruit, T BadFruit...  
    /// </summary>
    /// <param name="tFlagsbyuValue">KVP collection in descending order of priority</param>
    public ObservableEnumKeyValuePairs(ICollection<KeyValuePair<T, U>> tFlagsbyuValue)
    {

        SetSkills(tFlagsbyuValue);
    }

    private void SetSkills(ICollection<KeyValuePair<T, U>> tFlagsbyuValue)
    {
        var tFlags = Enum.GetValues(typeof(T)).Cast<T>().Skip(1).ToList();

        Pairs = new();

        foreach (var flag in tFlags)
        {
            foreach (var kvp in tFlagsbyuValue)
            {
                if (kvp.Key.HasFlag(flag))
                {
                    Pairs.Add(new ObservableOptionKVP<T, U>(flag, kvp.Value, ValuesRange));
                    break;
                }
            }
        }

        if (Pairs.Count == 0)
        {
            foreach(var flag in tFlags)
            {
                Pairs.Add(new(flag, tFlagsbyuValue.First().Value, ValuesRange)); //fallback to ensure pairs is populated with the lowest U value if empty
            }
        }
    }

    private static T SetFlag(T value, T flag)
    {
        try
        {
            Unsafe.As<T, int>(ref value) |= Unsafe.As<T, int>(ref flag);
            return value;
        }
        catch
        {
            throw new ArgumentException($"{typeof(T)} does not use int32 as a backing field");
        }
    }


    public Dictionary<U, T> GetFlags()
    {
        Dictionary<U, T> dict = new();

        foreach (var value in ValuesRange)
        {
            T blank = default;

            dict.Add(value, blank);

            //result.Add(new(blank, value));
        }

        foreach (var kvp in Pairs)
        {
            dict[kvp.Value] = SetFlag(dict[kvp.Value], kvp.Key);
            //SetFlag(dict[kvp.Value], ref kvp.Key);           
        }

        return dict;
    }


}
