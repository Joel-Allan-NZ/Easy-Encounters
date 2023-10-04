using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Messages;

namespace EasyEncounters.ViewModels
{
    /// <summary>
    /// ViewModel for accepting or dealing damage. Simplier to just use the single VM with excess properties than create
    /// a viewmodel for each.
    /// </summary>
    public partial class DamageCreatureViewModel : ObservableRecipient
    {
        [ObservableProperty]
        private IList<DamageVolume> _damageVolumes = Enum.GetValues(typeof(DamageVolume)).Cast<DamageVolume>().ToList();

        [ObservableProperty]
        private DamageVolume _selectedDamageVolume;

        [ObservableProperty]
        private ActiveEncounterCreatureViewModel _activeEncounterCreatureViewModel;


        public DamageCreatureViewModel(ActiveEncounterCreatureViewModel creatureVM)
        {
            SelectedDamageVolume = DamageVolume.Normal;
            ActiveEncounterCreatureViewModel = creatureVM;

        }

        [RelayCommand]
        private void RemoveTargetRequested()
        {
            WeakReferenceMessenger.Default.Send(new RemoveTargetCreatureRequestMessage(this));
        }
    }
}
