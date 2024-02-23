using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Messages;

internal class DamageTypeChangedMessage : EventRaisedMessage<DamageType>
{
    public DamageTypeChangedMessage(DamageType parameter) : base(parameter)
    {
    }
}