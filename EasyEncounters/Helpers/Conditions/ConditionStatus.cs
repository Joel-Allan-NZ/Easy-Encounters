using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using EasyEncounters.Core.Models.Enums;

namespace EasyEncounters.Helpers.Conditions;
public partial class ConditionStatus : ObservableObject
{
    [ObservableProperty]
    private Condition _condition;

    [ObservableProperty]
    private bool _active;


    public ConditionStatus(Condition condition, bool active)
    {
        Condition = condition;
        Active = active;
    }
}
