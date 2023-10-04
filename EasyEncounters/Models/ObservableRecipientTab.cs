using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using EasyEncounters.Contracts.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace EasyEncounters.Models
{
    public abstract class ObservableRecipientTab : ObservableRecipient, ITabAware
    {
        public Page? Content
        {
            get; set;
        }

        public string? TabName
        {
            get; set;
        }
        
        public bool IsClosable
        {
            get; set;
        }

        public abstract void OnTabClosed();
        public abstract void OnTabOpened(object parameter);
    }
}
