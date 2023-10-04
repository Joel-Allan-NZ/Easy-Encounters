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
using Microsoft.UI.Xaml.Controls;
using Windows.Services.Maps;

namespace EasyEncounters.ViewModels;
public partial class CreatureCRUDViewModel : ObservableRecipient, INavigationAware
{
    public ObservableCollection<CreatureViewModel> Creatures
    {
        get; private set;
    } = new();

    private readonly INavigationService _navigationService;
    private readonly IDataService _dataService;

    private DispatcherQueueTimer _filterTimer;

    public CreatureCRUDViewModel(IDataService dataService, INavigationService navigationService)
    {
        _dataService = dataService;
        _navigationService = navigationService;

        WeakReferenceMessenger.Default.Register<CreatureCopyRequestMessage>(this, (r, m) =>
        {
            CopyCreature(m.Parameter.Creature);
        });
        WeakReferenceMessenger.Default.Register<CreatureDeleteRequestMessage>(this, (r, m) =>
        {
            DeleteCreature(m.Parameter.Creature);
        });
        WeakReferenceMessenger.Default.Register<CreatureEditRequestMessage>(this, (r, m) =>
        {
            EditCreature(m.Parameter);
        });

        var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        _filterTimer = dispatcherQueue.CreateTimer();



    }

    [RelayCommand]
    private void EditCreature(object parameter)
    {
        if(parameter is CreatureViewModel)
        {
            //todo: pass a copy of the creature rather than the original, so changes are discarded if user hits back button rather than committing changes.
            _navigationService.NavigateTo(typeof(CreatureEditViewModel).FullName!, ((CreatureViewModel)parameter).Creature);
        }
    }

    [RelayCommand]
    private async void AddNewCreature()
    {
        var creature = new Creature();
        Creatures.Add(new CreatureViewModel(creature));
        await _dataService.SaveAddAsync(creature);
        _navigationService.NavigateTo(typeof(CreatureEditViewModel).FullName!, creature);
    }

    [RelayCommand]
    private async void DeleteCreature(object parameter)
    {
        if (parameter != null && parameter is Creature)
        {
            var creature = (Creature)parameter;
            Creatures.Remove(Creatures.First(x => x.Creature == creature));
            await _dataService.DeleteAsync(creature);
        }
    }

    [RelayCommand]
    private async void CopyCreature(object parameter)
    {
        if(parameter != null && parameter is Creature)
        {
            var copied = await _dataService.CopyAsync(parameter as Creature);
            if(copied != null)
            {
                Creatures.Add(new CreatureViewModel(copied));
            }

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


    public void OnNavigatedFrom()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
    public async void OnNavigatedTo(object parameter)
    {
        Creatures.Clear();
        foreach (var creature in await _dataService.GetAllCreaturesAsync())
            Creatures.Add(new CreatureViewModel(creature));

    }
}
