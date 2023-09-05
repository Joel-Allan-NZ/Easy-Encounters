using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.ViewModels
{
    public partial class DamageInstanceViewModel : ObservableRecipient
    {
        public ObservableCollection<DamageCreatureViewModel> Targets { get; private set; } = new();


        partial void OnSelectedDamageTypeChanged(DamageType value)
        {
            //add damage receive suggestion logic        
            foreach(var target in Targets)
            {
                target.SelectedDamageVolume = _activeEncounterService.GetDamageVolumeSuggestion(target.ActiveEncounterCreatureViewModel.Creature, SelectedDamageType);
            }
        }

        [ObservableProperty]
        private int _damageValue;

        [ObservableProperty]
        private DamageType _selectedDamageType;

        private IList<DamageType> _damageTypes = Enum.GetValues(typeof(DamageType)).Cast<DamageType>().ToList();

        public IList<DamageType> DamageTypes => _damageTypes;

        private readonly IActiveEncounterService _activeEncounterService;
        public DamageInstanceViewModel(IList<DamageCreatureViewModel> targets, IActiveEncounterService activeEncounterService)
        {
            _activeEncounterService = activeEncounterService;
            
            Targets.Clear();
            foreach(var target in targets)
            {
                if(target.ActiveEncounterCreatureViewModel != null)
                    Targets.Add(new DamageCreatureViewModel(target.ActiveEncounterCreatureViewModel));
            }
            SelectedDamageType = DamageType.None;
        }




    }
}
