using EasyEncounters.Core.Models;
using EasyEncounters.Core.Services;

namespace ServiceTests;

public class AbilityService_Tests : IDisposable
{
    AbilityService? _abilityService;
    int attackBaseValue;
    int saveBaseValue;

    public AbilityService_Tests()
    {
        _abilityService = new AbilityService();
        attackBaseValue = 10;
        saveBaseValue = 8;
    }
    public void Dispose()
    {
        _abilityService = null;
        attackBaseValue = 0;
        saveBaseValue = 0;

    }

    [Fact]
    public void CopyTo_HasEqualParams()
    {
        Ability targetAbility = new()
        {
            ActionSpeed = EasyEncounters.Core.Models.Enums.ActionSpeed.BonusAction,
            TargetAreaType = EasyEncounters.Core.Models.Enums.TargetAreaType.Creatures,
            TargetCount = 1,
            CastingComponents = EasyEncounters.Core.Models.Enums.SpellCastComponent.Verbal,
            Concentration = false,
            DamageTypes = EasyEncounters.Core.Models.Enums.DamageType.Force,
            EffectDescription = "words",
            MagicSchool = EasyEncounters.Core.Models.Enums.MagicSchool.Evocation,
            MaterialCost = "cost string",
            Name = "ability name",
            Resolution = EasyEncounters.Core.Models.Enums.ResolutionType.Attack,
            ResolutionStat = EasyEncounters.Core.Models.Enums.CreatureAttributeType.Constitution,
            SaveType = EasyEncounters.Core.Models.Enums.CreatureAttributeType.None,
            SpellLevel = EasyEncounters.Core.Models.Enums.SpellLevel.LevelFive,
            TargetDistance = 30,
            TargetAreaSize = 0,
            TimeDuration = 0,
            TimeDurationType = EasyEncounters.Core.Models.Enums.TimeDuration.Instantaneous,
            TargetDistanceType = EasyEncounters.Core.Models.Enums.ActionRangeType.Feet
        };

        Ability copied = new();

        _abilityService.CopyTo(copied, targetAbility);

        Assert.Multiple(() =>
        {
            Assert.Equal(copied.ActionSpeed, targetAbility.ActionSpeed);
            Assert.Equal(copied.TargetAreaType, targetAbility.TargetAreaType);
            Assert.Equal(copied.TargetCount, targetAbility.TargetCount);
            Assert.Equal(copied.CastingComponents, targetAbility.CastingComponents);
            Assert.Equal(copied.Concentration, targetAbility.Concentration);
            Assert.Equal(copied.DamageTypes, targetAbility.DamageTypes);
            Assert.Equal(copied.EffectDescription, targetAbility.EffectDescription);
            Assert.Equal(copied.MagicSchool, targetAbility.MagicSchool);
            Assert.Equal(copied.MaterialCost, targetAbility.MaterialCost);
            Assert.Equal(copied.Name, targetAbility.Name);
            Assert.Equal(copied.Resolution, targetAbility.Resolution);
            Assert.Equal(copied.ResolutionStat, targetAbility.ResolutionStat);
            Assert.Equal(copied.SaveType, targetAbility.SaveType);
            Assert.Equal(copied.SpellLevel, targetAbility.SpellLevel);
            Assert.Equal(copied.TargetDistance, targetAbility.TargetDistance);
            Assert.Equal(copied.TargetAreaSize, targetAbility.TargetAreaSize);
            Assert.Equal(copied.TimeDuration, targetAbility.TimeDuration);
            Assert.Equal(copied.TimeDurationType, targetAbility.TimeDurationType);
            Assert.Equal(copied.TargetDistanceType, targetAbility.TargetDistanceType);
        });

    }

    [Fact]
    public void CreateActiveAbility_IsNotNull()
    {
        ActiveEncounterCreature creature = new()
        {
            ProficiencyBonus = 2,
        };

        Ability ability = new()
        {
            Resolution = EasyEncounters.Core.Models.Enums.ResolutionType.Attack
        };

        var activeAbility = _abilityService.CreateActiveAbility(creature, ability, 1);


        Assert.NotNull(activeAbility);
    }

    [Theory]
    [InlineData(2,2)]
    [InlineData(0,0)]
    [InlineData(1,0)]
    [InlineData(0,1)]
    [InlineData(100,500)]
    [InlineData(0,-5)]
    [InlineData(2, -5)]
    [InlineData(-2,3)]
    [InlineData(-2,-8)]
    public void CreateActiveAbility_CanResolveAttack(int prof, int abilityBonus)
    {
        ActiveEncounterCreature creature = new()
        {
            ProficiencyBonus = prof,
        };

        Ability ability = new()
        {
            Resolution = EasyEncounters.Core.Models.Enums.ResolutionType.Attack
        };

        var activeAbility = _abilityService.CreateActiveAbility(creature, ability, abilityBonus);

        Assert.Equal(activeAbility.ResolutionValue, prof + abilityBonus + attackBaseValue);
    }

    [Theory]
    [InlineData(2, 2)]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    [InlineData(0, 1)]
    [InlineData(100, 500)]
    [InlineData(0, -5)]
    [InlineData(2, -5)]
    [InlineData(-2, 3)]
    [InlineData(-2, -8)]
    public void CreateActiveAbility_CanResolveSavingThrow(int prof, int abilityBonus)
    {
        ActiveEncounterCreature creature = new()
        {
            ProficiencyBonus = prof,
        };

        Ability ability = new()
        {
            Resolution = EasyEncounters.Core.Models.Enums.ResolutionType.SavingThrow
        };

        var activeAbility = _abilityService.CreateActiveAbility(creature, ability, abilityBonus);

        Assert.Equal(activeAbility.ResolutionValue, prof + abilityBonus + saveBaseValue);
    }


}