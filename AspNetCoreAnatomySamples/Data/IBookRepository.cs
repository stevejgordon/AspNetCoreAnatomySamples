using System.Collections.Generic;
using AspNetCoreAnatomySamples.Models;
using AspNetCoreAnatomySamples.Models.Output;

namespace AspNetCoreAnatomySamples.Data
{
    public interface IBookRepository
    {
        IReadOnlyCollection<BookOutputModel> GetAll();

        IReadOnlyCollection<BookOutputModel> GetByDateRange(DateRange dateRange);

        BookOutputModel GetBook(int id, bool includeOutOfPrint = false);
    }
}
