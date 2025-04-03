using Microsoft.AspNetCore.Mvc;
using AdilBooks.Models;
using AdilBooks.Services;
using System.Threading.Tasks;
using AdilBooks.Interfaces;

namespace AdilBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ApplicationDbContext _context;

        // Dependency Injection
        public BooksController(IBookService bookService, ApplicationDbContext context)
        {
            _bookService = bookService;
            _context = context;
        }

        [HttpGet("List")]
        public async Task<IActionResult> ListBooks()
        {
            var books = await _bookService.ListBooks();
            return Ok(new { message = "Books retrieved successfully.", data = books });
        }

        [HttpGet("Find/{id}")]
        public async Task<IActionResult> FindBook(int id)
        {
            var book = await _context.Books
                .Include(b => b.Authors)
                .Include(b => b.Publisher)
                .Include(b => b.DesignerBooks)
                    .ThenInclude(db => db.Designer)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                return NotFound(new { error = "NotFound", message = "Book not found." });
            }

            var bookDto = new BookDto
            {
                BookId = book.BookId,
                Title = book.Title,
                Year = book.Year,
                Synopsis = book.Synopsis,
                PublisherName = book.Publisher?.PublisherName,
                AuthorNames = book.Authors?.Select(a => a.Name).ToList(),
                LinkedDesigners = book.DesignerBooks?.Select(db => new DesignerInfoDto
                {
                    DesignerId = db.Designer.DesignerId,
                    Name = db.Designer.Name,
                    Category = db.Designer.Category
                }).ToList() ?? new List<DesignerInfoDto>()
            };

            return Ok(new { message = "Book retrieved successfully.", data = bookDto });
        }


        [HttpPost("Add")]
        public async Task<IActionResult> AddBook(AddBookDto addBookDto)
        {
            var response = await _bookService.AddBook(addBookDto);
            if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, new { error = "InternalServerError", message = response.Messages });
            }
            return CreatedAtAction(nameof(FindBook), new { id = response.CreatedId }, new { message = "Book added successfully.", data = response.Data });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var response = await _bookService.DeleteBook(id);
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(new { error = "NotFound", message = response.Messages });
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, new { error = "InternalServerError", message = response.Messages });
            }

            return NoContent();
        }

        [HttpPost("LinkAuthorToBook/{bookId}/{authorId}")]
        public async Task<IActionResult> LinkAuthorToBook(int bookId, int authorId)
        {
            var response = await _bookService.LinkAuthorToBook(bookId, authorId);
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(new { error = "NotFound", message = response.Messages });
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, new { error = "InternalServerError", message = response.Messages });
            }

            return Ok(new { message = response.Messages });
        }

        [HttpPost("UnlinkAuthorFromBook/{bookId}/{authorId}")]
        public async Task<IActionResult> UnlinkAuthorFromBook(int bookId, int authorId)
        {
            var response = await _bookService.UnlinkAuthorFromBook(bookId, authorId);
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(new { error = "NotFound", message = response.Messages });
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, new { error = "InternalServerError", message = response.Messages });
            }

            return Ok(new { message = response.Messages });
        }
    }
}
