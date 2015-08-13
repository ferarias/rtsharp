using System;
using System.Collections.Specialized;
using Toolfactory.RequestTracker.Connector;

namespace Toolfactory.RequestTracker.Client
{
    public class RtRestClientFactory : RtClientFactory
    {
        #region "Singleton"
        private static readonly Lazy<RtRestClientFactory> Singleton = new Lazy<RtRestClientFactory>(() => new RtRestClientFactory());
        public static RtRestClientFactory SingleInstance { get { return Singleton.Value; } }
        private RtRestClientFactory() { }
        #endregion

        public static readonly string BaseUrl = typeof(RtRestClientFactory).FullName + "_RtBaseUrl";
        public static readonly string Username = typeof(RtRestClientFactory).FullName + "_RtUsername";
        public static readonly string Password = typeof(RtRestClientFactory).FullName + "_RtPassword";

        public override IRtTicketClient GetRtTicketClient(StringDictionary parameters)
        {
            return new RtRestTicketClient
            {
                Client = new RtClient(
                    parameters[BaseUrl], 
                    parameters[Username],
                    parameters[Password])
            };
        }
    }
}