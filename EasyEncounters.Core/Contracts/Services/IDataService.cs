using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Models;

namespace EasyEncounters.Core.Contracts.Services;
public interface IDataService
{
    Task<IEnumerable<Campaign>> GetAllCampaignsAsync();

    Task<IEnumerable<Party>> GetAllPartiesAsync();

    Task<IEnumerable<Creature>> GetAllCreaturesAsync();

    Task<IEnumerable<Encounter>> GetAllEncountersAsync();

    Task<bool> ActiveEncounterExistsAsync();

    Task<ActiveEncounter> GetActiveEncounterAsync();

    Task WriteLog(IEnumerable<string> log);

    Task<IEnumerable<Party>> GetCampaignPartiesAsync(Campaign campaign);

    Task<IEnumerable<EncounterData>> GetAllEncounterDataAsync(Party party);

    Task SaveAddAsync<T>(T entity) where T : IPersistable;

    Task ClearActiveEncounterAsync();

    Task DeleteAsync<T>(T entity) where T: IPersistable;

    Task<T> CopyAsync<T>(T entity) where T : IPersistable;
}
