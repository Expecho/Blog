using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace Metrics
{
    [EventSource(Name = "My-CustomMetricsEventSource-Minimal")]
    public sealed class CustomMetricsEventSource : EventSource
    {
        private EventCounter methodDurationCounter;

        private Dictionary<string, EventCounter> dynamicCounters =
            new Dictionary<string, EventCounter>();

        public static CustomMetricsEventSource Log = new CustomMetricsEventSource();

        public CustomMetricsEventSource()
        {
            methodDurationCounter = new EventCounter(nameof(methodDurationCounter), this);
        }

        [NonEvent]
        public void ReportMethodDurationInMs(long milliseconds)
        {
            methodDurationCounter.WriteMetric(milliseconds);
        }

        [NonEvent]
        public void ReportMetric(string name, float value)
        {
            if (!dynamicCounters.TryGetValue(name, out EventCounter counterInstance))
            {
                counterInstance = new EventCounter(name, this);
                dynamicCounters.Add(name, counterInstance);
            }
            counterInstance.WriteMetric(value);
        }

        [Event(1, Level = EventLevel.Informational)]
        public void ApplicationStop()
        {
            WriteEvent(1);
        }
    }
}