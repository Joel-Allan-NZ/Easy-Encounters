using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;

namespace EasyEncounters.Core.Services;

public class PartyXPService : IPartyXPService
{
    /// <summary>
    /// Calculates the party's XP thresholds for various encounter difficulties, based on party size and level,
    /// and returns the range of thresholds in order of difficulty, including an unofficial "Very Difficult"
    /// </summary>
    /// <returns>Array of int in order of encounter difficulty</returns>
    public double[] CalculatePartyXPThresholds(Party party)
    {
        var result = new double[6] { 0, 0, 0, 0, 0, double.MaxValue };
        foreach (Creature c in party.Members)
        {
            var add = AddSinglePlayerXPThreshold(c.LevelOrCR);
            for (var i = 0; i < 4; i++)
                result[i] += add[i];
        }
        result[4] = result[3] * 1.5;
        return result;
    }

    /// <summary>
    /// find the xp Thresholds for a single player character.
    /// </summary>
    /// <param name="playerLevel">The level of the player to find thresholds for</param>
    /// <returns></returns>
    private static int[] AddSinglePlayerXPThreshold(double playerLevel)
    {
        var thresholds = playerLevel switch
        {
            1 => new int[4] { 25, 50, 75, 100 },
            2 => new int[4] { 50, 75, 100, 150 },
            3 => new int[4] { 75, 150, 225, 400 },
            4 => new int[4] { 125, 250, 375, 500 },
            5 => new int[4] { 250, 500, 750, 1100 },
            6 => new int[4] { 300, 600, 900, 1400 },
            7 => new int[4] { 350, 750, 1100, 1700 },
            8 => new int[4] { 450, 900, 1400, 2100 },
            9 => new int[4] { 550, 1100, 1600, 2400 },
            10 => new int[4] { 600, 1200, 1900, 2800 },
            11 => new int[4] { 800, 1600, 2400, 3600 },
            12 => new int[4] { 1000, 2000, 3000, 4500 },
            13 => new int[4] { 1100, 2200, 3400, 5100 },
            14 => new int[4] { 1250, 2500, 3800, 5700 },
            15 => new int[4] { 1400, 2800, 4300, 6400 },
            16 => new int[4] { 1600, 3200, 4800, 7200 },
            17 => new int[4] { 2000, 3900, 5900, 8800 },
            18 => new int[4] { 2100, 4200, 6300, 9500 },
            19 => new int[4] { 2400, 4900, 7300, 10900 },
            20 => new int[4] { 2800, 5700, 8500, 12700 },
            _ => new int[4] { 0, 0, 0, 0 },
        };
        return thresholds;
    }
}