using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;

namespace EasyEncounters.ViewModels
{
    public partial class DealDamageViewModel : ObservableRecipient, INavigationAware
    {
        [ObservableProperty]
        private ActiveEncounterCreatureViewModel? _sourceCreature;

        //public ObservableCollection<ActiveEncounterCreatureViewModel> Targets = new();
        private IList<DamageCreatureViewModel> _targets;

        public ObservableCollection<DamageInstanceViewModel> DamageInstances
        {
            get; private set;
        } = new();


        private readonly INavigationService _navigationService;
        private readonly IActiveEncounterService _activeEncounterService;
        private ActiveEncounter? _activeEncounter;

        public DealDamageViewModel(IActiveEncounterService activeEncounterService, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _activeEncounterService = activeEncounterService;
            _targets = new List<DamageCreatureViewModel>();

        }

        public void OnNavigatedFrom()
        {
        }
        public void OnNavigatedTo(object parameter)
        {
            if (parameter is DealDamageTargetting && parameter != null)
            {
                //set properties
                var targetting = (DealDamageTargetting)parameter;

                SourceCreature = targetting.Source;

                _activeEncounter = targetting.Encounter;
                _targets = targetting.Targets.Select(x => new DamageCreatureViewModel(x)).ToList();

                AddDamage();

            }
        }

        [RelayCommand]
        private void AddDamage()
        {
            DamageInstances.Add(new DamageInstanceViewModel(_targets, _activeEncounterService));
        }

        [RelayCommand]
        private void DealDamage()
        {
            var instances = GetInstancesOfDamage();
            foreach(var instance in instances)
                _activeEncounterService.DealDamage(_activeEncounter, instance);

            _navigationService.NavigateTo(typeof(RunEncounterViewModel).FullName!);
        }

        private List<DamageInstance> GetInstancesOfDamage()
        {
            List<DamageInstance> damageInstances = new List<DamageInstance>();

            foreach(var damage in DamageInstances)
            {
                foreach(var target in damage.Targets)
                {
                    damageInstances.Add(new DamageInstance(target.ActiveEncounterCreatureViewModel.Creature, SourceCreature.Creature, damage.SelectedDamageType, target.SelectedDamageVolume, damage.DamageValue));
                }
            }
            return damageInstances;
        }
    }
}
