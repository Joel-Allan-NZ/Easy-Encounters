using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.ViewModels;
public partial class DamageTypesViewModel: ObservableRecipient
{
    public DamageType DamageTypes
    {
        get; set;
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

    public bool NonSilveredPhysical
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool NonMagicalPhysical
    {
        get => Flagged();
        set => HandleFlag(value);
    }


    public DamageTypesViewModel(DamageType damageTypes)
    {
        DamageTypes = damageTypes;
    }

    private void HandleFlag(bool value, [CallerMemberName] string name = null)
    {
        var flagged = Flagged(name);
        if (flagged != value)
        {
            if (flagged)
                RemoveFlag(name);
            else
                AddFlag(name);
        }
    }
    private bool Flagged([CallerMemberName] string name = null)
    {
        return DamageTypes.HasFlag((DamageType)Enum.Parse(typeof(DamageType), name));
    }

    private void AddFlag(string name)
    {
        DamageTypes |= (DamageType)Enum.Parse(typeof(DamageType), name);
    }

    private void RemoveFlag(string name)
    {
        DamageTypes &= ~(DamageType)Enum.Parse(typeof(DamageType), name);
    }

}
