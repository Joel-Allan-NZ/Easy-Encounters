using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Helpers;
/// <summary>
/// Represents an Enum with [Flags] attribute, for tidy use of Enums in the View.
/// </summary>
/// <typeparam name="T"></typeparam>
public partial class ObservableFlagEnum<T> : ObservableObject where T: Enum
{
    [ObservableProperty]
    T _enumValue;

    public ObservableCollection<ObservableFlag<T>> Flags
    {
        get; private set;
    } = new();

    /// <summary>
    /// Initializes a new instance of the ObservableFlagEnums<typeparamref name="T"/> with an optional blacklist of excluded flags.
    /// </summary>
    /// <param name="flagEnum"></param>
    /// <param name="excludedFlags">Any flag values that should not be included</param>
    public ObservableFlagEnum(T flagEnum, IEnumerable<T>? excludedFlags = null)
    {
        var valueList = Enum.GetValues(typeof(T))
                         .Cast<T>()
                         .Except(excludedFlags ?? new List<T>())
                         .ToList();
        foreach(var flag in valueList)
        {
            Flags.Add(new(flag, flagEnum.HasFlag(flag)));
        }
        EnumValue = flagEnum;
    }
}


