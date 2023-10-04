using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Messages;
internal class DamageTypeChangedMessage : EventRaisedMessage<DamageType>
{
    public DamageTypeChangedMessage(DamageType parameter) : base(parameter)
    {
    }
}
