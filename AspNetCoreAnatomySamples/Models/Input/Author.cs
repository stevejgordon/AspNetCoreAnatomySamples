using System.Collections.Generic;

namespace AspNetCoreAnatomySamples.Models.Input
{
    public class Author
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IList<Book> Books { get; set; }
    }
}