
using Microsoft.AspNetCore.Mvc;
using AdilBooks.Data;
using AdilBooks.Models;
using Microsoft.EntityFrameworkCore;

namespace AdilBooks.Controllers
{
    public class VotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var votes = await _context.Votes.ToListAsync();
            return View(votes);
        }
    }
}
