﻿using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EasyEncounters.Core.Models;
using EasyEncounters.Messages;
using EasyEncounters.Models;

namespace EasyEncounters.ViewModels;

public partial class CreatureDisplayTabViewModel : ObservableRecipientTab
{
    [ObservableProperty]
    private ActiveEncounterCreature? _creature;

    [ObservableProperty]
    private ActiveEncounterCreatureViewModel? _creatureVM;

    public CreatureDisplayTabViewModel()
    {
        WeakReferenceMessenger.Default.Register<UseAbilityRequestMessage>(this, (r, m) =>
        {
            if (Abilities.Contains(m.Parameter) && CreatureVM != null)
                WeakReferenceMessenger.Default.Send(new AbilityDamageRequestMessage(m.Parameter, CreatureVM));
        });
    }

    public ObservableCollection<ObservableActiveAbility> Abilities
    {
        get; private set;
    } = new();

    public override void OnTabClosed()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public override void OnTabOpened(object? parameter)
    {
        if (parameter is not null and ActiveEncounterCreatureViewModel creatureVM)
        {
            CreatureVM = creatureVM;
            Creature = CreatureVM.Creature; //parameter as ActiveEncounterCreature;

            Abilities.Clear();
            foreach (var activeAbility in Creature.ActiveAbilities)
                Abilities.Add(new ObservableActiveAbility(activeAbility));
        }
    }

    [RelayCommand]
    private void RequestDamage()
    {
        if (CreatureVM != null)
            WeakReferenceMessenger.Default.Send(new AbilityDamageRequestMessage(null, CreatureVM));
    }
}