using AdilBooks.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdilBooks.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookListDto>> ListBooks();
        Task<BookDto?> FindBook(int id);
        Task<ServiceResponse> AddBook(AddBookDto addBookDto);
        Task<ServiceResponse> UpdateBook(UpdateBookDto updateBookDto);
        Task<ServiceResponse> DeleteBook(int id);
        Task<ServiceResponse> LinkAuthorToBook(int authorId, int bookId);
        Task<ServiceResponse> UnlinkAuthorFromBook(int authorId, int bookId);

        // Add this method to the interface
        Task<List<BookDto>> GetBooksByPublisher(int publisherId);  // Fetch books by publisher
    }
}
