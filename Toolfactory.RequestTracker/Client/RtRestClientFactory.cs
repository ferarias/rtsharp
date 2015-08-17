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

        public static readonly string RtServer = typeof(RtRestClientFactory).FullName + "_RtServer";
        public static readonly string RtUsername = typeof(RtRestClientFactory).FullName + "_RtUsername";
        public static readonly string RtPassword = typeof(RtRestClientFactory).FullName + "_RtPassword";

        public override IRtTicketClient GetRtTicketClient(StringDictionary parameters)
        {
            return new RtRestTicketClient
            {
                Client = new RtClient(
                    parameters[RtServer], 
                    parameters[RtUsername],
                    parameters[RtPassword])
            };
        }
    }
}