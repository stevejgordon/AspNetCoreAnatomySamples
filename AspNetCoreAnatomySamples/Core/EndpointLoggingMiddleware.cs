using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace AspNetCoreAnatomySamples.Core
{
    public class EndpointLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<EndpointLoggingMiddleware> _logger;

        public EndpointLoggingMiddleware(RequestDelegate next, ILogger<EndpointLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public Task InvokeAsync(HttpContext ctx)
        {
            var endpoint = ctx.GetEndpoint(); // Try to get the endpoint data from the EndpointFeature inside the FeaturesCollection

            switch (endpoint)
            {
                case RouteEndpoint routeEndpoint:
                    _logger.LogInformation($"Endpoint Display Name: {routeEndpoint.DisplayName}");
                    _logger.LogInformation($"Route Pattern: {routeEndpoint.RoutePattern}");

                    foreach (var type in routeEndpoint.Metadata.Select(md => md.GetType())) // objects include the controller/action attributes
                    {
                        _logger.LogInformation($"{type}");
                    }

                    break;

                case null: // Will be null if UseRouting has not run first, or no matched endpoint
                    _logger.LogInformation("Endpoint is null");
                    break;
            }

            return _next(ctx);
        }
    }
}
