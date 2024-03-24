using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Helpers;
using EasyEncounters.Helpers.Conditions;
using EasyEncounters.Messages;
using EasyEncounters.Models;
using Microsoft.UI.Xaml;

namespace EasyEncounters.ViewModels;

public partial class ObservableActiveEncounterCreature : ObservableRecipient 
{
    public ObservableCollection<ConditionStatus> Conditions
    {
        get; private set;
    } = new();

    [ObservableProperty]
    private string _creatureInfoString;

    [ObservableProperty]
    private int _currentHP;

    [ObservableProperty]
    private SpellSlotViewModel _spellSlots;

    [ObservableProperty]
    private ActiveEncounterCreature creature;

    [ObservableProperty]
    private string enumString;

    [ObservableProperty]
    private bool targeted;

    [ObservableProperty]
    private Thickness targetVisibility;

    [ObservableProperty]
    private string _skillsString;

    [ObservableProperty]
    private bool _hasSkills;

    public ObservableActiveEncounterCreature(ActiveEncounterCreature creature)
    {
        this.Creature = creature;
        TargetVisibility = new Thickness(0);
        CurrentHP = creature.CurrentHP;

        foreach (var activeAbility in creature.ActiveAbilities)
            Abilities.Add(new ObservableActiveAbility(activeAbility));

        SpellSlots = new SpellSlotViewModel(creature.SpellSlots);
        SetConditionStatus(creature.ActiveConditions);
        EnumString = creature.ActiveConditions.ToString();
        SetCreatureInfoString();

    }

    public void HandleSkills(IEnumerable<KeyValuePair<CreatureSkills, int>> bonuses)
    {
        SkillsString = "";
        foreach(var bonus in bonuses)
        {
            SkillsString += $"{ResourceExtensions.GetEnumerationString(bonus.Key)} +{bonus.Value}, "; 
        }
        if (SkillsString.Length > 0)
        {
            SkillsString = SkillsString[..^2];
            HasSkills = true;
        }
        else
        {
            HasSkills = false;
        }
    }

    private void SetCreatureInfoString()
    {
        CreatureInfoString = $"{Creature.Size} {Creature.CreatureType}";
        if (Creature.CreatureSubtype != null)
        {
            CreatureInfoString += $" ({Creature.CreatureSubtype})";
        }
        CreatureInfoString += $", {ResourceExtensions.GetEnumerationString(Creature.Alignment)}";
    }

    public event EventHandler ConditionsChanged;

    public ObservableCollection<ObservableActiveAbility> Abilities
    {
        get; private set;
    } = new();

    //set => SetProperty(_ability.Concentration, value, _ability, (m, v) => m.Concentration = v);
    public bool Concentrating
    {
        get => Creature.Concentrating;
        set => SetProperty(Creature.Concentrating, value, Creature, (m, v) => m.Concentrating = v);
    }

    public bool Dead
    {
        get => this.Creature.Dead;
        set
        {
            if (this.Creature.Dead != value)
            {
                this.Creature.Dead = value;
                OnPropertyChanged();
            }
        }
    }

    public bool HasConditionImmunities => this.Creature.ConditionImmunities > Core.Models.Enums.Condition.None;

    public bool HasFeatures => this.Creature.Features?.Length > 1;

    public bool HasImmunities => this.Creature.Immunity > Core.Models.Enums.DamageType.None;

    public bool HasResists => this.Creature.Resistance > Core.Models.Enums.DamageType.None;

    public bool HasSpellSlots => this.Creature.SpellSlots.Any(x => x > 0);

    public bool HasVulnerabilities => this.Creature.Vulnerability > Core.Models.Enums.DamageType.None;

    public int Initiative
    {
        get => Creature.Initiative;
        set => SetProperty(Creature.Initiative, value, Creature, (m, v) => m.Initiative = v);
    }

    public string Notes
    {
        get => this.Creature.Notes;
        set
        {
            this.Creature.Notes = value;
            OnPropertyChanged();
        }
    }

    public bool Reaction
    {
        get => this.Creature.Reaction;
        set
        {
            if (this.Creature.Reaction != value)
            {
                this.Creature.Reaction = value;
                OnPropertyChanged();
            }
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj != null && obj is ObservableActiveEncounterCreature)
        {
            var other = (ObservableActiveEncounterCreature)obj;

            return other.Creature.Equals(this.Creature);
        }
        return false;
    }

    public override int GetHashCode() => Creature.GetHashCode();

    /// <summary>
    /// Helper to check equality between this ActiveEncounterCreature's Creature property and the given parameter creature.
    /// </summary>
    /// <param name="creature"></param>
    /// <returns></returns>
    public bool IsWrapperFor(ActiveEncounterCreature creature)
    {
        return (this.Creature.Equals(creature));
    }

    //[RelayCommand]
    //private void AddTargetRequested()
    //{
    //    WeakReferenceMessenger.Default.Send(new AddTargetCreatureRequestMessage(this));
    //}

    [RelayCommand]
    private void DamageRequested()
    {
        WeakReferenceMessenger.Default.Send(new DamageSourceSelectedMessage(this));
    }

    [RelayCommand]
    private void InspectRequested()
    {
        WeakReferenceMessenger.Default.Send(new InspectRequestMessage(this));
    }

    partial void OnTargetedChanged(bool value)
    {
        if (value)
        {
            TargetVisibility = new Thickness(2);
        }
        else
            TargetVisibility = new Thickness(0);
    }

    [RelayCommand]
    private void ToggleTarget()
    {
        Targeted = !Targeted;
    }

    private void SetConditionStatus(Condition conditions)
    {
        var conditionsList = Enum.GetValues(typeof(Condition))
                                 .Cast<Condition>()
                                 .Except(new List<Condition>() { Condition.None, Condition.All })
                                 .ToList();

        foreach (var condition in conditionsList)
        {
            Conditions.Add(new(condition, conditions.HasFlag(condition)));
            Conditions.Last().PropertyChanged += OnConditionChanged;
        }
    }

    private void OnConditionChanged(object? sender, PropertyChangedEventArgs e)
    {
        if(sender is ConditionStatus conditionStatus)
        {
            Creature.ActiveConditions ^= conditionStatus.Condition;
            EnumString = Creature.ActiveConditions.ToString();
        }
    }
}