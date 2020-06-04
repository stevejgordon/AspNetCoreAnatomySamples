using System;
using System.Collections.Generic;
using AspNetCoreAnatomySamples.Customisation.ActionFilter;
using AspNetCoreAnatomySamples.Customisation.AuthorizationFilter;
using AspNetCoreAnatomySamples.Data;
using AspNetCoreAnatomySamples.Models;
using AspNetCoreAnatomySamples.Models.Output;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetCoreAnatomySamples.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBookRepository _bookRepository;

        public BooksController(ILogger<BooksController> logger, IBookRepository bookRepository)
        {
            _logger = logger;
            _bookRepository = bookRepository;
        }

        [HttpGet]
        [EvenNumberAuthorization]
        public ActionResult<IEnumerable<BookOutputModel>> Get()
        {
            return Ok(_bookRepository.GetAll());
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(404)]
        [ExpiredBookFilter]
        public ActionResult<BookOutputModel> Get(int id)
        {
            var book = _bookRepository.GetBook(id);

            if (book is null) return NotFound();

            return Ok(book);
        }

        [ProducesResponseType(200)]
        [HttpGet("by-range")]
        [TwoYearDateRangeFilter]
        public ActionResult<IEnumerable<BookOutputModel>> GetByDateRange(DateRange dateRange)
        {
            _logger.LogInformation($"The provided date range was '{dateRange}'.");

            return Ok(_bookRepository.GetByDateRange(dateRange));
        }

        [HttpGet("bang")]
        public IActionResult GetGoesBang()
        {
            throw new Exception("We didn't handle this too well, did we?");
        }
    }
}
