using System.Collections.Generic;
using Toolfactory.RequestTracker.Connector;
using Toolfactory.RequestTracker.Model;

namespace Toolfactory.RequestTracker.Parser
{
    public interface ITicketSearchResponseParser
    {
        IEnumerable<RtTicket> ParseTicketSearchResponse(RtResponse response);
    }
}