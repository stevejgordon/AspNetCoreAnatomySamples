namespace AspNetCoreAnatomySamples.Core
{
    public class MetricRecorder : IMetricRecorder
    {
        private readonly ILogger<MetricRecorder> _logger;

        public MetricRecorder(ILogger<MetricRecorder> logger) => _logger = logger;

        public void RecordRequest(int statusCode, long milliseconds)
        {
            // imagine recording a metric to an metric system

            _logger.LogInformation("Returned {StatusCode} in {ElapsedMilliseconds}ms.", statusCode, milliseconds);
        }

        public void RecordException(Exception exception)
        {
            // imagine recording a metric to an metric system

            _logger.LogError(exception, "An unhandled exception occurred");
        }
    }
}
