using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Models;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;
internal class UseAbilityRequestMessage : EventRaisedMessage<ObservableActiveAbility>
{
    public UseAbilityRequestMessage(ObservableActiveAbility parameter) : base(parameter)
    {
    }
}
