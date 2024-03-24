using CommunityToolkit.Mvvm.ComponentModel;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Models;

public partial class ObservableAbility : ObservableObject
{
    public Ability Ability;

    [ObservableProperty]
    private bool _isSpell;

    [ObservableProperty]
    private bool _needsArea;

    [ObservableProperty]
    private bool _needsRangeValue;

    [ObservableProperty]
    private bool _needsSaveType;

    [ObservableProperty]
    private bool _needsTime;

    [ObservableProperty]
    private bool _needsCastTimeString;

    public ObservableAbility(Ability ability)
    {
        Ability = ability;

        IsSpell = ability.SpellLevel != SpellLevel.NotASpell;
        NeedsSaveType = ability.Resolution == ResolutionType.SavingThrow;
        NeedsRangeValue = ability.TargetDistanceType == ActionRangeType.Feet || ability.TargetDistanceType == ActionRangeType.Mile;
        NeedsArea = ability.TargetAreaType != TargetAreaType.Creatures && ability.TargetAreaType != TargetAreaType.None;
        NeedsTime = ability.TimeDurationType != Core.Models.Enums.TimeDuration.Permanent && ability.TimeDurationType != Core.Models.Enums.TimeDuration.Instantaneous;
        NeedsCastTimeString = ActionSpeed == ActionSpeed.Other;
    }

    public ActionSpeed ActionSpeed
    {
        get => Ability.ActionSpeed;
        set
        {
            SetProperty(Ability.ActionSpeed, value, Ability, (m, v) => m.ActionSpeed = v);
            NeedsCastTimeString = ActionSpeed == ActionSpeed.Other;
        }
    }

    public bool Concentration
    {
        get => Ability.Concentration;
        set => SetProperty(Ability.Concentration, value, Ability, (m, v) => m.Concentration = v);
    }

    public DamageType DamageType
    {
        get => Ability.DamageTypes;
        set => SetProperty(Ability.DamageTypes, value, Ability, (m, v) => m.DamageTypes = v);
    }

    public string EffectDescription
    {
        get => Ability.EffectDescription;
        set => SetProperty(Ability.EffectDescription, value, Ability, (m, v) => m.EffectDescription = v);
    }

    public MagicSchool MagicSchool
    {
        get => Ability.MagicSchool;
        set => SetProperty(Ability.MagicSchool, value, Ability, (m, v) => m.MagicSchool = v);
    }

    public string MaterialCost
    {
        get => Ability.MaterialCost;
        set => SetProperty(Ability.MaterialCost, value, Ability, (m, v) => m.MaterialCost = v);
    }

    public string Name
    {
        get => Ability.Name;
        set => SetProperty(Ability.Name, value, Ability, (m, v) => m.Name = v);
    }

    public ResolutionType Resolution
    {
        get => Ability.Resolution;
        set
        {
            SetProperty(Ability.Resolution, value, Ability, (m, v) => m.Resolution = v);
            NeedsSaveType = Ability.Resolution == ResolutionType.SavingThrow;
        }
    }

    public CreatureAttributeType ResolutionStat
    {
        get => Ability.ResolutionStat;
        set => SetProperty(Ability.ResolutionStat, value, Ability, (m, v) => m.ResolutionStat = v);
    }

    public CreatureAttributeType SaveType
    {
        get => Ability.SaveType;
        set => SetProperty(Ability.SaveType, value, Ability, (m, v) => m.SaveType = v);
    }

    public SpellCastComponent SpellCastComponents
    {
        get => Ability.CastingComponents;
        set => SetProperty(Ability.CastingComponents, value, Ability, (m, v) => m.CastingComponents = v);
    }

    public SpellLevel SpellLevel
    {
        get => Ability.SpellLevel;
        set
        {
            SetProperty(Ability.SpellLevel, value, Ability, (m, v) => m.SpellLevel = v);
            IsSpell = Ability.SpellLevel != SpellLevel.NotASpell;
        }
    }

    public TargetAreaType TargetAreaType
    {
        get => Ability.TargetAreaType;
        set
        {
            SetProperty(Ability.TargetAreaType, value, Ability, (m, v) => m.TargetAreaType = v);
            NeedsArea = Ability.TargetAreaType != TargetAreaType.Creatures && Ability.TargetAreaType != TargetAreaType.None;
        }
    }

    public int TargetCount
    {
        get => Ability.TargetCount;
        set => SetProperty(Ability.TargetCount, value, Ability, (m, v) => m.TargetCount = v);
    }

    public int TargetDistance
    {
        get => Ability.TargetDistance;
        set => SetProperty(Ability.TargetDistance, value, Ability, (m, v) => m.TargetDistance = v);
    }

    public ActionRangeType TargetDistanceType
    {
        get => Ability.TargetDistanceType;
        set
        {
            SetProperty(Ability.TargetDistanceType, value, Ability, (m, v) => m.TargetDistanceType = v);
            NeedsRangeValue = Ability.TargetDistanceType == ActionRangeType.Feet || Ability.TargetDistanceType == ActionRangeType.Mile;
        }
    }

    public int TargetSize
    {
        get => Ability.TargetAreaSize;
        set => SetProperty(Ability.TargetAreaSize, value, Ability, (m, v) => m.TargetAreaSize = v);
    }

    public int TimeDuration
    {
        get => Ability.TimeDuration;
        set => SetProperty(Ability.TimeDuration, value, Ability, (m, v) => m.TimeDuration = v);
    }

    public TimeDuration TimeDurationType
    {
        get => Ability.TimeDurationType;
        set
        {
            SetProperty(Ability.TimeDurationType, value, Ability, (m, v) => m.TimeDurationType = v);
            NeedsTime = Ability.TimeDurationType != Core.Models.Enums.TimeDuration.Permanent &&
                Ability.TimeDurationType != Core.Models.Enums.TimeDuration.Instantaneous;
        }
    }

}