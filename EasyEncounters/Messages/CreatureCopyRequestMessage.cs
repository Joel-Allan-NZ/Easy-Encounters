using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;
internal class CreatureCopyRequestMessage : EventRaisedMessage<CreatureViewModel>
{
    public CreatureCopyRequestMessage(CreatureViewModel parameter) : base(parameter)
    {
    }
}
