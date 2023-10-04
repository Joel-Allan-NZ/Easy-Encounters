using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using EasyEncounters.Core.Models;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.Helpers;
using Microsoft.UI.Xaml;

namespace EasyEncounters.Models
{
    public partial class ObservableAbility : ObservableObject
    {
        private readonly Ability _ability;
        public string Name
        {
            get => _ability.Name;
            set => SetProperty(_ability.Name, value, _ability, (m, v) => m.Name = v);
        }
        
        public bool Concentration
        {
            get => _ability.Concentration;
            set => SetProperty(_ability.Concentration, value, _ability, (m, v) => m.Concentration = v);
        }

        public ResolutionType Resolution
        {
            get => _ability.Resolution;
            set
            {
                SetProperty(_ability.Resolution, value, _ability, (m, v) => m.Resolution = v);
                NeedsSaveType = _ability.Resolution == ResolutionType.SavingThrow;
            }
        }

        public int TargetCount
        {
            get => _ability.TargetCount;
            set => SetProperty(_ability.TargetCount, value, _ability, (m, v) => m.TargetCount = v);
        }

        public int TargetSize
        {
            get => _ability.TargetSize;
            set => SetProperty(_ability.TargetSize, value, _ability, (m, v) => m.TargetSize = v);
        }

        public int TargetDistance
        {
            get => _ability.TargetDistance;
            set => SetProperty(_ability.TargetDistance, value, _ability, (m, v) => m.TargetDistance = v);
        }

        public TargetAreaType TargetAreaType
        {
            get => _ability.TargetAreaType;
            set
            {
                SetProperty(_ability.TargetAreaType, value, _ability, (m, v) => m.TargetAreaType = v);
                NeedsArea = _ability.TargetAreaType != TargetAreaType.Creatures && _ability.TargetAreaType != TargetAreaType.None;
            }
        }

        public string EffectDescription
        {
            get => _ability.EffectDescription;
            set => SetProperty(_ability.EffectDescription, value, _ability, (m, v) => m.EffectDescription = v);
        }

        public DamageType DamageType
        {
            get => _ability.DamageTypes;
            set => SetProperty(_ability.DamageTypes, value, _ability, (m, v) => m.DamageTypes = v);
        }
        public CreatureAttributeType ResolutionStat
        {
            get => _ability.ResolutionStat;
            set => SetProperty(_ability.ResolutionStat, value, _ability, (m, v) => m.ResolutionStat = v);
        }
        public SpellCastComponent SpellCastComponents
        {
            get => _ability.CastingComponents;
            set => SetProperty(_ability.CastingComponents, value, _ability, (m, v) => m.CastingComponents = v);
        }
        public ActionSpeed ActionSpeed
        {
            get => _ability.ActionSpeed;
            set => SetProperty(_ability.ActionSpeed, value, _ability, (m, v) => m.ActionSpeed = v);
        }
        public string MaterialCost
        {
            get => _ability.MaterialCost;
            set => SetProperty(_ability.MaterialCost, value, _ability, (m, v) => m.MaterialCost = v);
        }
        public SpellLevel SpellLevel
        {
            get => _ability.SpellLevel;
            set
            {
                SetProperty(_ability.SpellLevel, value, _ability, (m, v) => m.SpellLevel = v);
                IsSpell = _ability.SpellLevel != SpellLevel.NotASpell;
            }
        }
        public ActionRangeType TargetDistanceType
        {
            get => _ability.TargetDistanceType;
            set
            {
                SetProperty(_ability.TargetDistanceType, value, _ability, (m, v) => m.TargetDistanceType = v);
                NeedsRangeValue = _ability.TargetDistanceType == ActionRangeType.Feet || _ability.TargetDistanceType == ActionRangeType.Mile;
            }
        }
        public CreatureAttributeType SaveType
        {
            get => _ability.SaveType;
            set => SetProperty(_ability.SaveType, value, _ability, (m, v) => m.SaveType = v);
        }
        public TimeDuration TimeDurationType
        {
            get => _ability.TimeDurationType;
            set {
                SetProperty(_ability.TimeDurationType, value, _ability, (m, v) => m.TimeDurationType = v);
                NeedsTime = _ability.TimeDurationType != Core.Models.Enums.TimeDuration.Permanent && 
                    _ability.TimeDurationType != Core.Models.Enums.TimeDuration.Instantaneous;
            }
        }

        public int TimeDuration
        {
            get => _ability.TimeDuration;
            set => SetProperty(_ability.TimeDuration, value, _ability, (m, v) => m.TimeDuration = v);
        }

        public MagicSchool MagicSchool
        {
            get => _ability.MagicSchool;
            set => SetProperty(_ability.MagicSchool, value, _ability, (m, v) => m.MagicSchool = v);
        }

        [ObservableProperty]
        private bool _isSpell;

        [ObservableProperty]
        private bool _needsRangeValue;

        [ObservableProperty]
        private bool _needsSaveType;

        [ObservableProperty]
        private bool _needsArea;

        [ObservableProperty]
        private bool _needsTime;



        public ObservableAbility(Ability ability)
        {
            _ability = ability;

            IsSpell = ability.SpellLevel != SpellLevel.NotASpell;
            NeedsSaveType = ability.Resolution == ResolutionType.SavingThrow;
            NeedsRangeValue = ability.TargetDistanceType == ActionRangeType.Feet || ability.TargetDistanceType == ActionRangeType.Mile;
            NeedsArea = ability.TargetAreaType != TargetAreaType.Creatures && ability.TargetAreaType != TargetAreaType.None;
            NeedsTime = ability.TimeDurationType != Core.Models.Enums.TimeDuration.Permanent && ability.TimeDurationType != Core.Models.Enums.TimeDuration.Instantaneous;
        }
    }
}
