using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace AspNetCoreAnatomySamples.Customisation.ActionFilter
{
    public class ExpiredBookActionFilter : IActionFilter
    {
        private readonly IMemoryCache _cache;

        // Requires a dependency from DI so we use the TypeFilterAttribute below to support this.
        public ExpiredBookActionFilter(IMemoryCache cache) => _cache = cache;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Get the "id" parameter

            var id = (int)context.ActionArguments["id"];

            // Is the id in the cache of expired books?
            if (_cache.TryGetValue($"ExpiredBook.{id}", out var cacheValue) && cacheValue is bool isExpired && isExpired)
            {
                // If so, short-circuit. Do not execute the action method, return not found.
                // This avoids a DB call
                context.Result = new NotFoundResult();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }

    public class ExpiredBookFilterAttribute : TypeFilterAttribute // Derives from TypeFilterAttribute so the filter when applied can include DI dependencies.
    {
        public ExpiredBookFilterAttribute() : base(typeof(ExpiredBookActionFilter)) { }
    }
}
