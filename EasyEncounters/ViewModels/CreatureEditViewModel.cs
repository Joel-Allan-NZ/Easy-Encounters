using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;

namespace EasyEncounters.ViewModels;
public partial class CreatureEditViewModel : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IDataService _dataService;

    [ObservableProperty]
    private Creature _creature;

    [ObservableProperty]
    private DamageTypesViewModel _resists;

    [ObservableProperty]
    private DamageTypesViewModel _immunities;

    [ObservableProperty]
    private DamageTypesViewModel _vulnerabilities;

    [RelayCommand]
    private async void CommitChanges(object obj)
    {
        await _dataService.SaveAddAsync(Creature);
        if (_navigationService.CanGoBack)
            _navigationService.GoBack();
    }

    public CreatureEditViewModel(INavigationService navigationService, IDataService dataService)
    {
        _navigationService = navigationService;
        _dataService = dataService;
    }

    public void OnNavigatedFrom()
    {
    
    }
    public void OnNavigatedTo(object parameter)
    {
        if (parameter is Creature)
        {
            //set
            Creature = (Creature)parameter;
            if (Creature.Hyperlink == null)
                Creature.Hyperlink = "https://www.dndbeyond.com/"; //default safety net.
            Resists = new DamageTypesViewModel(Creature.Resistance);
            Immunities = new DamageTypesViewModel(Creature.Immunity);
            Vulnerabilities = new DamageTypesViewModel(Creature.Vulnerability);
        }
    }


}
