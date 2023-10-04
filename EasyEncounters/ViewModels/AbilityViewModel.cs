using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    [RelayCommand]
    private void AddAbilityRequested()
    {
        WeakReferenceMessenger.Default.Send(new AddAbilityRequestMessage(this));
    }


    public AbilityViewModel(Ability ability)
    {
        _ability = ability;
    }


}
