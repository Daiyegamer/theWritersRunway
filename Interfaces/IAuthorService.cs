using AdilBooks.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdilBooks.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorListDto>> ListAuthors();

        Task<AuthorDto?> FindAuthor(int id);

        Task<ServiceResponse> AddAuthor(AuthorDto authorDto);

        Task<ServiceResponse> UpdateAuthor(AuthorDto authorDto);

        Task<ServiceResponse> DeleteAuthor(int id);
    }
}
