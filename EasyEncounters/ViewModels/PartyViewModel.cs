using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EasyEncounters.Core.Models;
using EasyEncounters.Messages;

namespace EasyEncounters.ViewModels;

public partial class PartyViewModel : ObservableRecipient
{
    [ObservableProperty]
    private Party _party;

    public PartyViewModel(Party party)
    {
        Party = party;
    }

    [RelayCommand]
    private void RequestCopyParty()
    {
        WeakReferenceMessenger.Default.Send(new PartyCopyRequestMessage(this));
    }

    [RelayCommand]
    private void RequestDeleteParty()
    {
        WeakReferenceMessenger.Default.Send(new PartyDeleteRequestMessage(this));
    }

    [RelayCommand]
    private void RequestEditParty()
    {
        WeakReferenceMessenger.Default.Send(new PartyEditRequestMessage(this));
    }
}