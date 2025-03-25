
using Microsoft.AspNetCore.Mvc;
using AdilBooks.Data;

namespace AdilBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ParticipantsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetParticipants()
        {
            return Ok(_context.Participants.ToList());
        }
    }
}
