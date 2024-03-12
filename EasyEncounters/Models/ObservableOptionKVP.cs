using CommunityToolkit.Mvvm.ComponentModel;

namespace EasyEncounters.Models;

/// <summary>
/// An observable KVP, but with a specified range of options for the value.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="U"></typeparam>
public partial class ObservableOptionKVP<T,U> : ObservableKVP<T,U>
{
    [ObservableProperty]
    private T _key;

    [ObservableProperty]
    private U _value;

    [ObservableProperty]
    private List<U> _items;

    public ObservableOptionKVP(T key, U value, ICollection<U> items) :base(key, value)
    {
        Key = key;
        Value = value;
        Items = items.ToList() ?? new List<U>();
    }
}