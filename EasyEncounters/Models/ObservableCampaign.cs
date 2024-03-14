using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using EasyEncounters.Core.Models;

namespace EasyEncounters.Models
{
    public partial class ObservableCampaign : ObservableObject
    {
        [ObservableProperty]
        private Campaign _campaign;

        public string Description
        {
            get => Campaign.Description;
            set => SetProperty(Campaign.Description, value, Campaign, (m, v) => m.Description = v);
        }

        public string Name
        {
            get => Campaign.Name;
            set => SetProperty(Campaign.Name, value, Campaign, (m,v) => m.Name = v);
        }

        public ObservableCampaign(Campaign campaign)
        {
            Campaign = campaign; 
        }
    }
}
