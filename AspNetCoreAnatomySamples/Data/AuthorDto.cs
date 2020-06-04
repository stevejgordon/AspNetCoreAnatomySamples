using AspNetCoreAnatomySamples.Models.Output;

namespace AspNetCoreAnatomySamples.Data
{
    public class AuthorDto : DtoBase
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public AuthorOutputModel ToOutputModel() => new AuthorOutputModel
        {
            FirstName = FirstName,
            LastName = LastName
        };

        public static AuthorDto FromOutputModel(AuthorOutputModel authorOutputModel) => new AuthorDto
        {
            FirstName = authorOutputModel.FirstName,
            LastName = authorOutputModel.LastName
        };
    }
}
