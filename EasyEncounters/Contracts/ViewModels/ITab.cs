﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace EasyEncounters.Contracts.ViewModels
{
    public interface ITab
    {
        string Name
        {
            get;
        }

        Page Content
        {
            get;
        }



    }
}
