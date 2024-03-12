using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Core.Models.Enums;
using EasyEncounters.ViewModels;
using Microsoft.UI.Xaml.Data;

namespace EasyEncounters.Helpers;

public class ConditionToBool : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if(value is ConditionsHelper conditions && parameter is Condition condition)
        {
            return conditions.HasCondition(condition.ToString());
        }
        //if(value is Condition name && parameter is ConditionsHelper conditions)
        //{
        //    return conditions.HasCondition(name.ToString());
        //}
        ////else/* if(value == null)*/
        //{
        //    return false; //for testing
        //}
        throw new ArgumentException("Not a valid value or parameter");
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException("NYI");
    }
}
