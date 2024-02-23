using System.Collections.ObjectModel;
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

public partial class EncounterCRUDViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService _dataService;

    private readonly INavigationService _navigationService;

    private readonly DispatcherQueueTimer _filterTimer;

    public EncounterCRUDViewModel(INavigationService navigationService, IDataService dataService)
    {
        _dataService = dataService;
        _navigationService = navigationService;

        WeakReferenceMessenger.Default.Register<EncounterCopyRequestMessage>(this, (r, m) =>
        {
            _ = CopyEncounter(m.Parameter.Encounter);
        });
        WeakReferenceMessenger.Default.Register<EncounterDeleteRequestMessage>(this, (r, m) =>
        {
            _ = DeleteEncounter(m.Parameter.Encounter);
        });
        WeakReferenceMessenger.Default.Register<EncounterEditRequestMessage>(this, (r, m) =>
        {
            EditEncounter(m.Parameter.Encounter);
        });

        var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        _filterTimer = dispatcherQueue.CreateTimer();
    }

    public ObservableCollection<EncounterViewModel> Encounters
    {
        get; private set;
    } = new();

    public void OnNavigatedFrom()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
        _filterTimer.Stop();
    }

    public async void OnNavigatedTo(object parameter)
    {
        Encounters.Clear();
        foreach (var encounter in await _dataService.GetAllEncountersAsync())
            Encounters.Add(new EncounterViewModel(encounter));
    }

    [RelayCommand]
    private async Task AddEncounter()
    {
        var encounter = new Encounter();
        Encounters.Add(new EncounterViewModel(encounter));
        await _dataService.SaveAddAsync(encounter);

        EditEncounter(encounter);
    }

    [RelayCommand]
    private async Task CopyEncounter(object parameter)
    {
        if (parameter != null && parameter is Encounter)
        {
            var copied = await _dataService.CopyAsync(parameter as Encounter);

            if (copied != null)
                Encounters.Add(new EncounterViewModel(copied));
        }
    }

    [RelayCommand]
    private async Task DeleteEncounter(object parameter)
    {
        if (parameter != null && parameter is Encounter)
        {
            var toDelete = (Encounter)parameter;
            Encounters.Remove(Encounters.First(x => x.Encounter == toDelete));
            await _dataService.DeleteAsync(toDelete);
        }
    }

    [RelayCommand]
    private void EditEncounter(object parameter)
    {
        if (parameter != null && parameter is Encounter)
        {
            _navigationService.NavigateTo(typeof(EncounterEditViewModel).FullName!, parameter);
        }
        else if (parameter != null && parameter is EncounterViewModel)
        {
            _navigationService.NavigateTo(typeof(EncounterEditViewModel).FullName!, ((EncounterViewModel)parameter).Encounter);
        }
    }

    [RelayCommand]
    private async Task Filter(object parameter)
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

            List<EncounterViewModel> matched = Encounters.Where(x => x.Encounter.Name.Contains(text, StringComparison.InvariantCultureIgnoreCase)).ToList();
            List<EncounterViewModel> noMatch = new();

            for (var i = Encounters.Count - 1; i >= 0; i--)
            {
                var item = Encounters[i];
                if (!matched.Contains(item))
                {
                    Encounters.Remove(item);
                    noMatch.Add(item);
                }
            }

            foreach (var item in noMatch)
                Encounters.Add(item);
        }
    }
}