using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdilBooks.Data;
using AdilBooks.Models;
using AdilBooks.Models.DTOs;
using System.Linq;
using System.Threading.Tasks;
using AdilBooks.Models.ViewModels;

namespace AdilBooks.Controllers
{
    [Route("designers")]
    public class DesignersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DesignersController(ApplicationDbContext context)
        {
            _context = context;
        }



        /// <summary>
        /// Displays the list of designers (Admin Only).
        /// </summary>
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var designers = await _context.Designers
                .Include(d => d.DesignerShows).ThenInclude(ds => ds.Show)
                .Include(d => d.DesignerBooks).ThenInclude(db => db.Book)
                .ToListAsync();

            return View("Index", designers);
        }



        /// <summary>
        /// Displays the form to create a new designer (Admin Only).
        /// </summary>
        /// 
        [HttpGet("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Shows = await _context.Shows
                .Where(s => s.EndTime > DateTime.UtcNow)
                .OrderBy(s => s.StartTime)
                .ToListAsync();

            ViewBag.Books = await _context.Books.ToListAsync();

            return View();
        }



        /// <summary>
        /// Handles form submission for creating a new designer (Admin Only).
        /// </summary>
        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] DesignerCreateDTO designerDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Shows = await _context.Shows.ToListAsync();
                return View("Create");
            }

            var designer = new Designer
            {
                Name = designerDto.Name,
                Category = designerDto.Category
            };

            _context.Designers.Add(designer);
            await _context.SaveChangesAsync();

            if (designerDto.SelectedShowIds != null && designerDto.SelectedShowIds.Any())
            {
                foreach (var showId in designerDto.SelectedShowIds)
                {
                    _context.DesignerShows.Add(new DesignerShow
                    {
                        DesignerId = designer.DesignerId,
                        ShowId = showId
                    });
                }
                await _context.SaveChangesAsync();
            }

            if (designerDto.SelectedBookIds != null && designerDto.SelectedBookIds.Any())
            {
                foreach (var bookId in designerDto.SelectedBookIds)
                {
                    _context.Add(new DesignerBook
                    {
                        DesignerId = designer.DesignerId,
                        BookId = bookId
                    });
                }
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "Designer added successfully!";
            return RedirectToAction(nameof(Index));
        }



        /// <summary>
        /// Displays the edit form for a designer (Admin Only).
        /// </summary>
        /// 
        [HttpGet("edit/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var designer = await _context.Designers
                .Include(d => d.DesignerShows)
                .ThenInclude(ds => ds.Show)
                .FirstOrDefaultAsync(d => d.DesignerId == id);

            if (designer == null) return NotFound();

            var designerDto = new DesignerUpdateDTO
            {
                DesignerId = designer.DesignerId,
                Name = designer.Name,
                Category = designer.Category,
                SelectedShowIds = designer.DesignerShows.Select(ds => ds.ShowId).ToList()
            };

            ViewBag.ShowList = new MultiSelectList(_context.Shows, "ShowId", "ShowName", designerDto.SelectedShowIds);

            ViewBag.BookList = new MultiSelectList(_context.Books, "BookId", "Title", designerDto.SelectedBookIds);

            return View(designerDto); 
        }



        /// <summary>
        /// Updates a designer's details (Admin Only).
        /// </summary>
        [HttpPost("edit/{id}")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] DesignerUpdateDTO designerDto)
        {
            if (id != designerDto.DesignerId) return BadRequest();

            var designer = await _context.Designers
                .Include(d => d.DesignerShows)
                .FirstOrDefaultAsync(d => d.DesignerId == id);

            if (designer == null) return NotFound();

            designer.Name = designerDto.Name;
            designer.Category = designerDto.Category;
            _context.DesignerShows.RemoveRange(designer.DesignerShows);

            foreach (var showId in (designerDto.SelectedShowIds ?? new List<int>()))
            {
                _context.DesignerShows.Add(new DesignerShow
                {
                    DesignerId = designer.DesignerId,
                    ShowId = showId
                });
            }

            // Remove & reassign books 
            var existingBookLinks = await _context.DesignerBooks
                .Where(db => db.DesignerId == id)
                .ToListAsync();

            _context.DesignerBooks.RemoveRange(existingBookLinks);

            if (designerDto.SelectedBookIds != null && designerDto.SelectedBookIds.Any())
            {
                foreach (var bookId in designerDto.SelectedBookIds)
                {
                    _context.DesignerBooks.Add(new DesignerBook
                    {
                        DesignerId = designer.DesignerId,
                        BookId = bookId
                    });
                }
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Designer updated successfully!";
            return RedirectToAction(nameof(Index));
        }



        /// <summary>
        /// Displays the delete confirmation page for a designer (Admin Only).
        /// </summary>
        [HttpGet("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var designer = await _context.Designers
                .Include(d => d.DesignerShows)
                .ThenInclude(ds => ds.Show)
                .FirstOrDefaultAsync(d => d.DesignerId == id);

            if (designer == null) return NotFound();

            return View(designer);
        }



        /// <summary>
        /// Handles the form submission to delete a designer (Admin Only).
        /// </summary>
        [HttpPost("delete/{id}")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var designer = await _context.Designers
                .Include(d => d.DesignerShows)
                .FirstOrDefaultAsync(d => d.DesignerId == id);

            if (designer == null)
                return NotFound();

            _context.DesignerShows.RemoveRange(designer.DesignerShows);
            _context.Designers.Remove(designer);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Designer deleted successfully!";
            return RedirectToAction(nameof(Index));
        }



        /// <summary>
        /// Displays the details of a designer (Admin Only).
        /// </summary>
        [HttpGet("details/{id}")]

        public async Task<IActionResult> Details(int id)
        {
            var designer = await _context.Designers
                .Include(d => d.DesignerShows).ThenInclude(ds => ds.Show)
                .Include(d => d.DesignerBooks).ThenInclude(db => db.Book)
                .FirstOrDefaultAsync(d => d.DesignerId == id);

            if (designer == null) return NotFound();

            var viewModel = new DesignerDetailsViewModel
            {
                Designer = designer,
                Books = designer.DesignerBooks.Select(db => db.Book).ToList(),
                ShowRemoveButton = User.Identity.IsAuthenticated
                // ShowRemoveButton = true

            };

            return View(viewModel);
        }


        [HttpPost("remove-book")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveBook(int designerId, int bookId)
        {
            var entry = await _context.Set<DesignerBook>()
                .FirstOrDefaultAsync(db => db.DesignerId == designerId && db.BookId == bookId);

            if (entry != null)
            {
                _context.Set<DesignerBook>().Remove(entry);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = designerId });
        }
    }
}