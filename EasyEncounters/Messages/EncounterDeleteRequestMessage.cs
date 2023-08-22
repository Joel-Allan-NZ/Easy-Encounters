using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;
internal class EncounterDeleteRequestMessage : EventRaisedMessage<EncounterViewModel>
{
    public EncounterDeleteRequestMessage(EncounterViewModel parameter) : base(parameter)
    {
    }
}
