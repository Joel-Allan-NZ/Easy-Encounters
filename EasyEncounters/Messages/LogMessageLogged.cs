using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEncounters.Messages
{
    public class LogMessageLogged
    {
        public IList<string> LogMessages
        {
            get; private set;
        }
        public LogMessageLogged(IList<string> logMessages)
        {
            LogMessages = logMessages;
        }
    }
}
