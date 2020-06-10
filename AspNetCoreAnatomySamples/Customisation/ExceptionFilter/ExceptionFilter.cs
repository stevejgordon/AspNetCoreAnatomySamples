using AspNetCoreAnatomySamples.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreAnatomySamples.Customisation.ExceptionFilter
{
    public class HandleExceptionFilter : IExceptionFilter
    {
        private readonly IMetricRecorder _metricRecorder;
        private readonly IHostEnvironment _hostEnvironment;

        public HandleExceptionFilter(IMetricRecorder metricRecorder, IHostEnvironment hostEnvironment)
        {
            _metricRecorder = metricRecorder;
            _hostEnvironment = hostEnvironment;
        }

        public void OnException(ExceptionContext context)
        {
            // Record a metric to our monitoring system so we can alert on unhandled exceptions
            _metricRecorder.RecordException(context.Exception);

            // If we're in production, send the custom formatted content with a general error
            // If we're not in production, send the custom formatted content with the exception message
            context.Result = _hostEnvironment.IsProduction()
                ? new JsonResult(new ApiError("An unhandled error occurred.")) { StatusCode = 500}
                : new JsonResult(new ApiError(context.Exception.Message)) {StatusCode = 500};

            context.ExceptionHandled = true;
        }
    }

    // Represent a model for the custom error format we use in this service
    public class ApiError
    {
        public string Message { get; set; }
        public bool IsError { get; set; }

        public ApiError(string message)
        {
            Message = message;
            IsError = true;
        }
    }
}
