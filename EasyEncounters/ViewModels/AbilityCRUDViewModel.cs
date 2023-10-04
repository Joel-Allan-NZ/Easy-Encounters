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
using EasyEncounters.Models;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;

namespace EasyEncounters.ViewModels;
public partial class AbilityCRUDViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService _dataService;
    private readonly INavigationService _navigationService;

    private DispatcherQueueTimer _filterTimer;


    public ObservableCollection<AbilityViewModel> Abilities
    {
        get; set;
    } = new();

    [RelayCommand]
    private async void DeleteAbility(object parameter)
    {
        if (parameter != null && parameter is Ability)
        {
            var toDelete = (Ability)parameter;
            Abilities.Remove(Abilities.First(x => x.Ability == toDelete));
            await _dataService.DeleteAsync(toDelete);
        }
    }

    [RelayCommand]
    private async void EditAbility(object ability)
    {
        if(ability != null && ability is Ability)
        {
            _navigationService.NavigateTo(typeof(AbilityEditViewModel).FullName!, ability);
        }
    }

    [RelayCommand]
    private async void CopyAbility(object ability)
    {
        if(ability != null && ability is Ability)
        {
            var copied = await _dataService.CopyAsync(ability as Ability);
            Abilities.Add(new AbilityViewModel(copied));
        }
    }

    [RelayCommand]
    private async void AddAbility()
    {
        var ability = new Ability();
        Abilities.Add(new AbilityViewModel(ability));
        await _dataService.SaveAddAsync(ability);
        EditAbility(ability);
    }

    [RelayCommand]
    private async void Filter(object parameter)
    {
        _filterTimer.Debounce(async () =>
        {

            await FilterAsync(parameter);

        }, TimeSpan.FromSeconds(0.3));
    }



    public AbilityCRUDViewModel(IDataService dataService, INavigationService navigationService)
    {
        _dataService = dataService;
        _navigationService = navigationService;

        var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        _filterTimer = dispatcherQueue.CreateTimer();

        WeakReferenceMessenger.Default.Register<AbilityCRUDRequestMessage>(this, (r, m) =>
        {
            HandleCRUDRequest(m.Parameter.Ability, m.RequestType);
        });

    }
    public void OnNavigatedFrom()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
        _filterTimer.Stop();
    }
    public async void OnNavigatedTo(object parameter)
    {
        Abilities.Clear();
        foreach (var ability in await _dataService.GetAllSpellsAsync())
            Abilities.Add(new AbilityViewModel(ability));
    }

    private void HandleCRUDRequest(Ability ability, CRUDRequestType requestType)
    {
        if (requestType == CRUDRequestType.Delete)
        {
            DeleteAbility(ability);
        }
        else if (requestType == CRUDRequestType.Edit)
        {
            EditAbility(ability);
        }
        else if (requestType == CRUDRequestType.Copy)
        {
            CopyAbility(ability);
        }
    }

    private async Task FilterAsync(object parameter)
    {
        if (parameter is string)
        {
            var text = (string)parameter;

            //remove is worse performance than clearing and repopulating the list, but much less 'flickery'.

            List<AbilityViewModel> matched = Abilities.Where(x => x.Ability.Name.Contains(text, StringComparison.InvariantCultureIgnoreCase)).ToList();
            List<AbilityViewModel> noMatch = new();


            for (var i = Abilities.Count - 1; i >= 0; i--)
            {
                var item = Abilities[i];
                if (!matched.Contains(item))
                {
                    Abilities.Remove(item);
                    noMatch.Add(item);
                }
            }

            foreach (var item in noMatch)
                Abilities.Add(item);

        }
    }
}
