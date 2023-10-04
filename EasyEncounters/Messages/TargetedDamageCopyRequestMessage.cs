using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;
internal class TargetedDamageCopyRequestMessage : EventRaisedMessage<TargetDamageInstanceViewModel>
{
    public TargetedDamageCopyRequestMessage(TargetDamageInstanceViewModel parameter) : base(parameter)
    {
    }
}
