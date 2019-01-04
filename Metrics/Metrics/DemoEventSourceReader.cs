using System;
using System.Diagnostics.Tracing;
using System.Linq;

namespace Metrics
{
    /// <inheritdoc />
    /// <summary>
    /// Implementation of <see cref="T:System.Diagnostics.Tracing.EventListener" /> that writes evententries to the debug console
    /// </summary>
    internal class CustomMetricsEventListener : EventListener
    {
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            Console.WriteLine(
                $"Payload for event {eventData.EventName} (id {eventData.EventId}, version {eventData.Version}) : " +
                $"{string.Join(", ", eventData.FlattenPayload().Select(pl => $"{pl.Key}: {pl.Value}"))}");
        }
    }
}