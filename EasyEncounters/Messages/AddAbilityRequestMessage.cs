using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

public class AddAbilityRequestMessage
{
    public AddAbilityRequestMessage(AbilityViewModel abilityViewModel)
    {
        AbilityViewModel = abilityViewModel;
    }

    public AbilityViewModel AbilityViewModel
    {
        get; private set;
    }
}