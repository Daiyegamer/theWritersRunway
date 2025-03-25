
using Microsoft.AspNetCore.Mvc;
using AdilBooks.Data;
using AdilBooks.Models;
using Microsoft.EntityFrameworkCore;

namespace AdilBooks.Controllers
{
    public class ShowsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShowsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var shows = await _context.Shows.ToListAsync();
            return View(shows);
        }
    }
}
