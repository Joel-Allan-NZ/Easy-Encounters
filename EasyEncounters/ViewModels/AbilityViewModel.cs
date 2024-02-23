using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EasyEncounters.Core.Models;
using EasyEncounters.Messages;

namespace EasyEncounters.ViewModels;

public partial class AbilityViewModel : ObservableRecipient
{
    [ObservableProperty]
    private Ability _ability;

    public AbilityViewModel(Ability ability)
    {
        _ability = ability;
    }

    [RelayCommand]
    private void AddAbilityRequested()
    {
        WeakReferenceMessenger.Default.Send(new AddAbilityRequestMessage(this));
    }

    [RelayCommand]
    private void CopyAbilityRequested()
    {
        WeakReferenceMessenger.Default.Send(new AbilityCRUDRequestMessage(this, Models.CRUDRequestType.Copy));
    }

    [RelayCommand]
    private void DeleteAbilityRequested()
    {
        WeakReferenceMessenger.Default.Send(new AbilityCRUDRequestMessage(this, Models.CRUDRequestType.Delete));
    }

    [RelayCommand]
    private void EditAbilityRequested()
    {
        WeakReferenceMessenger.Default.Send(new AbilityCRUDRequestMessage(this, Models.CRUDRequestType.Edit));
    }
}