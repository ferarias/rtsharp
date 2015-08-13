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

        public RtClient(string baseUrl, string username, string password)
        {
            BaseUrl = baseUrl;
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
            IList<KeyValuePair<string, string>> @params = new List<KeyValuePair<string, string>>();
            @params.Add(new KeyValuePair<string, string>("user", Username));
            @params.Add(new KeyValuePair<string, string>("pass", Password));

            return GetResponse(url, @params);
        }

        public RtResponse Logout()
        {
            return GetResponse("logout");
        }

        public RtResponse SearchTickets(string query)
        {
            return SearchTickets(query, null, null);
        }

        public RtResponse SearchTickets(string query, string orderby)
        {
            return SearchTickets(query, orderby, null);
        }

        public RtResponse SearchTickets(string query, TicketSearchResponseFormat format)
        {
            return SearchTickets(query, null, format);
        }

        public RtResponse SearchTickets(string query, string orderby, TicketSearchResponseFormat format)
        {
            var @params = new List<KeyValuePair<string, string>> {new KeyValuePair<string, string>("query", query)};

            if (orderby != null)
            {
                @params.Add(new KeyValuePair<string, string>("orderby", orderby));
            }

            if (format != null)
            {
                @params.Add(new KeyValuePair<string, string>("format", format.FormatString));
            }

            return GetResponse("search/ticket", @params);
        }

        #endregion

        #region "Private methods"

        private RtResponse GetResponse(string url)
        {
            return GetResponse(url, new List<KeyValuePair<string, string>>());
        }

        private RtResponse GetResponse(string url, IList<KeyValuePair<string, string>> @params)
        {
            string responseBody;
            if (@params.Any())
            {
                var postData = new StringBuilder();
                foreach (var item in @params)
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