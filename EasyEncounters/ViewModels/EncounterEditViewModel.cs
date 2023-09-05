using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.UI;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Messages;
using Microsoft.UI.Dispatching;

namespace EasyEncounters.ViewModels;
public partial class EncounterEditViewModel : ObservableRecipient, INavigationAware
{
    public ObservableCollection<CreatureViewModel> EncounterCreatures
    {
        get; private set;
    } = new();

    public ObservableCollection<CreatureViewModel> Creatures
    {
        get; private set;
    } = new();

    [ObservableProperty]
    private Encounter _encounter;

    [RelayCommand]
    private async void CommitChanges(object obj)
    {
        await _dataService.SaveAddAsync(Encounter);
        if (_navigationService.CanGoBack)
            _navigationService.GoBack();
    }

    [RelayCommand]
    private void AddCreature(object obj)
    {
        if(obj != null && obj is CreatureViewModel)
        {
            CreatureViewModel creature = (CreatureViewModel)obj;
            EncounterCreatures.Add(new CreatureViewModel(creature.Creature));
            Encounter.Creatures.Add(creature.Creature);
        }
    }

    [RelayCommand]
    private void RemoveCreature(object obj)
    {
        if(obj != null && obj is CreatureViewModel)
        {
            CreatureViewModel toRemove = (CreatureViewModel)obj;
            EncounterCreatures.Remove(EncounterCreatures.First(x => x.Creature == toRemove.Creature));
            Encounter.Creatures.Remove(toRemove.Creature);
        }
    }

    [RelayCommand]
    private async void Filter(object parameter)
    {
        _filterTimer.Debounce(async () =>
        {

            await FilterAsync(parameter);

        }, TimeSpan.FromSeconds(0.3));
    }


    private async Task FilterAsync(object parameter)
    {
        if (parameter is string)
        {
            var text = (string)parameter;

            //remove is worse performance than clearing and repopulating the list, but much less 'flickery'.

            List<CreatureViewModel> matched = Creatures.Where(x => x.Creature.Name.Contains(text, StringComparison.InvariantCultureIgnoreCase)).ToList();
            List<CreatureViewModel> noMatch = new();


            for (int i = Creatures.Count - 1; i >= 0; i--)
            {
                var item = Creatures[i];
                if (!matched.Contains(item))
                {
                    Creatures.Remove(item);
                    noMatch.Add(item);
                }
            }

            foreach (var item in noMatch)
                Creatures.Add(item);

        }
    }

    private readonly INavigationService _navigationService;
    private readonly IDataService _dataService;
    private readonly DispatcherQueueTimer _filterTimer;

    public EncounterEditViewModel(INavigationService navigationService, IDataService dataService)
    {
        _navigationService = navigationService;
        _dataService = dataService;

        var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        _filterTimer = dispatcherQueue.CreateTimer();

        WeakReferenceMessenger.Default.Register<CreatureDeleteRequestMessage>(this, (r, m) =>
        {
            RemoveCreature(m.Parameter);
        });

        WeakReferenceMessenger.Default.Register<CreatureCopyRequestMessage>(this, (r, m) =>
        {
            AddCreature(m.Parameter);
        });
    }


    public void OnNavigatedFrom()
    {
        _filterTimer.Stop();
        WeakReferenceMessenger.Default.UnregisterAll(this);
        
    }

    public async void OnNavigatedTo(object parameter)
    {
        if(parameter != null && parameter is Encounter)
        {
            Encounter = (Encounter)parameter;

            EncounterCreatures.Clear();
            foreach (var creature in Encounter.Creatures)
                EncounterCreatures.Add(new CreatureViewModel(creature));
        }
        else
        {
            Encounter = new Encounter(); //create a new encounter if you aren't editing an existing one.

        }

        Creatures.Clear();
        foreach (var creature in await _dataService.GetAllCreaturesAsync())
            Creatures.Add(new CreatureViewModel(creature));
    }
}
