using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Contracts.Services;

namespace EasyEncounters.Core.Services;
public class DiceService : IDiceService
{
    private readonly IRandomService _randomService;
    public DiceService(IRandomService randomService)
    {
        _randomService = randomService;
    }

    public int Roll(int dieSize, int dieCount = 1)
    {
        int result = 0;
        for(int i =0; i<dieCount; i++)
        {
            result += _randomService.RandomInteger(1, dieSize);
        }

        return result;
    }
    public int Roll(string diceString) //we're assuming the diceString doesn't have any invalid elements - validating elsewhere.
    {
        var result = 0;

        var splitOnPlus = diceString.ToLower().Split('+', (StringSplitOptions)3); //discard empty values and trim splits.
        foreach (var split in splitOnPlus)
        {
            var dieSplit = split.Split('d');
            if (dieSplit.Length == 1) //only a flat value
            {
                result += int.Parse(dieSplit[0]);
            }
            else
            {
                result += Roll(int.Parse(dieSplit[1]), int.Parse(dieSplit[0]));
            }
        }
        return result;
    }
}
