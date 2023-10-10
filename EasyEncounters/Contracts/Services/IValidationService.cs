using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEncounters.Contracts.Services
{
    public interface IValidationService
    {
        bool Validate<T, U>(T model, U value, string propertyPath);
    }
}
