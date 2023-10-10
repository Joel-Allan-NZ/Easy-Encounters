using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.UI;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Contracts.ViewModels;
using EasyEncounters.Core.Contracts.Services;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Core.Services;
using EasyEncounters.Messages;
using EasyEncounters.Models;
using Windows.Devices.WiFi;

namespace EasyEncounters.ViewModels;
public partial class CreatureEditViewModel : ObservableRecipientWithValidation, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IDataService _dataService;
    private readonly IValidationService _validationService;
    private readonly IList<CreatureAttributeType> _creatureAttributeTypes = Enum.GetValues(typeof(CreatureAttributeType)).Cast<CreatureAttributeType>().ToList();

    [ObservableProperty]
    private Creature _creature;

    [ObservableProperty]
    private DamageTypesViewModel _resists;

    [ObservableProperty]
    private DamageTypesViewModel _immunities;

    [ObservableProperty]
    private DamageTypesViewModel _vulnerabilities;

    [ObservableProperty]
    private ConditionTypesViewModel _conditionImmunities;

    [ObservableProperty]
    private SpellSlotViewModel _spellSlots;

    [Required]
    [CustomValidation(typeof(CreatureEditViewModel), nameof(ValidateLevelCR))]
    public double LevelCR
    {
        get => Creature.LevelOrCR;
        set => TrySetProperty(Creature.LevelOrCR, value, Creature, (v, m) => v.LevelOrCR = m, out IReadOnlyCollection<ValidationResult> errs);
    }

   
    public IList<CreatureAttributeType> StatTypes => _creatureAttributeTypes;

    public ObservableCollection<AbilityViewModel> Spells
    {
        get; set;
    } = new();

    public ObservableCollection<AbilityViewModel> CreatureAbilities
    {
        get; 
        private set;
    } =new();

    [RelayCommand]
    private async void CommitChanges(object obj)
    {
        Creature.Resistance = Resists.DamageTypes;
        Creature.Immunity = Immunities.DamageTypes;
        Creature.Vulnerability = Vulnerabilities.DamageTypes;
        Creature.ConditionImmunities = ConditionImmunities.ConditionTypes;
        Creature.Abilities = CreatureAbilities.Select(x => x.Ability).ToList();

        await _dataService.SaveAddAsync(Creature);

        if (_navigationService.CanGoBack)
            _navigationService.GoBack();
    }

    [RelayCommand]
    private async void AddCreatureAbility()
    {
        var ability = new Ability();
        Creature.Abilities.Add(ability);
        await _dataService.SaveAddAsync(Creature);

        _navigationService.NavigateTo(typeof(AbilityEditViewModel).FullName!, ability);
    }

    [RelayCommand]
    private async void SortSpellsByResolution()
    {
    
    }

    [RelayCommand]
    private async void SortSpellsByName()
    {
    
    }

    [RelayCommand]
    private async void SortSpellsByDamageType()
    {
    
    }

    public CreatureEditViewModel(INavigationService navigationService, IDataService dataService, IValidationService validationService)
    {
        _navigationService = navigationService;
        _dataService = dataService;
        _validationService = validationService;
    }

    public void OnNavigatedFrom()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
    public async void OnNavigatedTo(object parameter)
    {

        if (parameter is Creature)
        {
            //set
            Creature = (Creature)parameter;
            if (Creature.Hyperlink == null)
                Creature.Hyperlink = "https://www.dndbeyond.com/"; //default safety net.
            Resists = new DamageTypesViewModel(Creature.Resistance);
            Immunities = new DamageTypesViewModel(Creature.Immunity);
            Vulnerabilities = new DamageTypesViewModel(Creature.Vulnerability);
            ConditionImmunities = new ConditionTypesViewModel(Creature.ConditionImmunities);

            CreatureAbilities.Clear();
            foreach(var ability in Creature.Abilities)
            {
                CreatureAbilities.Add(new AbilityViewModel(ability));
            }

            WeakReferenceMessenger.Default.Register<AbilityCRUDRequestMessage>(this, (r, m) =>
            {
                HandleAbilityCRUDRequest(m.Parameter, m.RequestType);
            });

            WeakReferenceMessenger.Default.Register<AddAbilityRequestMessage>(this, (r, m) =>
            {
                CreatureAbilities.Add(m.AbilityViewModel);
            });

            WeakReferenceMessenger.Default.Register<AbilityChangeCommitMessage>(this, (r, m) =>
            {
                //todo:
                AddEditAbility(m.Ability);
            });

            SpellSlots = new SpellSlotViewModel(Creature.SpellSlots);

            var spells = await _dataService.GetAllSpellsAsync();

            foreach(var spell in spells)
            {
                Spells.Add(new AbilityViewModel(spell));
            }
        }
    }

    private async void AddEditAbility(Ability ability)
    {
        //save/update a spell to main Spell list, but just add a regular ability normally.
        //todo: move to service?

        if (ability.SpellLevel != Core.Models.Enums.SpellLevel.NotASpell)
        {
            await _dataService.SaveAddAsync(ability); //todo: make the new spell also appear in this VM's Spells list; currently it won't as we aren't adding it or refreshing the list.
        }

        var existingAbility = CreatureAbilities.FirstOrDefault(x => x.Ability == ability);

        if (existingAbility == null)
        {
            CreatureAbilities.Add(new AbilityViewModel(ability));
        }
    }

    private void HandleAbilityCRUDRequest(AbilityViewModel ability, CRUDRequestType requestType)
    {
        if(requestType == CRUDRequestType.Edit)
        {
            _navigationService.NavigateTo(typeof(AbilityEditViewModel).FullName!, ability.Ability);//Tuple.Create(Creature, ability.Ability));
        }
        else if(requestType == CRUDRequestType.Delete)
        {
            CreatureAbilities.Remove(ability);
        }
    }

    public static ValidationResult ValidateLevelCR(double levelCR, ValidationContext context)
    {
        //todo: move validation to a service.
        var instance = (CreatureEditViewModel)context.ObjectInstance;

        bool valid = instance._validationService.Validate(instance, levelCR, "LevelCR");
        if(valid)
            return ValidationResult.Success;

        return new("Not a valid Level or Challenge Rating");
    }



}
