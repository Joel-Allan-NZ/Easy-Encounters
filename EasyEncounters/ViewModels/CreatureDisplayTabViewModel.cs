using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Services;
using EasyEncounters.Messages;
using EasyEncounters.Models;

namespace EasyEncounters.ViewModels;
public partial class CreatureDisplayTabViewModel : ObservableRecipientTab
{

    [ObservableProperty]
    private ActiveEncounterCreature? _creature;

    [ObservableProperty]
    private ActiveEncounterCreatureViewModel _creatureVM;

    public ObservableCollection<ObservableActiveAbility> Abilities
    {
        get; private set;
    } = new();

    

    public CreatureDisplayTabViewModel()
    {
        WeakReferenceMessenger.Default.Register<UseAbilityRequestMessage>(this, (r, m) =>
        {
            if(Abilities.Contains(m.Parameter))
                WeakReferenceMessenger.Default.Send(new AbilityDamageRequestMessage(m.Parameter, CreatureVM));
        });
        
    }

    [RelayCommand]
    private void RequestDamage()
    {
        WeakReferenceMessenger.Default.Send(new AbilityDamageRequestMessage(null, CreatureVM));
    }

    public override void OnTabClosed()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
    public override void OnTabOpened(object parameter)
    {
        if (parameter != null && parameter is ActiveEncounterCreatureViewModel)
        {
            //var vm = parameter as ActiveEncounterCreature;
            CreatureVM = (ActiveEncounterCreatureViewModel)parameter;
            Creature = CreatureVM.Creature; //parameter as ActiveEncounterCreature;

            Abilities.Clear();
            foreach (var activeAbility in Creature.ActiveAbilities)
                Abilities.Add(new ObservableActiveAbility(activeAbility));

            //Conditions = new ConditionTypesViewModel(Creature.ActiveConditions);
        }
    }
}
