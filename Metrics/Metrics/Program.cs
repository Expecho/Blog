using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Threading;

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
            for (int i = 0; i <= 10; i++)
            {
                SleepingBeauty(random.Next(10, 200));
            }

            Console.ReadKey();
        }

        static void SleepingBeauty(int sleepTimeInMs)
        {
            var stopwatch = Stopwatch.StartNew();

            CustomMetricsEventSource.Log.ReportMethodCalled();

            Thread.Sleep(sleepTimeInMs);

            stopwatch.Stop();

            CustomMetricsEventSource.Log.ReportMethodDurationInMs(stopwatch.ElapsedMilliseconds);
            CustomMetricsEventSource.Log.ReportMetric("SomeCounter", DateTime.Now.Millisecond);
        }
    }
}