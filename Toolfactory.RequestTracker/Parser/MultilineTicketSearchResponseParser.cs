using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Toolfactory.RequestTracker.Connector;
using Toolfactory.RequestTracker.Helper;
using Toolfactory.RequestTracker.Model;
using Toolfactory.RequestTracker.Parser.Processor;

namespace Toolfactory.RequestTracker.Parser
{
    public sealed class MultilineTicketSearchResponseParser : ITicketSearchResponseParser
    {
        private const string TicketsDelimiter = "\n\n--\n\n";
        private const string TicketLinesDelimiter = "\n";
        private const string NoMatchingResults = "No matching results.";
        private static readonly Regex KeyValueRegex = new Regex("^(.*?): ?(.*)");

        #region "Singleton"
        private static readonly Lazy<MultilineTicketSearchResponseParser> Singleton = new Lazy<MultilineTicketSearchResponseParser>(() => new MultilineTicketSearchResponseParser());
        public static MultilineTicketSearchResponseParser Instance { get { return Singleton.Value; } }
        private MultilineTicketSearchResponseParser() { }
        #endregion

        public RtTicket ParseTicketGetResponse(RtResponse response)
        {
            IDictionary<string, string> ticketData = new Dictionary<string, string>();

            string fieldName = null;
            var tmp = new StringBuilder();
            foreach (var ticketLine in response.Body.Split(TicketLinesDelimiter, true))
            {
                var m = KeyValueRegex.Matches(ticketLine);
                if (m.Count > 0)
                {
                    var match = m[0];
                    var groupCollection = match.Groups;

                    if (fieldName != null)
                    {
                        ticketData[fieldName] = tmp.ToString();
                        tmp.Length = 0;
                    }

                    fieldName = groupCollection[1].ToString();
                    tmp.Append(groupCollection[2]);
                }
                else
                {
                    tmp.Append(ticketLine);
                }
            }
            if (fieldName != null)
                ticketData[fieldName] = tmp.ToString();

            return ProcessTicketData(ticketData);
        }

        public IEnumerable<RtTicket> ParseTicketSearchResponse(RtResponse response)
        {
            var resultData = new List<IDictionary<string, string>>();
            if (response.Body == NoMatchingResults)
                return ProcessResultData(resultData);

            foreach (var ticketString in response.Body.Split(TicketsDelimiter, true))
            {
                IDictionary<string, string> ticketData = new Dictionary<string, string>();

                string fieldName = null;
                var tmp = new StringBuilder();
                foreach (var ticketLine in ticketString.Split(TicketLinesDelimiter, true))
                {
                    var m = KeyValueRegex.Matches(ticketLine);
                    if (m.Count > 0)
                    {
                        var match = m[0];
                        var groupCollection = match.Groups;

                        if (fieldName != null)
                        {
                            ticketData[fieldName] = tmp.ToString();
                            tmp.Length = 0;
                        }

                        fieldName = groupCollection[1].ToString();
                        tmp.Append(groupCollection[2]);
                    }
                    else
                    {
                        tmp.Append(ticketLine);
                    }
                }
                if (fieldName != null)
                    ticketData[fieldName] = tmp.ToString();

                resultData.Add(ticketData);
            }

            return ProcessResultData(resultData);
        }

        private static IList<RtTicket> ProcessResultData(IEnumerable<IDictionary<string, string>> resultData)
        {
            return resultData.Select(ProcessTicketData).ToList();
        }

        private static RtTicket ProcessTicketData(IDictionary<string, string> ticketData)
        {
            var ticket = new RtTicket();
            foreach (var e in ticketData)
            {
                var fieldProcessor = FieldProcessorRegistry.Instance.GetTicketFieldProcessor(ticket, e.Key);
                fieldProcessor.Process(ticket, e.Key, e.Value);
            }

            return ticket;
        }
    }
}