using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace AspNetCoreAnatomySamples.Customisation.AuthorizationFilter
{
    // *************************************************************************************************
    // *************************************************************************************************
    // IMPORTANT: It is NOT RECOMMENDED to use this approach. Instead use authorization policies with a
    // custom requirement/handler when needed. JUST BECAUSE YOU CAN, DOESN'T MEAN YOU SHOULD!
    // *************************************************************************************************
    // *************************************************************************************************

    public class EvenNumberAuthorizationAttribute : TypeFilterAttribute
    {
        public EvenNumberAuthorizationAttribute() : base(typeof(EvenNumberAuthorizationFilter))
        {
        }
    }

    public class EvenNumberAuthorizationFilter : IAuthorizationFilter
    {
        private readonly ILogger<EvenNumberAuthorizationFilter> _logger;

        public EvenNumberAuthorizationFilter(ILogger<EvenNumberAuthorizationFilter> logger) => 
            _logger = logger; // only safe to inject singleton services

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Do we have an authorization header?
            if (context.HttpContext.Request.Headers.TryGetValue(HeaderNames.Authorization, out var values))
            {
                var value = values.FirstOrDefault();

                // If so, does it have a value?
                if (!string.IsNullOrEmpty(value))
                {
                    var scheme = value.Split(' ').FirstOrDefault(); // get the scheme (before the space)
                    var credential = value.Split(' ').LastOrDefault(); // get the "credential" value (after the space)

                    // Is the scheme "custom" and is the value an even number?
                    if (!string.IsNullOrEmpty(scheme) && scheme.Equals("custom", StringComparison.OrdinalIgnoreCase) && int.TryParse(credential, out var number) && number % 2 == 0)
                    {
                        return; // If so, this passes our filter and we simply return.
                    }
                }
            }

            // If we reach here, the request is not authorized.

            _logger.LogInformation("User is forbidden.");

            context.Result = new ForbidResult();
        }
    }
}
