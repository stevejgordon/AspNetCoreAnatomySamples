using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCoreAnatomySamples.Models.Output;

namespace AspNetCoreAnatomySamples.Data
{
    public class BookDto : DtoBase
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ISBN { get; set; }

        public DateTime PublishedDate { get; set; }
        
        public IList<AuthorDto> Authors { get; set; }

        public bool IsExpired { get; set; }

        public BookOutputModel ToOutputModel() => new BookOutputModel(Id)
        {
            Title = Title,
            ISBN = ISBN,
            DatePublished = PublishedDate,
            Authors = Authors?.Select(x => x.ToOutputModel()).ToList(),
            LastModified = LastModified
        };

        public static BookDto FromOutputModel(BookOutputModel bookOutputModel) => new BookDto
        {
            Id = bookOutputModel.Id,
            Title = bookOutputModel.Title,
            ISBN = bookOutputModel.ISBN,
            PublishedDate = bookOutputModel.DatePublished,
            Authors = bookOutputModel.Authors?.Select(AuthorDto.FromOutputModel).ToList(),
            LastModified = bookOutputModel.LastModified
        };
    }
}
