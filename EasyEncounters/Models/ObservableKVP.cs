using CommunityToolkit.Mvvm.ComponentModel;

namespace EasyEncounters.Models;

public partial class ObservableKVP<T, U> : ObservableObject
{
    [ObservableProperty]
    private T _key;

    [ObservableProperty]
    private U _value;

    public ObservableKVP(T key, U value)
    {
        Key = key;
        Value = value;
    }
}