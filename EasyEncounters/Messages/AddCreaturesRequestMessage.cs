using EasyEncounters.Models;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

public class AddCreaturesRequestMessage
{
    public AddCreaturesRequestMessage(ICollection<ObservableKVP<ObservableCreature, int>> creaturesToAdd)
    {
        CreaturesToAdd = creaturesToAdd;
    }

    public ICollection<ObservableKVP<ObservableCreature, int>> CreaturesToAdd
    {
        get; set;
    }
}