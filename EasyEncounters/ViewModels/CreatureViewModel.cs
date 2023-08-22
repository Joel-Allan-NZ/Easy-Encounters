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
public partial class CreatureViewModel : ObservableRecipient
{
    [ObservableProperty]
    private Creature creature;

    public CreatureViewModel(Creature creature)
    {
        Creature = creature;
    }

    [RelayCommand]
    private void RequestDeleteCreature()
    {
        WeakReferenceMessenger.Default.Send(new CreatureDeleteRequestMessage(this));
    }

    [RelayCommand]
    private void RequestCopyCreature()
    {
        WeakReferenceMessenger.Default.Send(new CreatureCopyRequestMessage(this));
    }

    [RelayCommand]
    private void RequestEditCreature()
    {
        WeakReferenceMessenger.Default.Send(new CreatureEditRequestMessage(this));
    }



}
