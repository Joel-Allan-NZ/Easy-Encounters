namespace EasyEncounters.Contracts.Services;

public interface IModelOptionsService
{
    bool RollHP
    {
        get;
    }

    Task<bool> ReadActiveEncounterOptionAsync();

    Task SaveActiveEncounterOptionAsync(bool optionValue);
}