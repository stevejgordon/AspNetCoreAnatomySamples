using System.Collections.Generic;
using AspNetCoreAnatomySamples.Customisation.AuthorizationFilter;
using AspNetCoreAnatomySamples.Models.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetCoreAnatomySamples.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;

        public BooksController(ILogger<BooksController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        //[EvenNumberAuthorization]
        public IEnumerable<Book> Get()
        {
            return new Book[0];
        }

        // TODO - Custom Model Binding - Date Range
    }
}
