using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.ViewModels;

public partial class ConditionsHelper : ObservableRecipient
{
    [ObservableProperty]
    private string _enumString;

    public ConditionsHelper(Condition condition)
    {
        ConditionTypes = condition;
        EnumString = ConditionTypes.ToString();
    }

    public bool Blinded
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Charmed
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public Condition ConditionTypes
    {
        get; set;
    }

    public bool Deafened
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Exhausted
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Frightened
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Grappled
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Incapacitated
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Paralyzed
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Petrified
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Poisoned
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Prone
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Restrained
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Stunned
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool Unconscious
    {
        get => Flagged();
        set => HandleFlag(value);
    }

    public bool HasCondition(string conditionName)
    {
        return Flagged(conditionName);
        
    }

    public void ToggleCondition(string conditionName)
    {
        HandleFlag(!Flagged(conditionName), conditionName);
    }


    private void AddFlag(string name)
    {
        ConditionTypes |= (Condition)Enum.Parse(typeof(Condition), name);
    }

    private bool Flagged([CallerMemberName] string? name = null)
    {
        if (name != null)
            return ConditionTypes.HasFlag((Condition)Enum.Parse(typeof(Condition), name));
        else
            throw new ArgumentException("Null is a not a valid Flag Property Name");
    }

    private void HandleFlag(bool value, [CallerMemberName] string? name = null)
    {
        if (name != null)
        {
            var flagged = Flagged(name);
            if (flagged != value)
            {
                if (flagged)
                    RemoveFlag(name);
                else
                    AddFlag(name);
            }
            EnumString = ConditionTypes.ToString();
        }
    }

    private void RemoveFlag(string name)
    {
        ConditionTypes &= ~(Condition)Enum.Parse(typeof(Condition), name);
    }
}