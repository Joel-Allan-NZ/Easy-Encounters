using EasyEncounters.Core.Models;

namespace EasyEncounters.Core.Contracts.Services;

public interface IPartyXPService
{
    int[] FindPartyXPThreshold(Party party);
}