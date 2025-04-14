using AdilBooks.Data;
using AdilBooks.Interfaces;
using AdilBooks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdilBooks.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;

        // Dependency injection of ApplicationDbContext
        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookListDto>> ListBooks()
        {
            var books = await _context.Books
                .Select(b => new BookListDto
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    Year = b.Year
                })
                .ToListAsync();

            return books;
        }

        public async Task<BookDto> FindBook(int id)
        {
            var book = await _context.Books
                .Include(b => b.Authors) // Ensure authors are included
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                return null;
            }

            return new BookDto
            {
                BookId = book.BookId,
                Title = book.Title,
                Year = book.Year,
                Synopsis = book.Synopsis,
                PublisherName = book.Publisher.PublisherName,

                // Populate LinkedAuthors with Author IDs & Names
                LinkedAuthors = book.Authors.Select(a => new ListAuthorDto
                {
                    AuthorId = a.AuthorId,
                    AuthorName = a.Name
                }).ToList()
            };
        }


        public async Task<ServiceResponse> UpdateBook(UpdateBookDto updateBookDto)
        {
            ServiceResponse serviceResponse = new();

            try
            {
                var book = await _context.Books.FindAsync(updateBookDto.BookId);
                if (book == null)
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                    serviceResponse.Messages.Add("Book not found.");
                    return serviceResponse;
                }

                book.Title = updateBookDto.Title;
                book.Year = updateBookDto.Year;
                book.Synopsis = updateBookDto.Synopsis;

                _context.Entry(book).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
                serviceResponse.Messages.Add("Book updated successfully.");
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add($"An error occurred while updating the book: {ex.Message}");
                return serviceResponse;
            }
        }
        public async Task<ServiceResponse> AddBook(AddBookDto addBookDto)
        {
            ServiceResponse serviceResponse = new();

            try
            {
                // 🔹 Get or Create Publisher
                var publisher = await _context.Publishers
                    .FirstOrDefaultAsync(p => p.PublisherName == addBookDto.PublisherName);

                if (publisher == null)
                {
                    publisher = new Publisher { PublisherName = addBookDto.PublisherName };
                    _context.Publishers.Add(publisher);
                    await _context.SaveChangesAsync();
                    serviceResponse.Messages.Add("New publisher added.");
                }

                var authors = new List<Author>();
                foreach (var authorName in addBookDto.AuthorNames)
                {
                    if (string.IsNullOrWhiteSpace(authorName)) continue; // ✅ Skip blank names

                    var author = await _context.Authors
                        .FirstOrDefaultAsync(a => a.Name == authorName.Trim());

                    if (author == null)
                    {
                        // ✅ Create new author with defaults
                        author = new Author
                        {
                            Name = authorName.Trim(),
                            Bio = "Biography not provided.",
                            ImagePath = null,
                            Books = new List<Book>()
                        };

                        _context.Authors.Add(author);
                        await _context.SaveChangesAsync();

                        serviceResponse.Messages.Add($"New author '{authorName}' added.");
                    }

                    authors.Add(author);
                }

                var book = new Book
                {
                    Title = addBookDto.Title,
                    Year = addBookDto.Year,
                    Synopsis = addBookDto.Synopsis,
                    Publisher = publisher,
                    Authors = authors
                };

                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
                serviceResponse.CreatedId = book.BookId;
                serviceResponse.Messages.Add("Book added successfully.");
                serviceResponse.Data = new BookDto
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    Year = book.Year,
                    Synopsis = book.Synopsis,
                    PublisherName = book.Publisher.PublisherName,
                    AuthorNames = book.Authors.Select(a => a.Name).ToList()
                };

                return serviceResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Book Creation Error: " + ex.ToString());

                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add($"An error occurred while adding the book: {ex.Message}");
                return serviceResponse;
            }
        }


        public async Task<ServiceResponse> DeleteBook(int id)
        {
            ServiceResponse response = new();

            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    response.Status = ServiceResponse.ServiceStatus.NotFound;
                    response.Messages.Add("Book not found.");
                    return response;
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                response.Status = ServiceResponse.ServiceStatus.Deleted;
                response.Messages.Add("Book deleted successfully.");
                return response;
            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add($"An error occurred while deleting the book: {ex.Message}");
                return response;
            }
        }

        public async Task<ServiceResponse> LinkAuthorToBook(int bookId, int authorId)
        {
            ServiceResponse serviceResponse = new();

            try
            {
                var book = await _context.Books
                    .Include(b => b.Authors)
                    .FirstOrDefaultAsync(b => b.BookId == bookId);

                var author = await _context.Authors.FindAsync(authorId);

                if (book == null || author == null)
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                    serviceResponse.Messages.Add("Book or Author not found.");
                    return serviceResponse;
                }

                if (!book.Authors.Contains(author))
                {
                    book.Authors.Add(author);
                    await _context.SaveChangesAsync();
                    serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
                    serviceResponse.Messages.Add("Author linked to book successfully.");
                }
                else
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                    serviceResponse.Messages.Add("Author already linked to this book.");
                }

                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add($"Error linking author to book: {ex.Message}");
                return serviceResponse;
            }
        }
        public async Task<List<BookDto>> GetBooksByPublisher(int publisherId)
        {
            return await _context.Books
                .Where(b => b.PublisherId == publisherId)
                .Select(b => new BookDto
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    Year = b.Year
                })
                .ToListAsync();
        }

        public async Task<ServiceResponse> UnlinkAuthorFromBook(int bookId, int authorId)
        {
            ServiceResponse serviceResponse = new();

            try
            {
                var book = await _context.Books
                    .Include(b => b.Authors)
                    .FirstOrDefaultAsync(b => b.BookId == bookId);

                var author = await _context.Authors.FindAsync(authorId);

                if (book == null || author == null)
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                    serviceResponse.Messages.Add("Book or Author not found.");
                    return serviceResponse;
                }

                if (book.Authors.Contains(author))
                {
                    book.Authors.Remove(author);
                    await _context.SaveChangesAsync();
                    serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
                    serviceResponse.Messages.Add("Author unlinked from book successfully.");
                }
                else
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                    serviceResponse.Messages.Add("Author is not linked to this book.");
                }

                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add($"Error unlinking author from book: {ex.Message}");
                return serviceResponse;
            }
        }
    }
}
