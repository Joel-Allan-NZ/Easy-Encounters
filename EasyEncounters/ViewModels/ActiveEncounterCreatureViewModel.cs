using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EasyEncounters.Core.Models;
using EasyEncounters.Messages;
using EasyEncounters.Models;
using Microsoft.UI.Xaml;
using Newtonsoft.Json.Linq;

namespace EasyEncounters.ViewModels;
public partial class ActiveEncounterCreatureViewModel : ObservableRecipient //todo: wrapper proxies for health etc, so tabs can show them accurately.
{
    [ObservableProperty]
    private ActiveEncounterCreature creature;

    [ObservableProperty]
    private Thickness targetVisibility;

    [ObservableProperty]
    private bool targeted;

    [ObservableProperty]
    private SpellSlotViewModel _spellSlots;

    [ObservableProperty]
    private int _currentHP;

    [ObservableProperty]
    private ConditionTypesViewModel _conditions;

    public int Initiative
    {
        get => Creature.Initiative;
        set => SetProperty(Creature.Initiative, value, Creature, (m, v) => m.Initiative = v);
    }

    public bool HasResists => this.Creature.Resistance > Core.Models.Enums.DamageType.None;

    public bool HasImmunities => this.Creature.Immunity > Core.Models.Enums.DamageType.None;

    public bool HasVulnerabilities => this.Creature.Vulnerability > Core.Models.Enums.DamageType.None;

    public bool HasFeatures => this.Creature.Features.Length > 1;

    public bool HasConditionImmunities => this.Creature.ConditionImmunities > Core.Models.Enums.Condition.None;

    public bool HasSpellSlots => this.Creature.SpellSlots.Any(x => x.Value > 0);

    partial void OnTargetedChanged(bool value)
    {
        if (value)
        {
            TargetVisibility = new Thickness(2);
        }
        else
            TargetVisibility = new Thickness(0);
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

    public bool Dead
    {
        get => this.Creature.Dead;
        set
        {
            if(this.Creature.Dead != value)
            {
                this.Creature.Dead = value;
                OnPropertyChanged();
            }
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
    //set => SetProperty(_ability.Concentration, value, _ability, (m, v) => m.Concentration = v);
    public bool Concentrating
    {
        get => Creature.Concentrating;
        set => SetProperty(Creature.Concentrating, value, Creature, (m, v) => m.Concentrating = v);
    }

    public ObservableCollection<ObservableActiveAbility> Abilities
    {
        get; private set;
    } = new();

    [RelayCommand]
    private void ToggleTarget()
    {
        Targeted = !Targeted;
    }

    [RelayCommand]
    private void DamageRequested()
    {
        WeakReferenceMessenger.Default.Send(new DamageSourceSelectedMessage(this));
    }

    [RelayCommand]
    private void AddTargetRequested()
    {
        WeakReferenceMessenger.Default.Send(new AddTargetCreatureRequestMessage(this));
    }

    [RelayCommand]
    private void InspectRequested()
    {
        WeakReferenceMessenger.Default.Send(new InspectRequestMessage(this));
    }

    /// <summary>
    /// Helper to check equality between this ActiveEncounterCreature's Creature property and the given parameter creature.
    /// </summary>
    /// <param name="creature"></param>
    /// <returns></returns>
    public bool IsWrapperFor(ActiveEncounterCreature creature)
    {
        
        return (this.Creature.Equals(creature));
        
    }



    public ActiveEncounterCreatureViewModel(ActiveEncounterCreature creature)
    {
        this.Creature = creature;
        TargetVisibility = new Thickness(0);
        CurrentHP = creature.CurrentHP;

        foreach (var activeAbility in creature.ActiveAbilities)
            Abilities.Add(new ObservableActiveAbility(activeAbility));

        SpellSlots = new SpellSlotViewModel(creature.SpellSlots);
        Conditions = new ConditionTypesViewModel(creature.ActiveConditions);
    }

    public override bool Equals(object? obj)
    {
        if (obj != null && obj is ActiveEncounterCreatureViewModel)
        {
            var other = (ActiveEncounterCreatureViewModel)obj;

            return other.Creature.Equals(this.Creature);
        }
        return false;
    }

    public override int GetHashCode() => Creature.GetHashCode();    
}


