using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.Diagnostics.Tracing;

namespace OOPMetricsReader
{
    class Program
    {
        static void Main(string[] args)
        {
            // Find the process containing the target EventSource.
            var targetProcess = DiagnosticsClient.GetPublishedProcesses()
                .Select(Process.GetProcessById)
                .FirstOrDefault(process => process?.ProcessName == "Metrics");

            if (targetProcess == null)
            {
                Console.WriteLine("No process named 'Metrics' found. Exiting.");
                return;
            }

            // Define what EventSource and events to listen to.
            var providers = new List<EventPipeProvider>()
            {
                new EventPipeProvider("My-CustomMetricsEventSource-Minimal",
                    EventLevel.Informational, arguments: new Dictionary<string, string>
                    {
                        {"EventCounterIntervalSec", "1"}
                    })
            };

            // Start listening session
            var client = new DiagnosticsClient(targetProcess.Id);
            using var session = client.StartEventPipeSession(providers, false);
            using var source = new EventPipeEventSource(session.EventStream);

            // Set up output writer
            source.Dynamic.All += obj =>
            {
                if (obj.EventName == "EventCounters")
                {
                    var payload = (IDictionary<string, object>)obj.PayloadValue(0);
                    Console.WriteLine(string.Join(", ", payload.Select(p => $"{p.Key}: {p.Value}")));
                }
                else
                {
                    Console.WriteLine($"{obj.ProviderName}: {obj.EventName}");
                }
            };

            try
            {
                source.Process();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error encountered while processing events");
                Console.WriteLine(e.ToString());
            }

            Console.ReadKey();
        }
    }
}
