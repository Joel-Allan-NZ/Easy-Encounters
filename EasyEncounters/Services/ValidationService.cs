using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Contracts.Services;
using EasyEncounters.Validation;
using EasyEncounters.ViewModels;

namespace EasyEncounters.Services
{
    public class ValidationService : IValidationService
    {
        public bool Validate<T, U>(T model, U value, string propertyPath)
        {
            if(model == null) throw new ArgumentNullException("model");
            //todo: handle multiple models elegantly (match them with validationmodels maybe?)
            if(model is CreatureEditViewModel)
            {
                return CreatureEditViewModelValidation.Validate(model as CreatureEditViewModel, value, propertyPath);
            }

            throw new ArgumentException($"{typeof(T)} is not yet supported for validation");
        }
    }
}
