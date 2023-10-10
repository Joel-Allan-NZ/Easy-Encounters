using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace EasyEncounters.Models
{
    public abstract class ObservableRecipientWithValidation : ObservableValidator
    {
        protected ObservableRecipientWithValidation()
            : this(WeakReferenceMessenger.Default)
        {
        }

        protected ObservableRecipientWithValidation(IMessenger messenger)
        {
            Messenger = messenger;
        }

        protected IMessenger Messenger
        {
            get;
        }
    }
}
