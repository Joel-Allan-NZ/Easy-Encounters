using EasyEncounters.Core.Models;
using EasyEncounters.Models;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

internal class AbilityCRUDRequestMessage : CRUDRequestMessage<Ability>
{
    public AbilityCRUDRequestMessage(Ability ability, CRUDRequestType requestType) : base(ability, requestType)
    {
    }
}