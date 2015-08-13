using System;
using System.Collections.Specialized;
using Toolfactory.RequestTracker.Client;

namespace RTConnector
{
    static class Program
    {
        static void Main(string[] args)
        {
            // Create a client object for the RT service
            var client = RtRestClientFactory.SingleInstance.GetRtTicketClient(new StringDictionary
            {
                {RtRestClientFactory.BaseUrl, "https://rt.rhino.acme.net/REST/1.0/"},
                {RtRestClientFactory.Username, "x"},
                {RtRestClientFactory.Password, "x"}
            });

            // Use client to Query tickets
            var tickets = client.FindByQuery("( Owner = 'Nobody' OR Owner = 'RT_System' ) AND (  Queue = 'arquitectura.incidencias' OR Queue = 'arquitectura.peticiones' ) AND (  Status = 'new' OR Status = 'open' )");

            // Traverse tickets
            foreach (var ticket in tickets)
            {
                Console.WriteLine("Ticket [{0}]: '{1}'", ticket.Id, ticket.Subject);
            }
            Console.ReadKey();
        }

       
    }
}
