using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEncounters.Core.Contracts.Services;
public interface IDiceService
{
    int Roll(int dieSize, int dieCount);

    int Roll(string diceString);
}
