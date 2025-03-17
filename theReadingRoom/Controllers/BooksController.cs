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

        // Dependency Injection
        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
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
            var book = await _bookService.FindBook(id);
            if (book == null)
            {
                return NotFound(new { error = "NotFound", message = "Book not found." });
            }
            return Ok(new { message = "Book retrieved successfully.", data = book });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateBook(UpdateBookDto updateBookDto)
        {
            var response = await _bookService.UpdateBook(updateBookDto);
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
