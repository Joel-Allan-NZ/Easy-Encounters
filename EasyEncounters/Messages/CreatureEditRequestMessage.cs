using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;
public class CreatureEditRequestMessage : EventRaisedMessage<CreatureViewModel>
{
    public CreatureEditRequestMessage(CreatureViewModel parameter) : base(parameter)
    {
    }
}
