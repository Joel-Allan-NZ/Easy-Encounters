using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.WinUI.Helpers;
using EasyEncounters.Core.Models.Enums;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace EasyEncounters.Helpers
{
    public class SkillLevelToFontColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (value is CreatureSkillLevel skillLevel)
                {
                    return skillLevel switch
                    {
                        CreatureSkillLevel.Expertise => App.Current.Resources["AccentTextFillColorPrimaryBrush"],
                        //CreatureSkillLevel.Proficient => "\uE735",
                        //CreatureSkillLevel.HalfProficient => "\uF0E7",
                        //CreatureSkillLevel.None => "\uE734",
                        _ => App.Current.Resources["SystemColorWindowTextColor"],
                    };; ;
                }
            }
            catch
            {
                return "\uE734";
            }
            throw new ArgumentException($"{value} is not a supported type.");
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
