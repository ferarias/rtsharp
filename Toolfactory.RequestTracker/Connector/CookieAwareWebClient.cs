using System;
using System.Net;

namespace Toolfactory.RequestTracker.Connector
{
    public class CookieAwareWebClient : WebClient
    {
        private readonly CookieContainer _cookieContainer = new CookieContainer();
        private string _lastPage;

        protected override WebRequest GetWebRequest(Uri address)
        {
            var req = base.GetWebRequest(address);
            var request = req as HttpWebRequest;
            if (request != null)
            {
                var wr = request;
                wr.CookieContainer = _cookieContainer;
                if (_lastPage != null)
                    wr.Referer = _lastPage;
            }
            _lastPage = address.ToString();
            return req;
        }
    }
}
