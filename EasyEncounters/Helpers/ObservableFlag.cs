using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json.Linq;

namespace EasyEncounters.Helpers;
/// <summary>
/// Represents a single flag value from a Enum with Flags attribute. Has a Flagged property to allow for cleaner
/// display of flag values in the View.
/// </summary>
/// <typeparam name="T"></typeparam>
public partial class ObservableFlag<T> : ObservableObject where T : Enum
{
    [ObservableProperty]
    private T _value;

    [ObservableProperty]
    private bool _flagged;

    public ObservableFlag(T value, bool flagged)
    {
        Value = value;
        Flagged = flagged;
    }
}