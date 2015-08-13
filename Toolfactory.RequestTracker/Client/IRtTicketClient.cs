using System.Collections.Generic;
using Toolfactory.RequestTracker.Connector;
using Toolfactory.RequestTracker.Model;

namespace Toolfactory.RequestTracker.Client
{
    public interface IRtTicketClient
    {
        RtTicket FindById(long id);
        IEnumerable<RtTicket> FindByQuery(string query);
        IEnumerable<RtTicket> FindByQuery(string query, string orderby);
        IEnumerable<RtTicket> FindByQuery(string query, TicketSearchResponseFormat format);
        IEnumerable<RtTicket> FindByQuery(string query, string orderby, TicketSearchResponseFormat format);
    }
}