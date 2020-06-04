using System;
using System.Collections.Generic;

namespace AspNetCoreAnatomySamples.Models.Output
{
    public class BookOutputModel : MutableOutputModelBase
    {
        public BookOutputModel(int id) => Id = id;

        public int Id { get; }

        public string Title { get; set; }

        public string ISBN { get; set; }

        public DateTime DatePublished { get; set; }

        public IList<AuthorOutputModel> Authors { get; set; }
    }
}
