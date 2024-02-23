using EasyEncounters.Models;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

internal class AbilityCRUDRequestMessage : CRUDRequestMessage<AbilityViewModel>
{
    public AbilityCRUDRequestMessage(AbilityViewModel abilityViewModel, CRUDRequestType requestType) : base(abilityViewModel, requestType)
    {
    }
}