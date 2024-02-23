namespace EasyEncounters.Core.Contracts.Services;

public interface IRandomService
{
    /// <summary>
    /// Get a random integer in the defined range, inclusive.
    /// </summary>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <returns></returns>
    int RandomInteger(int minValue, int maxValue);
}