using System.Diagnostics;
using AspNetCoreAnatomySamples.Core;

namespace AspNetCoreAnatomySamples.Customisation
{
    public class MetricMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMetricRecorder _metricRecorder;

        public MetricMiddleware(RequestDelegate next, IMetricRecorder metricRecorder)
        {
            _next = next;
            _metricRecorder = metricRecorder;
        }

        public async Task InvokeAsync(HttpContext ctx)
        {
            var stopWatch = Stopwatch.StartNew();

            await _next(ctx);

            stopWatch.Stop();
            
            _metricRecorder.RecordRequest(ctx.Response.StatusCode, stopWatch.ElapsedMilliseconds);
        }
    }
}
