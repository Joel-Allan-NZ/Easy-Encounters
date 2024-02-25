using System.Security;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using EasyEncounters.Core.Contracts.Enums;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Helpers;

namespace EasyEncounters.Core.Services;

internal enum DiceParseType
{
    Minus,
    Flat,
    Dice,
    Other
}

public class DiceService : IDiceService
{
    private readonly IRandomService _randomService;

    public DiceService(IRandomService randomService)
    {
        _randomService = randomService;
    }

    public int Roll(int dieSize, int dieCount = 1, DiceRollModifier modifier = DiceRollModifier.None)
    {
        var result = 0;
        for (var i = 0; i < dieCount; i++)
        {
            result += _randomService.RandomInteger(1, dieSize);
        }

        if(modifier != DiceRollModifier.None)
        {
            var secondResult = Roll(dieSize, dieCount, DiceRollModifier.None);

            result = modifier == DiceRollModifier.Disadvantage ? Math.Min(result, secondResult) : Math.Max(result, secondResult);
        }

        return result;
    }

    public int Roll(string diceString) //we're assuming the diceString doesn't have any invalid elements - validating elsewhere.
    {
        return diceString == null ? 0 : Parse(diceString);
    }

    private int Parse(string diceString)
    {
        var result = 0;

        var lowerString = diceString.ToLower();
        var tokens = lowerString.SplitInclusive(new char[] { '+', '-' });


        var lastParseType = GetParseType(tokens[0]);
        if (lastParseType == DiceParseType.Other)
        {
            result += ResolveTokenValue(tokens[0]);
        }

        for(var i = 1; i<tokens.Length; i++)
        {
            var parseType = GetParseType(tokens[i]);

            if(parseType == DiceParseType.Other)
            {
                var tokenValue = ResolveTokenValue(tokens[i]);

                var positiveOrNegative = lastParseType == DiceParseType.Flat ? 1 : -1;

                result += (tokenValue * positiveOrNegative);
            }
            lastParseType = parseType;
        }

        return result;

    }

    private int ResolveTokenValue(string possibleDiceToken)
    {
        var diceSplit = possibleDiceToken.SplitInclusive('d');
        int result;

        if (diceSplit.Length == 3)
        {
            result = Roll(int.Parse(diceSplit[2]), int.Parse(diceSplit[0]));
        }
        else if (diceSplit.Length == 2)
        {
            result = Roll(int.Parse(diceSplit[1]));
        }
        else
        {
            result = int.Parse(possibleDiceToken);
        }

        return result;

    }

    private static DiceParseType GetParseType(string token)
    {
        return token[0] switch
        {
            '-' => DiceParseType.Minus,
            '+' => DiceParseType.Flat,
            _ => DiceParseType.Other //only possibly Dice
        };
    }
}