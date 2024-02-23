namespace EasyEncounters.Core.Contracts.Services;

public interface IDiceService
{
    int Roll(int dieSize, int dieCount);

    int Roll(string diceString);
}