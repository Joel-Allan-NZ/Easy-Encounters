using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Helpers.DamageTypes;
public partial class ToggleableDamageType : ObservableObject
{
    [ObservableProperty]
    private DamageType _damageType;

    [ObservableProperty]
    private bool _toggled;

    public ToggleableDamageType(DamageType damageType, bool toggled)
    {
        _damageType = damageType;
        _toggled = toggled;
    }
}
