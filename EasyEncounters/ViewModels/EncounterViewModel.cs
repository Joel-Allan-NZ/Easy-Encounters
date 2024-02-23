using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EasyEncounters.Core.Models;
using EasyEncounters.Messages;

namespace EasyEncounters.ViewModels;

public partial class EncounterViewModel : ObservableRecipient
{
    [ObservableProperty]
    private Encounter _encounter;

    public EncounterViewModel(Encounter encounter)
    {
        Encounter = encounter;
    }

    [RelayCommand]
    private void RequestCopyEncounter()
    {
        WeakReferenceMessenger.Default.Send(new EncounterCopyRequestMessage(this));
    }

    [RelayCommand]
    private void RequestDeleteEncounter()
    {
        WeakReferenceMessenger.Default.Send(new EncounterDeleteRequestMessage(this));
    }

    [RelayCommand]
    private void RequestEditEncounter()
    {
        WeakReferenceMessenger.Default.Send(new EncounterEditRequestMessage(this));
    }
}