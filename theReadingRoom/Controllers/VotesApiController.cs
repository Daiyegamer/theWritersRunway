
using Microsoft.AspNetCore.Mvc;
using AdilBooks.Data;

namespace AdilBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VotesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetVotes()
        {
            return Ok(_context.Votes.ToList());
        }
    }
}
