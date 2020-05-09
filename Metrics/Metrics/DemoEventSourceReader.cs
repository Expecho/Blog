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
            var counterData = eventData.ToEventCounterData();

            if (counterData == null)
                Console.WriteLine($"Event received: {eventData.EventId} - {eventData.EventName}");
            // Only write to console if actual data has been reported
            else if (counterData?.Count == 0)
                return;
            else
                Console.WriteLine(
                    $"Counter {counterData.Name} reported values " +
                    $"Min: {counterData.Min}, " +
                    $"Max: {counterData.Max}, " +
                    $"Count {counterData.Count}, " +
                    $"Mean {counterData.Mean}, " +
                    $"StandardDeviation: {counterData.StandardDeviation}, " +
                    $"IntervalSec: {counterData.IntervalSec}");
        }
    }
}