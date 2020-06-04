using AspNetCoreAnatomySamples.Models.Output;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetCoreAnatomySamples.Customisation.ResultFilter
{
    public class LastModifiedResultFilter : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            // Is the IActionResult an Ok with Object result and if so, is the object of type "MutableOutputModelBase"?
            if (context.Result is OkObjectResult result && result.Value is MutableOutputModelBase outputModel)
            {
                // If so, set the last modified header with the value from the output model
                context.HttpContext.Response.GetTypedHeaders().LastModified = outputModel.LastModified;
            }
        }
    }
}
