using CommunityToolkit.Mvvm.Messaging.Messages;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

public class DamageSourceSelectedMessage : ValueChangedMessage<ObservableActiveEncounterCreature>
{
    public DamageSourceSelectedMessage(ObservableActiveEncounterCreature value) : base(value)
    {
    }
}