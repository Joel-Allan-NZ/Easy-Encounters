using EasyEncounters.Core.Models;

namespace EasyEncounters.Messages;

public class AbilityChangeCommitMessage
{
    public AbilityChangeCommitMessage(Ability ability)
    {
        Ability = ability;
    }

    public Ability Ability
    {
        get; private set;
    }
}