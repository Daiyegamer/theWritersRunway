using AdilBooks.Models;
using AdilBooks.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AdilBooks.Interfaces;
using AdilBooks.Models.DTOs;

namespace AdilBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherService _publisherService;

        public PublishersController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        [HttpGet("List")]
        public async Task<IActionResult> ListPublishers()
        {
            var publishers = await _publisherService.ListPublishers();
            return Ok(new { message = "Publishers retrieved successfully.", data = publishers });
        }

        [HttpGet("Find/{id}")]
        public async Task<IActionResult> FindPublisher(int id)
        {
            var publisher = await _publisherService.FindPublisher(id);
            if (publisher == null)
            {
                return NotFound(new { error = "NotFound", message = "Publisher not found." });
            }
            return Ok(new { message = "Publisher retrieved successfully.", data = publisher });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdatePublisher(PublisherDto publisherDto)
        {
            var response = await _publisherService.UpdatePublisher(publisherDto);
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(new { error = "NotFound", message = response.Messages });
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, new { error = "InternalServerError", message = response.Messages });
            }

            return NoContent();
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddPublisher(PublisherDto publisherDto)
        {
            var response = await _publisherService.AddPublisher(publisherDto);
            if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, new { error = "InternalServerError", message = response.Messages });
            }
            return CreatedAtAction(nameof(FindPublisher), new { id = response.CreatedId }, new { message = "Publisher added successfully.", data = publisherDto });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            var response = await _publisherService.DeletePublisher(id);
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(new { error = "NotFound", message = response.Messages });
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, new { error = "InternalServerError", message = response.Messages });
            }

            return NoContent();
        }
    }
}
