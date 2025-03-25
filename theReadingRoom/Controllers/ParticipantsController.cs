
using Microsoft.AspNetCore.Mvc;
using AdilBooks.Data;
using AdilBooks.Models;
using Microsoft.EntityFrameworkCore;

namespace AdilBooks.Controllers
{
    public class ParticipantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParticipantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var participants = await _context.Participants.ToListAsync();
            return View(participants);
        }
    }
}
