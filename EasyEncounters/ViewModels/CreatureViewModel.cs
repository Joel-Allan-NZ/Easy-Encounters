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
}