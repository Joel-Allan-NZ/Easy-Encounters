using CommunityToolkit.Mvvm.Messaging.Messages;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

public class DamageSourceSelectedMessage : ValueChangedMessage<ActiveEncounterCreatureViewModel>
{
    public DamageSourceSelectedMessage(ActiveEncounterCreatureViewModel value) : base(value)
    {
    }
}