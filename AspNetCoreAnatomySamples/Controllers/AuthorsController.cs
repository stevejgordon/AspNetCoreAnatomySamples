using AspNetCoreAnatomySamples.Customisation.ResourceFilter;
using AspNetCoreAnatomySamples.Models.Output;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAnatomySamples.Controllers
{
    // IMAGINE THIS IS UNDER DEVELOPMENT AND IN PREVIEW

    [ApiController]
    [Route("[controller]")]
    [PreviewResourceFilter]
    [MiddlewareFilter(typeof(CompressionMiddlewarePipeline))]
    public class AuthorsController : ControllerBase
    {
        [ProducesResponseType(200)]
        public ActionResult<IEnumerable<AuthorOutputModel>> Get()
        {
            return Enumerable.Range(1, 200).Select(x => new AuthorOutputModel()).ToArray();
        }

        [Route("enumerable")]
        [ProducesResponseType(200)]
        public IEnumerable<AuthorOutputModel> GetEnumerable() // converted to ObjectResult by the framework
        {
            return Enumerable.Range(1, 200).Select(x => new AuthorOutputModel()).ToArray();
        }
    }

    public class CompressionMiddlewarePipeline
    {
        // Use Configure to define the middleware pipeline which will be applied to all actions in the AuthorsController.
        // Only required if the middleware applies to certain controllers/actions.
        // Consider "Endpoint aware" middleware, rather than middleware filters.

        public void Configure(IApplicationBuilder app)
        {
            app.UseResponseCompression();
        }
    }
}
