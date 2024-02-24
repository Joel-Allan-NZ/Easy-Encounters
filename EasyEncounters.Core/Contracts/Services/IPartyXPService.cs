using EasyEncounters.Core.Models;

namespace EasyEncounters.Core.Contracts.Services;

public interface IPartyXPService
{
    double[] CalculatePartyXPThresholds(Party party);
}