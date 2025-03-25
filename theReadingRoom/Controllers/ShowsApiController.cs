
using Microsoft.AspNetCore.Mvc;
using AdilBooks.Data;

namespace AdilBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ShowsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("list")]
        public IActionResult GetShows()
        {
            return Ok(_context.Shows.ToList());
        }
    }
}
