using EasyEncounters.Core.Models;

namespace EasyEncounters.Core.Contracts.Services;

public interface IDataService
{
    //Task<bool> ActiveEncounterExistsAsync();

    //Task ClearActiveEncounterAsync();

    Task<T> CopyAsync<T>(T entity) where T : IPersistable;

    Task DeleteAsync<T>(T entity) where T : IPersistable;

    //Task<ActiveEncounter> GetActiveEncounterAsync();

    Task<IEnumerable<Campaign>> GetAllCampaignsAsync();

    Task<IEnumerable<Creature>> GetAllCreaturesAsync();

    Task<IEnumerable<Encounter>> GetAllEncountersAsync();

    Task<IEnumerable<Party>> GetAllPartiesAsync();

    Task<IEnumerable<Ability>> GetAllSpellsAsync();

    Task<IEnumerable<Party>> GetCampaignPartiesAsync(Campaign campaign);

    Task SaveAddAsync<T>(T entity) where T : class, IPersistable;

    Task WriteLogAsync(IEnumerable<string> log);

    Task<IEnumerable<Encounter>> GetCampaignEncountersAsync(Campaign campaign, bool includeGeneralEncounters);

    Task SaveAddAsync<T>(IEnumerable<T> entities) where T : IPersistable;

    IQueryable<Campaign> Campaigns();
    IQueryable<Encounter> Encounters();
    IQueryable<Ability> Abilities();
    IQueryable<Party> Parties();
    IQueryable<Creature> Creatures();
    IQueryable<ActiveEncounter> ActiveEncounters();

    Task CommitChanges();


}