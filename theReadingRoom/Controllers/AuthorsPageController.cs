using AdilBooks.Interfaces;
using AdilBooks.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AdilBooks.Services;
using AdilBooks.Data;

namespace AdilBooks.Controllers
{

    [Route("Authors")]
    public class AuthorsPageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorService _authorService;
        private readonly IBookService _bookService;

        public AuthorsPageController(ApplicationDbContext context, IAuthorService authorService, IBookService bookService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); // ✅ Ensure _context is initialized
            _authorService = authorService ?? throw new ArgumentNullException(nameof(authorService));
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        }

        // GET: Authors/List
        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            IEnumerable<AuthorListDto> authors = await _authorService.ListAuthors();
            return View(authors);  // Looks for Views/Authors/List.cshtml
        }
        [HttpGet("Find/{id}")]
        public async Task<IActionResult> Find(int id)
        {
            var author = await _authorService.FindAuthor(id); // Fetch the author with books
            if (author == null)
            {
                return View("Error", new ErrorViewModel { Errors = new List<string> { "Author not found." } });
            }

            return View(author); // Pass an AuthorDto object, not a list
        }




        // GET: Authors/Add
        [HttpGet("Add")]
        public IActionResult Add()
        {
            return View(new AuthorDto()); // ✅ Ensure a model is sent
        }
        // [Authorize]
        [HttpPost("Add")]
        [ValidateAntiForgeryToken] // ✅ Prevent CSRF Attacks
        public async Task<IActionResult> Add(AuthorDto authorDto)
        {
            Console.WriteLine($"🔹 Received Add Request: Name={authorDto.Name}, Bio={authorDto.Bio}, Titles={authorDto.Titles}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("🚨 ModelState is INVALID!");
                foreach (var key in ModelState.Keys)
                {
                    foreach (var error in ModelState[key].Errors)
                    {
                        Console.WriteLine($"❌ Model Error - {key}: {error.ErrorMessage}");
                    }
                }
                return View("Add", authorDto); // ✅ Return with validation errors
            }

            var response = await _authorService.AddAuthor(authorDto);
            Console.WriteLine($"🔹 Service Response: {response.Status}");

            if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                Console.WriteLine($"❌ Error Messages: {string.Join(", ", response.Messages)}");
                return View("Error", new ErrorViewModel { Errors = response.Messages });
            }

            Console.WriteLine("✅ Author added successfully!");
            TempData["SuccessMessage"] = "Author added successfully!";
            return RedirectToAction("List");
        }


        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var author = await _authorService.FindAuthor(id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }
        // [Authorize]
        [HttpPost("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(AuthorDto authorDto)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", authorDto);
            }

            var response = await _authorService.UpdateAuthor(authorDto); // ✅ Only pass authorDto

            if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return View("Error", new ErrorViewModel { Errors = response.Messages });
            }

            TempData["SuccessMessage"] = "Author updated successfully!";
            return RedirectToAction("List");
        }


        [HttpGet("ConfirmDelete/{id}")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var author = await _authorService.FindAuthor(id);
            if (author == null)
            {
                return NotFound(); // Show 404 page if author doesn't exist
            }
            return View(author); // Render ConfirmDelete.cshtml
        }




        // POST: Authors/Delete/{id}
        // [Authorize]
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _authorService.DeleteAuthor(id);
            if (response.Status == ServiceResponse.ServiceStatus.Error || response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return View("Error", new ErrorViewModel { Errors = response.Messages });
            }
            TempData["SuccessMessage"] = "Author deleted successfully!";
            return RedirectToAction("List");
        }
    }

}
