﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.UI.Controls;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Helpers;
using EasyEncounters.Helpers.Conditions;
using EasyEncounters.Helpers.DamageTypes;
using EasyEncounters.Messages;
using EasyEncounters.Models;
using EasyEncounters.Services.Filter;
using Microsoft.UI.Xaml.Controls;

namespace EasyEncounters.ViewModels;
public partial class CreatureEditNavigationPageViewModel : ObservableRecipient, INavigationAware
{
    private readonly IList<CreatureAttributeType> _creatureAttributeTypes = Enum.GetValues(typeof(CreatureAttributeType)).Cast<CreatureAttributeType>().ToList();
    private readonly IList<CreatureSizeClass> _creatureSizeClasses = Enum.GetValues(typeof(CreatureSizeClass)).Cast<CreatureSizeClass>().ToList();
    private readonly IList<CreatureAlignment> _creatureAlignments = Enum.GetValues(typeof(CreatureAlignment)).Cast<CreatureAlignment>().ToList();
    private readonly IList<CreatureType> _creatureTypes = Enum.GetValues(typeof(CreatureType)).Cast<CreatureType>().ToList();
    private readonly INavigationService _navigationService;
    private readonly IDataService _dataService;
    private readonly IFilteringService _filteringService;

    [ObservableProperty]
    private AbilityFilter _abilityFilterValues;

    [ObservableProperty]
    private ObservableFlagEnum<DamageType> _resists;

    [ObservableProperty]
    private ObservableFlagEnum<DamageType> _vulnerabilities;

    [ObservableProperty]
    private ObservableFlagEnum<DamageType> _immunities;

    [ObservableProperty]
    private ObservableFlagEnum<Condition> _conditionImmunities;

    [ObservableProperty]
    private Creature? _creature;

    [ObservableProperty]
    private SpellSlotViewModel? _spellSlots;

    [ObservableProperty]
    private CRDifficultyGuide _difficultyGuide;

    [ObservableProperty]
    private ObservableEnumKeyValuePairs<CreatureSkills, CreatureSkillLevel> _skills;

    [ObservableProperty]
    private CreatureAttributeType _creatureCastingStat;

    [ObservableProperty]
    private bool showSpellSlots;

    partial void OnCreatureCastingStatChanged(CreatureAttributeType oldValue, CreatureAttributeType newValue)
    {
        ShowSpellSlots = (newValue != CreatureAttributeType.None);
        if (Creature != null)
        {
            Creature.SpellStat = newValue;
        }
    }

    public CreatureEditNavigationPageViewModel(INavigationService navigationService, IDataService dataService, IFilteringService filteringService)
    {
        _navigationService = navigationService;
        _filteringService = filteringService;
        _dataService = dataService;
        _difficultyGuide = new CRDifficultyGuide();
        Spells = new();
    }
    public ObservableCollection<Ability> CreatureAbilities
    {
        get;
        private set;
    } = new();

    public List<Ability> Spells
    {
        get; set;
    } = new();

    public IList<CreatureAttributeType> StatTypes => _creatureAttributeTypes;
    public IList<CreatureType> CreatureTypes => _creatureTypes;
    public IList<CreatureAlignment> CreatureAlignments => _creatureAlignments;
    public IList<CreatureSizeClass> CreatureSizeClasses => _creatureSizeClasses;

    public void OnNavigatedFrom() => WeakReferenceMessenger.Default.UnregisterAll(this);
    public async void OnNavigatedTo(object parameter)
    {
        if(parameter != null && parameter is Creature creature)
        {
            Creature = creature;
            if (Creature.Hyperlink == null)
                Creature.Hyperlink = "https://www.dndbeyond.com/"; //default safety net.

            SetDamageTypeInfo();
            SetConditionInfo();

            CreatureAbilities.Clear();
            foreach (var ability in Creature.Abilities)
            {
                CreatureAbilities.Add(ability);
            }
            Creature.SpellSlots ??= new int[9];

            SpellSlots = new SpellSlotViewModel(Creature.SpellSlots);

            AbilityFilterValues = (AbilityFilter)_filteringService.GetFilterValues<Ability>();
            await AbilityFilterValues.ResetAsync();

            var skillsKVP = new List<KeyValuePair<CreatureSkills, CreatureSkillLevel>>()
            {
                new(creature.NotProficient, CreatureSkillLevel.None),
                new(creature.HalfProficient, CreatureSkillLevel.HalfProficient),
                new(creature.Proficient, CreatureSkillLevel.Proficient),
                new(creature.Expertise, CreatureSkillLevel.Expertise)
            };

            Skills = new(skillsKVP);

            CreatureCastingStat = Creature.SpellStat;


        }
    }

    private void SetDamageTypeInfo()
    {
        List<DamageType> excluded = new() { DamageType.None, DamageType.Healing, DamageType.Untyped };
        Resists = new ObservableFlagEnum<DamageType>(Creature?.Resistance ?? DamageType.None, excluded);
        Immunities = new ObservableFlagEnum<DamageType>(Creature?.Immunity ?? DamageType.None, excluded);
        Vulnerabilities = new ObservableFlagEnum<DamageType>(Creature?.Vulnerability ?? DamageType.None, excluded);
    }

    private void SetConditionInfo()
    {
        List<Condition> excluded = new()
        {
            Condition.None, Condition.All,  Condition.Exhaustion2, Condition.Exhaustion3, Condition.Exhaustion4, Condition.Exhaustion5, Condition.Exhaustion6
        };

        ConditionImmunities = new ObservableFlagEnum<Condition>(Creature?.ConditionImmunities ?? Condition.None, excluded);
    }

    [RelayCommand]
    private async Task EditAbility(Ability ability)
    {
        if (ability != null && ability is Ability)
        {
            //save current state rather than discarding changes
            await CommitChanges(true);

            _navigationService.NavigateTo(typeof(AbilityEditViewModel).FullName!, ability);
        }
    }

    [RelayCommand]
    private void DeleteAbility(Ability ability)
    {
        CreatureAbilities.Remove(ability);
    }

    [RelayCommand]
    private async Task AddAbility()
    {
        var ability = new Ability();
        CreatureAbilities.Add(ability);
        await _dataService.SaveAddAsync(ability);
        await EditAbility(ability);
    }

    [RelayCommand]
    private async Task CommitChanges(object obj)
    {

        if (Creature != null)
        {
            Creature.Resistance = Resists.EnumValue; //Resists?.DamageTypes ?? DamageType.None;
            Creature.Immunity = Immunities.EnumValue;
            Creature.Vulnerability = Vulnerabilities.EnumValue;
            Creature.ConditionImmunities = ConditionImmunities.EnumValue;
            Creature.Abilities = CreatureAbilities.Select(x => x).ToList();

            var kvps = Skills.GetFlags();

            Creature.NotProficient = kvps[CreatureSkillLevel.None];
            Creature.HalfProficient = kvps[CreatureSkillLevel.HalfProficient];
            Creature.Proficient = kvps[CreatureSkillLevel.Proficient];
            Creature.Expertise = kvps[CreatureSkillLevel.Expertise];
        }

        await _dataService.SaveAddAsync(Creature);

        if (!(obj is bool && (bool)obj))
        {
            if (_navigationService.CanGoBack)
                _navigationService.GoBack();
        }
        
    }

    [RelayCommand]
    private void AddSelectedSpell(object ability)
    {
        if (ability is Ability)
        {
            CreatureAbilities.Add((Ability)ability);
        }
    }
}
