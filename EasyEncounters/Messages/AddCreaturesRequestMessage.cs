using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Models;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;
public class AddCreaturesRequestMessage
{
    public ICollection<ObservableKVP<CreatureViewModel, int>> CreaturesToAdd
    {
    get; set; }

    public AddCreaturesRequestMessage(ICollection<ObservableKVP<CreatureViewModel, int>> creaturesToAdd)
    {
        CreaturesToAdd = creaturesToAdd;   
    }
}
