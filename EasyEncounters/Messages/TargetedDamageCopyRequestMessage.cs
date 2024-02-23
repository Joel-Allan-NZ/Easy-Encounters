using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

internal class TargetedDamageCopyRequestMessage : EventRaisedMessage<TargetDamageInstanceViewModel>
{
    public TargetedDamageCopyRequestMessage(TargetDamageInstanceViewModel parameter) : base(parameter)
    {
    }
}