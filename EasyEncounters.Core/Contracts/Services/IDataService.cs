using EasyEncounters.Core.Models;

namespace EasyEncounters.Core.Contracts.Services;

public interface IDataService
{
    Task<bool> ActiveEncounterExistsAsync();

    Task ClearActiveEncounterAsync();

    Task<T> CopyAsync<T>(T entity) where T : IPersistable;

    Task DeleteAsync<T>(T entity) where T : IPersistable;

    Task<ActiveEncounter> GetActiveEncounterAsync();

    Task<IEnumerable<Campaign>> GetAllCampaignsAsync();

    Task<IEnumerable<Creature>> GetAllCreaturesAsync();

    Task<IEnumerable<Encounter>> GetAllEncountersAsync();

    Task<IEnumerable<Party>> GetAllPartiesAsync();

    Task<IEnumerable<Ability>> GetAllSpellsAsync();

    Task<IEnumerable<Party>> GetCampaignPartiesAsync(Campaign campaign);

    Task SaveAddAsync<T>(T entity) where T : IPersistable;

    Task WriteLog(IEnumerable<string> log);
}