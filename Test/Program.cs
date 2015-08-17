using System;
using System.Collections.Specialized;
using System.Configuration;
using CommandLine;
using Toolfactory.RequestTracker.Client;

namespace RtUtil
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine("\tRequest Tracker Utility");
            Console.WriteLine();

            // Try to parse options from command line
            var options = new Options();
            if (Parser.Default.ParseArguments(args, options))
            {
                try
                {
                    // Set values from parameters or from app.config
                    var server = !String.IsNullOrEmpty(options.Server) ? options.Server : ConfigurationManager.AppSettings["RtServer"];
                    var user = !String.IsNullOrEmpty(options.User) ? options.User : ConfigurationManager.AppSettings["RtUser"];
                    var password = !String.IsNullOrEmpty(options.Password) ? options.Password : ConfigurationManager.AppSettings["RtPassword"];

                    Console.WriteLine("\tRT Server: {0}", server);
                    Console.WriteLine("\tUser: {0}", user);
                    Console.WriteLine("\tPassword: {0}", String.IsNullOrEmpty(password) ? "Not set" : "Set");
                    Console.WriteLine("\tQuery: {0}", options.Query);

                    // Create a client object for the RT service
                    var client = RtRestClientFactory.SingleInstance.GetRtTicketClient(new StringDictionary
                    {
                        {RtRestClientFactory.RtServer, server},
                        {RtRestClientFactory.RtUsername, user},
                        {RtRestClientFactory.RtPassword, password}
                    });

                    Console.WriteLine("-------------------------------------------------------------------------------");

                    // Use client to Query tickets
                    var tickets = client.FindByQuery(options.Query);

                    // Traverse tickets
                    foreach (var ticket in tickets)
                    {
                        Console.WriteLine(ticket);
                    }
                    //Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nAn error occured:\n{0}\n", ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Couldn't read options!");
                Console.WriteLine();
            }
        }
    }
}