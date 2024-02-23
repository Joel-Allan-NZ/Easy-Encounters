using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

internal class TargetedDamageDeleteRequestMessage : EventRaisedMessage<TargetDamageInstanceViewModel>
{
    public TargetedDamageDeleteRequestMessage(TargetDamageInstanceViewModel parameter) : base(parameter)
    {
    }
}