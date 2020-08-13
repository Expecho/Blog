using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Threading;
using System.Threading.Tasks;

namespace Metrics
{
    class Program
    {
        static void Main(string[] args)
        {
            var reader = new CustomMetricsEventListener();
            var arguments = new Dictionary<string, string>
            {
                {"EventCounterIntervalSec", "1"}
            };
            reader.EnableEvents(CustomMetricsEventSource.Log, EventLevel.LogAlways, EventKeywords.All, arguments);

            var random = new Random();

            var cts = new CancellationTokenSource();

            Task.Run(() =>
            {
                while (!cts.IsCancellationRequested)
                {
                    SleepingBeauty(random.Next(10, 200));
                }
            });

            Console.WriteLine("Press any key to stop");
            Console.ReadKey();

            cts.Cancel();

            CustomMetricsEventSource.Log.ApplicationStop();
        }

        static void SleepingBeauty(int sleepTimeInMs)
        {
            var stopwatch = Stopwatch.StartNew();

            Thread.Sleep(sleepTimeInMs);

            stopwatch.Stop();

            CustomMetricsEventSource.Log.ReportMethodDurationInMs(stopwatch.ElapsedMilliseconds);
            CustomMetricsEventSource.Log.ReportMetric("someCounter", DateTime.Now.Millisecond);
        }
    }
}