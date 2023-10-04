using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEncounters.Contracts.ViewModels
{
    public interface ITabAware
    {
        void OnTabOpened(object parameter);

        void OnTabClosed();
    }
}
