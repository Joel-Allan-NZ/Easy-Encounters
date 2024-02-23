using EasyEncounters.Models;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;

public class AddCreaturesRequestMessage
{
    public AddCreaturesRequestMessage(ICollection<ObservableKVP<CreatureViewModel, int>> creaturesToAdd)
    {
        CreaturesToAdd = creaturesToAdd;
    }

    public ICollection<ObservableKVP<CreatureViewModel, int>> CreaturesToAdd
    {
        get; set;
    }
}