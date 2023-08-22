using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;
internal class EncounterCopyRequestMessage : EventRaisedMessage<EncounterViewModel>
{
    public EncounterCopyRequestMessage(EncounterViewModel parameter) : base(parameter)
    {
    }
}
