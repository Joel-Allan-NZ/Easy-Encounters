using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;
internal class PartyDeleteRequestMessage : EventRaisedMessage<PartyViewModel>
{
    public PartyDeleteRequestMessage(PartyViewModel parameter) : base(parameter)
    {
    }
}
