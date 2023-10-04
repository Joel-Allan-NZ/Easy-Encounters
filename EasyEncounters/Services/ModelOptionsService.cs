using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Core.Contracts.Services;

namespace EasyEncounters.Services;
public class ModelOptionsService : IModelOptionsService
{
    private const string OptionsKey = "MaxHPRoll";
    private readonly ILocalSettingsService _localSettingService;
    private readonly IActiveEncounterService _activeEncounterService;

    public ModelOptionsService(ILocalSettingsService localSettingService, IActiveEncounterService activeEncounterService)
    {
        _activeEncounterService = activeEncounterService;
        _localSettingService = localSettingService;
    }

    public bool RollHP
    {
        get; private set;
    }

    public async Task<bool> ReadActiveEncounterOptionAsync()
    {
        _activeEncounterService.RollHP = await _localSettingService.ReadSettingAsync<bool>(OptionsKey);
        RollHP = _activeEncounterService.RollHP;
        return _activeEncounterService.RollHP;
    }

    public async Task SaveActiveEncounterOptionAsync(bool optionValue)
    {
        await _localSettingService.SaveSettingAsync<bool>(OptionsKey, optionValue);
        _activeEncounterService.RollHP = optionValue;
    }
}
