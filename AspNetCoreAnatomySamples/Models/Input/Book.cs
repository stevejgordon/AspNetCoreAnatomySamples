using System;
using System.Collections.Generic;

namespace AspNetCoreAnatomySamples.Models.Input
{
    public class Book
    {
        public string Title { get; set; }

        public string ISBN { get; set; }

        public DateTimeOffset YearPublished { get; set; }

        public IList<Author> Authors { get; set; }
    }
}
