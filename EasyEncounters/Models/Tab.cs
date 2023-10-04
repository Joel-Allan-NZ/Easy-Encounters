using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyEncounters.Contracts.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace EasyEncounters.Models
{
    public class Tab : ITab
    {
        public string Name
        {
            get;
        }

        public Page Content
        {
            get; 
        }

        public Tab(string name, Page content)
        {
            Content = content;
            Name = name;
        }
    }
}
