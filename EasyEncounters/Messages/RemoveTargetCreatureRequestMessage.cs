using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

internal class RemoveTargetCreatureRequestMessage : EventRaisedMessage<DamageCreatureViewModel>
{
    public RemoveTargetCreatureRequestMessage(DamageCreatureViewModel parameter) : base(parameter)
    {
    }
}