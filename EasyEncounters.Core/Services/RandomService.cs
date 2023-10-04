using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Contracts.Services;

namespace EasyEncounters.Core.Services;
public class RandomService : IRandomService
{
    private readonly Random _random;
    public int RandomInteger(int minValue, int maxValue)
    {
        return _random.Next(minValue, maxValue+1);
    }

    public RandomService()
    {
        _random = new Random();
    }
}
