using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.ViewModels;

public partial class DamageTypesViewModel : ObservableRecipient
{
    [ObservableProperty]
    private string _enumString;

    public DamageTypesViewModel(DamageType damageTypes)
    {
        DamageTypes = damageTypes;
        EnumString = DamageTypes.ToString();
    }

    public bool Acid
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Bludgeoning
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Cold
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public DamageType DamageTypes
    {
        get; set;
    }

    public bool Fire
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Force
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Lightning
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Necrotic
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool NonMagicalPhysical
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool NonSilveredPhysical
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Piercing
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Poison
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Psychic
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Radiant
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Slashing
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Thunder
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    private void AddFlag(string name)
    {
        DamageTypes |= (DamageType)Enum.Parse(typeof(DamageType), name);
    }

    private bool Flagged([CallerMemberName] string name = "")
    {
        return DamageTypes.HasFlag((DamageType)Enum.Parse(typeof(DamageType), name));
    }

    private void HandleFlag(bool value, [CallerMemberName] string name = "")
    {
        var flagged = Flagged(name);
        if (flagged != value)
        {
            if (flagged)
                RemoveFlag(name);
            else
                AddFlag(name);
        }
        EnumString = DamageTypes.ToString();
    }

    private void RemoveFlag(string name)
    {
        DamageTypes &= ~(DamageType)Enum.Parse(typeof(DamageType), name);
    }
}