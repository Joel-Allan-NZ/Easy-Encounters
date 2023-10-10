using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.ViewModels;
using Microsoft.Extensions.Options;
using Microsoft.Web.WebView2.Core;

namespace EasyEncounters.Validation
{
    public static class CreatureEditViewModelValidation
    {
        public static bool Validate<U>(CreatureEditViewModel model, U value, string propertyPath)
        {
            return propertyPath switch
            {
                "LevelCR" => ValidateLevelCR(value),
                _ => throw new NotImplementedException($"Validation for CreatureEditViewModel.{propertyPath} is not yet implemented.")
            };
        }

        private static bool ValidateLevelCR(object obj)
        {
            if(obj != null && obj is double)
            {
                var value = (double)obj;

                if (value < 0 || value > 30)
                    return false;

                if (value < 1)
                {
                    if (value != 0.25 && value != 0.125 && value != 0.5 && value != 0)
                        return false;
                }
                return true;
            }
            return false;
        }
    }
}
