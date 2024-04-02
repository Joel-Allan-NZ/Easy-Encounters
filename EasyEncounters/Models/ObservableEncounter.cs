using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Models;
public partial class ObservableEncounter : ObservableObject
{
    [ObservableProperty]
    private Encounter _encounter;

    [ObservableProperty]
    private EncounterDifficulty _encounterDifficulty;

    public bool IsCampaignOnlyEncounter
    {
        get => Encounter.IsCampaignOnlyEncounter;
        set => SetProperty(Encounter.IsCampaignOnlyEncounter, value, Encounter, (m,v) => m.IsCampaignOnlyEncounter = v);
    }

    public string Name
    {
        get => Encounter.Name;
        set => SetProperty(Encounter.Name, value, Encounter, (m,v) => m.Name = v);
    }

    public Campaign? Campaign
    {
        get => Encounter.Campaign;
        set => SetProperty(Encounter.Campaign, value, Encounter, (m,v) => m.Campaign = v);
    }

    public int CreatureCount
    {
        get => Encounter.CreatureCount;
        set => SetProperty(Encounter.CreatureCount, value, Encounter, (m, v) => m.CreatureCount = v);
    }


    public ObservableEncounter(Encounter encounter)
    {
        _encounterDifficulty = EncounterDifficulty.None;
        _encounter = encounter;
    }

}
