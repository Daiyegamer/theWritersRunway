
using Microsoft.AspNetCore.Mvc;
using AdilBooks.Data;
using AdilBooks.Models;

namespace AdilBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignersApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DesignersApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetDesigners()
        {
            return Ok(_context.Designers.ToList());
        }
    }
}
