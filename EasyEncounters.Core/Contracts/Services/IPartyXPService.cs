using EasyEncounters.Core.Models;

namespace EasyEncounters.Core.Contracts.Services;

public interface IPartyXPService
{
    void CalculatePartyXPThresholds(Party party);
}