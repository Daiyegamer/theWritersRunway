
using Microsoft.AspNetCore.Mvc;
using AdilBooks.Data;
using AdilBooks.Models;
using Microsoft.EntityFrameworkCore;

namespace AdilBooks.Controllers
{
    public class DesignersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DesignersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var designers = await _context.Designers.ToListAsync();
            return View(designers);
        }
    }
}
