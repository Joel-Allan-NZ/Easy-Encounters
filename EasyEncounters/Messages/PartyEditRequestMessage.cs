using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;
internal class PartyEditRequestMessage : EventRaisedMessage<PartyViewModel>
{
    public PartyEditRequestMessage(PartyViewModel parameter) : base(parameter)
    {
    }
}
