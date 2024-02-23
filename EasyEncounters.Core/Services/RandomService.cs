using EasyEncounters.Core.Contracts.Services;

namespace EasyEncounters.Core.Services;

public class RandomService : IRandomService
{
    private readonly Random _random;

    public RandomService()
    {
        _random = new Random();
    }

    public int RandomInteger(int minValue, int maxValue)
    {
        return _random.Next(minValue, maxValue + 1);
    }
}