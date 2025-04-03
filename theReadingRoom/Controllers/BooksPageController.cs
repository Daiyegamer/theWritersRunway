using AdilBooks.Interfaces;
using AdilBooks.Models;
using AdilBooks.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AdilBooks.Models.ViewModels;
using AdilBooks.Data;
using Microsoft.EntityFrameworkCore;



namespace AdilBooks.Controllers
{
    [Route("Books")]
    
    public class BooksPageController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        private readonly ApplicationDbContext _context;

        public BooksPageController(IBookService bookService, IAuthorService authorService, IPublisherService publisherService, ApplicationDbContext context)
        {
            _bookService = bookService;
            _authorService = authorService;  // Initialize _authorService
            _context = context;
        }

        // GET: Books/List
        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            IEnumerable<BookListDto> books = await _bookService.ListBooks();
            return View(books);  // Looks for Views/Books/List.cshtml
        }

        // GET: Books/Find/{id}
        [HttpGet("Find/{id}")]
        public async Task<IActionResult> Find(int id)
        {
            var book = await _bookService.FindBook(id);
            if (book == null)
            {
                return View("Error", new ErrorViewModel { Errors = new List<string> { "Book not found." } });
            }

            var availableAuthors = (await _authorService.ListAuthors()).ToList();

            var linkedDesigners = await _context.DesignerBooks
                .Where(db => db.BookId == id)
                .Include(db => db.Designer)
                .Select(db => db.Designer)
                .ToListAsync();

            var model = new BookWithAuthorsViewModel
            {
                Book = book,
                AvailableAuthors = availableAuthors,
                LinkedAuthors = new List<AuthorListDto>(), 
                LinkedDesigners = linkedDesigners 
            };

            return View(model);
        }




        [HttpGet("Add")]
        public IActionResult Add()
        {
            return View(); // Render the "Add" form (Views/Books/Add.cshtml)
        }

        // POST: Books/Add
        // [Authorize]
        [HttpPost("Add")]
        public async Task<IActionResult> Add(AddBookDto addBookDto)
        {
            if (!ModelState.IsValid)
            {
                return View(addBookDto); // Return to the "Add" view with the invalid model
            }

            ServiceResponse response = await _bookService.AddBook(addBookDto);
            if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return View("Error", new ErrorViewModel { Errors = response.Messages });
            }
            TempData["SuccessMessage"] = "Book added successfully!";
            // Redirect to Find to show the details of the added book
            return RedirectToAction("Find", new { id = response.CreatedId });
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            // Fetch the book details
            BookDto book = await _bookService.FindBook(id);

            // If the book is not found, show the error page
            if (book == null)
            {
                return View("Error", new ErrorViewModel { Errors = new List<string> { "Book not found." } });
            }

            // Create an UpdateBookDto to pass to the view
            UpdateBookDto updateBookDto = new UpdateBookDto
            {
                BookId = book.BookId,
                Title = book.Title,
                Year = book.Year,
                Synopsis = book.Synopsis
            };

            // Pass the UpdateBookDto to the view
            return View(updateBookDto); // Looks for Views/BooksPage/Edit.cshtml
        }

        // POST: Books/Update
        // [Authorize]
        [HttpPost("Update")]
        public async Task<IActionResult> Update(UpdateBookDto updateBookDto)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", updateBookDto);
            }

            ServiceResponse response = await _bookService.UpdateBook(updateBookDto);
            if (response.Status == ServiceResponse.ServiceStatus.Error || response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return View("Error", new ErrorViewModel { Errors = response.Messages });
            }
            TempData["SuccessMessage"] = "Book updated successfully!";
            return RedirectToAction("Find", new { id = updateBookDto.BookId });
        }

        // GET: Books/ConfirmDelete/{id}
        [HttpGet("ConfirmDelete/{id}")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            BookDto book = await _bookService.FindBook(id);
            if (book == null)
            {
                return View("Error", new ErrorViewModel { Errors = new List<string> { "Book not found." } });
            }
            return View(book); // Looks for Views/Books/ConfirmDelete.cshtml
        }

        // POST: Books/Delete/{id}
        // [Authorize]
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _bookService.DeleteBook(id);
            if (response.Status == ServiceResponse.ServiceStatus.Error || response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return View("Error", new ErrorViewModel { Errors = response.Messages });
            }
            TempData["SuccessMessage"] = "Book deleted successfully!";
            return RedirectToAction("List");
        }

        // POST: Books/LinkAuthorToBook/{bookId}/{authorId}
        // [Authorize]
        [HttpPost("LinkAuthorToBook")]
        public async Task<IActionResult> LinkAuthorToBook([FromForm] int bookId, [FromForm] int authorId)

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

            return RedirectToAction("Find", new { id = bookId });
        }

        // POST: Books/UnlinkAuthorFromBook/{bookId}/{authorId}
        // [Authorize]
        [HttpPost("UnlinkAuthorFromBook")]
        public async Task<IActionResult> UnlinkAuthorFromBook([FromForm] int bookId, [FromForm] int authorId)

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

            return RedirectToAction("Find", new { id = bookId });
        }
    }

}
