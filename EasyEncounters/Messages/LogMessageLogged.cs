namespace EasyEncounters.Messages
{
    public class LogMessageLogged
    {
        public LogMessageLogged(IList<string> logMessages)
        {
            LogMessages = logMessages;
        }

        public IList<string> LogMessages
        {
            get; private set;
        }
    }
}