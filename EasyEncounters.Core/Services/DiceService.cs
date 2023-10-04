using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Contracts.Services;

namespace EasyEncounters.Core.Services;
internal enum DiceParseType
{
    Minus,
    Flat,
    Dice
}
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
        //var result = 0;


        //var splitOnPlus = diceString.ToLower().Split('+', (StringSplitOptions)3); //discard empty values and trim splits.
        //foreach (var split in splitOnPlus)
        //{
        //    var dieSplit = split.Split('d');
        //    if (dieSplit.Length == 1) //only a flat value
        //    {
        //        result += int.Parse(dieSplit[0]);
        //    }
        //    else
        //    {
        //        result += Roll(int.Parse(dieSplit[1]), int.Parse(dieSplit[0]));
        //    }
        //}
        //return result;
        if (diceString == null)
            return 0;
        return Parse(diceString);
    }

    private int Parse(string diceString)
    {
        int result = 0;

        DiceParseType parseType = DiceParseType.Dice;
        var lowerString = diceString.ToLower();

        Dictionary<int, int> dice = new();
        int substringStart = 0, substringCurrent = 0;

        

        if (lowerString[0] == '-')
        {
            parseType = DiceParseType.Minus;
        }
        else if (lowerString[0] == 'd')
        {
            parseType = DiceParseType.Dice;
        }
        else if (lowerString[0] == '+')
        {
            parseType = DiceParseType.Flat;
        }

       
        while(substringCurrent < lowerString.Length)
        {
            if (Char.IsDigit(lowerString[substringCurrent]))
            {
                substringCurrent++;
            }
            else if (lowerString[substringCurrent] ==  '-')
            {
                result += HandleToken(parseType, diceString.Substring(substringStart, substringCurrent - substringStart));
                parseType = DiceParseType.Minus;
                
                substringCurrent++;
                substringStart = substringCurrent;
            }
            else if (lowerString[substringCurrent] == 'd')
            {
                //ParseLast(parseType, diceString.Substring(substringStart, substringStart + 1 - substringCurrent));
                parseType = DiceParseType.Dice;
                substringCurrent++;
            }
            else if (lowerString[substringCurrent] == '+')
            {
                result += HandleToken(parseType, diceString.Substring(substringStart, substringCurrent - substringStart));
                parseType = DiceParseType.Flat;

                substringCurrent++;
                substringStart = substringCurrent;
            }
        }
        result += HandleToken(parseType, lowerString.Substring(substringStart));
        return result;
    }

    private int HandleToken(DiceParseType parseType, string subString)
    {
        int res = 0;
        if(parseType == DiceParseType.Dice)
        {
            var split = subString.Split('d').Select(x => int.Parse(x)).ToList();
            if (split.Count == 1)
            {
                res = Roll(split[0]);
            }
            else
                res = Roll(split[1], split[0]);
        }
        else if(parseType == DiceParseType.Minus)
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
}
