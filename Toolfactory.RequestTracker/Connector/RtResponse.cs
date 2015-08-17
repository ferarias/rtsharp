namespace Toolfactory.RequestTracker.Connector
{
    public class RtResponse
    {
        public string Version { get; set; }
        public long? StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string Body { get; set; }
    }
}