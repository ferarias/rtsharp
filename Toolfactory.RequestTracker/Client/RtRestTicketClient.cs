using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Authentication;
using Toolfactory.RequestTracker.Connector;
using Toolfactory.RequestTracker.Model;
using Toolfactory.RequestTracker.Parser;

namespace Toolfactory.RequestTracker.Client
{
    public sealed class RtRestTicketClient : IRtTicketClient
    {
        public RtClient Client { get; set; }

        public RtTicket FindById(long id)
        {
            Client.Login();
            var response = Client.GetTicket(id);
            Client.Logout();
            var parser = MultilineTicketSearchResponseParser.Instance;
            switch (response.StatusCode)
            {
                case 200:
                    return parser.ParseTicketGetResponse(response);
                case 401:
                    throw new InvalidCredentialException(response.StatusMessage);
                default:
                    throw new IOException(String.Format("Server returned {0} ({1})", response.StatusCode, response.StatusMessage));
            }
        }

        public IEnumerable<RtTicket> FindByQuery(string query)
        {
            return FindByQuery(query, null, TicketSearchResponseFormat.MultiLine);
        }

        public IEnumerable<RtTicket> FindByQuery(string query, string orderby)
        {
            return FindByQuery(query, orderby, TicketSearchResponseFormat.MultiLine);
        }

        public IEnumerable<RtTicket> FindByQuery(string query, TicketSearchResponseFormat format)
        {
            return FindByQuery(query, null, format);
        }

        public IEnumerable<RtTicket> FindByQuery(string query, string orderby, TicketSearchResponseFormat format)
        {
            Client.Login();
            var response = Client.SearchTickets(query, orderby, format);
            Client.Logout();
            ITicketSearchResponseParser parser = null;

            switch (format.InnerEnumValue())
            {
                case TicketSearchResponseFormat.InnerEnum.IdOnly:
                    break;
                case TicketSearchResponseFormat.InnerEnum.IdAndSubject:
                    break;
                case TicketSearchResponseFormat.InnerEnum.MultiLine:
                    parser = MultilineTicketSearchResponseParser.Instance;
                    break;
            }

            if (parser == null) throw new NotSupportedException("Could not create parser for response format.");

            switch (response.StatusCode)
            {
                case 200:
                    return parser.ParseTicketSearchResponse(response);
                case 401:
                    throw new InvalidCredentialException(response.StatusMessage);
                default:
                    throw new IOException(String.Format("Server returned {0} ({1})", response.StatusCode, response.StatusMessage));
            }

        }
    }
}