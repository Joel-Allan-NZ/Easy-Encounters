using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEncounters.Core.Contracts;
public interface IPersistable
{
    Guid Id
    {
        get; set;
    }
}
