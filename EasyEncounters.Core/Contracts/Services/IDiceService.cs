using EasyEncounters.Core.Contracts.Enums;

namespace EasyEncounters.Core.Contracts.Services;

public interface IDiceService
{
    int Roll(int dieSize, int dieCount, DiceRollModifier modifier);

    int Roll(string diceString);
}