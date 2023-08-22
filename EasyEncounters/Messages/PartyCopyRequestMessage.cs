using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;
internal class PartyCopyRequestMessage : EventRaisedMessage<PartyViewModel>
{
    public PartyCopyRequestMessage(PartyViewModel parameter) : base(parameter)
    {
    }
}
