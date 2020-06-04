using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCoreAnatomySamples.Models;
using AspNetCoreAnatomySamples.Models.Output;
using Microsoft.Extensions.Caching.Memory;

namespace AspNetCoreAnatomySamples.Data
{
    public class BookRepository : IBookRepository
    {
        private readonly IMemoryCache _cache;

        public BookRepository(IMemoryCache cache) => _cache = cache;

        public readonly List<BookDto> _books = new List<BookDto>
        {
            new BookDto
            {
                Id = 1,
                ISBN = "978-1-4842-4026-7",
                Title = "Pro .NET Memory Management",
                Authors = new List<AuthorDto>{ new AuthorDto { FirstName = "Konrad", LastName = "Kokosa" }},
                PublishedDate = new DateTime(2018, 11, 13),
                LastModified = new DateTime(2020, 04, 01)
            },
            new BookDto
            {
                Id = 2,
                ISBN = "978-1-4842-9999-7",
                Title = "Out of Print BookOutputModel",
                Authors = new List<AuthorDto>{ new AuthorDto { FirstName = "Steve", LastName = "Gordon" } },
                IsExpired = true,
                PublishedDate = new DateTime(2001, 10, 20),
                LastModified = new DateTime(2005, 02, 25)
            },
            new BookDto
            {
                Id = 3,
                ISBN = "978-0735667457",
                Title = "CLR via C#",
                Authors = new List<AuthorDto>{ new AuthorDto { FirstName = "Jeffrey", LastName = "Richter" } },
                PublishedDate = new DateTime(2012, 12, 7),
                LastModified = new DateTime(2015, 11, 16)
            },
        };

        public IReadOnlyCollection<BookOutputModel> GetAll()
        {
            return _books.Select(x => x.ToOutputModel()).ToArray();
        }

        public IReadOnlyCollection<BookOutputModel> GetByDateRange(DateRange dateRange)
        {
            return _books.Where(x => x.PublishedDate > dateRange.StartDate && x.PublishedDate < dateRange.EndDate).Select(x => x.ToOutputModel()).ToArray();
        }

        public BookOutputModel GetBook(int id, bool includeOutOfPrint = false)
        {
            var book = _books.SingleOrDefault(b => b.Id == id);

            if (book is object && book.IsExpired)
                _cache.Set($"ExpiredBook.{id}", true, TimeSpan.FromHours(1));

            if (book.IsExpired && !includeOutOfPrint)
                return null;

            return book?.ToOutputModel();
        }
    }
}
