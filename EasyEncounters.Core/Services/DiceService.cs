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
        if (diceString == null)
            return 0;
        return Parse(diceString);
    }

    private int HandleToken(DiceParseType parseType, string[] tokens, int currentIndex)
    {
        var result = 0;

        if (parseType == DiceParseType.Minus)
        {
            result -= int.Parse(tokens[currentIndex]);
        }
        else if (parseType == DiceParseType.Flat)
        {
            result = int.Parse(tokens[currentIndex]);
        }
        else //handle dice - tricksy 
        { 

        }
        return result;
    }

    private int HandleToken(DiceParseType parseType, string subString)
    {
        var res = 0;
        if (parseType == DiceParseType.Dice)
        {
            var split = subString.Split('d').Select(x => int.Parse(x)).ToList();
            if (split.Count == 1)
            {
                res = Roll(split[0]);
            }
            else
                res = Roll(split[1], split[0]);
        }
        else if (parseType == DiceParseType.Minus)
        {
            var val = int.Parse(subString);
            res = 0 - val;
        }
        else
        {
            var val = int.Parse(subString);
            res = val;
        }
        return res;
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
        var diceSplit = possibleDiceToken.SplitInclusive('d');//Regex.Split(possibleDiceToken, @"(?<=[d])"); //the result will be in form x,d,y or d,y or just y
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

    private DiceParseType GetParseType(string token)
    {
        return token[0] switch
        {
            '-' => DiceParseType.Minus,
            '+' => DiceParseType.Flat,
            _ => DiceParseType.Other //only possibly Dice
        };
    }
}