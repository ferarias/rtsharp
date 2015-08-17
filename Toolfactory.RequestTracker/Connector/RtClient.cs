using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Toolfactory.RequestTracker.Connector
{
    public sealed class RtClient
    {
        #region "Ctors"

        private const string RtBaseUrlFormat = "https://{0}/REST/1.0/";

        public RtClient(string server, string username, string password)
        {
            BaseUrl = String.Format(RtBaseUrlFormat, server);
            Username = username;
            Password = password;
            _httpClient = new CookieAwareWebClient {Encoding = Encoding.UTF8};
        }

        #endregion

        #region "Private fields"

        private static readonly Regex ResponseBodyRegex = new Regex("^(.*) (\\d+) (.*)\n((.*\n)*)", RegexOptions.Multiline);
        private readonly CookieAwareWebClient _httpClient;

        #endregion

        #region "Properties"

        private string BaseUrl { get; set; }
        private string Username { get; set; }
        private string Password { get; set; }

        #endregion

        #region "Public Methods"

        public RtResponse Login()
        {
            var url = string.Format("user/{0}", Username);
            var parameters = new Dictionary<string, string>
            {
                {"user", Username},
                {"pass", Password}
            };

            return GetResponse(url, parameters);
        }

        public RtResponse Logout()
        {
            return GetResponse("logout");
        }

        public RtResponse SearchTickets(string query, string orderby = null, TicketSearchResponseFormat format = null)
        {
            var queryParams = new Dictionary<string, string> {{"query", query}};
            if (orderby != null) queryParams.Add("orderby", orderby);
            if (format != null) queryParams.Add("format", format.FormatString);
            return GetResponse("search/ticket", queryParams);
        }

        public RtResponse GetTicket(long id)
        {
            return GetResponse("ticket/" + id);
        }

        #endregion

        #region "Private methods"

        private RtResponse GetResponse(string url)
        {
            return GetResponse(url, new Dictionary<string, string>());
        }

        private RtResponse GetResponse(string url, Dictionary<string, string> parameters)
        {
            string responseBody;
            if (parameters.Any())
            {
                var postData = new StringBuilder();
                foreach (var item in parameters)
                    postData.Append(String.Format("{0}={1}&", item.Key, HttpUtility.UrlEncode(item.Value)));
                responseBody = _httpClient.UploadString(BaseUrl + url, postData.ToString());
            }
            else
                responseBody = _httpClient.DownloadString(BaseUrl + url);

            var matcher = ResponseBodyRegex.Matches(responseBody);

            if (matcher.Count > 0)
            {
                var match = matcher[0];
                var groupCollection = match.Groups;

                var response = new RtResponse
                {
                    Version = groupCollection[1].ToString(),
                    StatusCode = Convert.ToInt64(groupCollection[2].ToString()),
                    StatusMessage = groupCollection[3].ToString(),
                    Body = groupCollection[4].ToString().Trim()
                };
                return response;
            }
            Console.Error.WriteLine("not matched");
            return new RtResponse();
        }

        #endregion
    }
}