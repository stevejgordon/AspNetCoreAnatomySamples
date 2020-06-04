using Microsoft.AspNetCore.Builder;

namespace AspNetCoreAnatomySamples.Customisation
{
    public static class MetricMiddlewareExtensions
    {
        public static IApplicationBuilder UseMetrics(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MetricMiddleware>();
        }
    }
}
