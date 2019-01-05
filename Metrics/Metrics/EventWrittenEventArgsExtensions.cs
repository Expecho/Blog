using System.Diagnostics.Tracing;

namespace Metrics
{
    /// <summary>
    /// Extensions methods for <see cref="EventWrittenEventArgs"/>
    /// </summary>
    public static class EventWrittenEventArgsExtensions
    {
        public static bool IsEventCounter(this EventWrittenEventArgs eventData)
        {
            return eventData.EventName == "EventCounters";
        }

        public static EventCounterData ToEventCounterData(this EventWrittenEventArgs eventData)
        {
            if (!eventData.IsEventCounter())
                return null;

            return new EventCounterData(eventData);
        }
    }
}