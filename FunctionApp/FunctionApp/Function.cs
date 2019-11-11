using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public static class DemoFunction
    {
        [FunctionName("Function")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            var name = req.Query["name"];
            
            CustomTelemetryInitializer.MyValue.Value = name;

            log.LogInformation("C# HTTP trigger function processed a request.");

            var telemetryItem = req.HttpContext.Features.Get<RequestTelemetry>();
            telemetryItem.Properties["SetInFunc"] = name;

            return new OkObjectResult($"Hello, {name}");
        }
    }
}
