using System.Threading;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace FunctionApp
{
    public class CustomTelemetryInitializer : ITelemetryInitializer
    {
        public static readonly AsyncLocal<string> MyValue = new AsyncLocal<string>();

        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry is ISupportProperties propertyItem)
            {
                propertyItem.Properties["myProp"] = MyValue.Value;
            }
        }
    }
}