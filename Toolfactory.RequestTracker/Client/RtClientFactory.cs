using System.Collections.Specialized;

namespace Toolfactory.RequestTracker.Client
{
    public abstract class RtClientFactory
    {
        public static RtClientFactory GetRtClientFactory(RtClientType type)
        {
            switch (type)
            {
                case RtClientType.Rest:
                    return RtRestClientFactory.SingleInstance;
                default:
                    return null;
            }
        }

        public abstract IRtTicketClient GetRtTicketClient(StringDictionary parameters);
    }
}