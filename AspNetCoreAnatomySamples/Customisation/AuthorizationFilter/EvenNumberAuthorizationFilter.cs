using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace AspNetCoreAnatomySamples.Customisation.AuthorizationFilter
{
    // IMPORTANT: It is NOT recommended to use this approach. Instead use authorization policies with a custom requirement/handler when needed.

    public class EvenNumberAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Request.Headers.TryGetValue(HeaderNames.Authorization, out var values))
            {
                var value = values.FirstOrDefault();

                if (!string.IsNullOrEmpty(value))
                {
                    var scheme = value.Split(' ').FirstOrDefault();
                    var credential = value.Split(' ').LastOrDefault();

                    if (!string.IsNullOrEmpty(scheme) && scheme.Equals("custom", StringComparison.OrdinalIgnoreCase) && int.TryParse(credential, out var number) && number % 2 == 0)
                    {
                        return;
                    }
                }
            }

            context.Result = new ForbidResult();
            
            // Note: If you need access to services, get them via context.HttpContext.RequestServices which is the IServiceProvider
        }
    }

    public class EvenNumberAuthorizationAttribute : TypeFilterAttribute
    {
        public EvenNumberAuthorizationAttribute() : base(typeof(EvenNumberAuthorizationFilter))
        {
        }
    }
}
