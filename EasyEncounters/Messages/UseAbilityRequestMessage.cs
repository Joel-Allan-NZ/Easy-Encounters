using EasyEncounters.Models;

namespace EasyEncounters.Messages;

internal class UseAbilityRequestMessage : EventRaisedMessage<ObservableActiveAbility>
{
    public UseAbilityRequestMessage(ObservableActiveAbility parameter) : base(parameter)
    {
    }
}