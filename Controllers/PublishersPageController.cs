using AdilBooks.Interfaces;
using AdilBooks.Models;
using AdilBooks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdilBooks.Data;
using Microsoft.AspNetCore.Authorization;
using AdilBooks.Models.DTOs;

namespace AdilBooks.Controllers
{
    [Route("Publishers")]
    
    public class PublishersPageController : Controller
    {
        private readonly IPublisherService _publisherService;
        private readonly IBookService _bookService;
        private readonly ApplicationDbContext _context;

        public PublishersPageController(IPublisherService publisherService, IBookService bookService, ApplicationDbContext context)
        {
            _publisherService = publisherService;
            _bookService = bookService;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: Publishers/List
        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            IEnumerable<PublisherDto> publishers = await _publisherService.ListPublishers();
            return View(publishers);  // Looks for Views/Publishers/List.cshtml
        }

        [HttpGet("Find/{id}")]
        public async Task<IActionResult> Find(int id)
        {
            // Fetch the publisher using PublisherDto
            var publisher = await _publisherService.FindPublisher(id);
            if (publisher == null)
            {
                return View("Error", new ErrorViewModel { Errors = new List<string> { "Publisher not found." } });
            }

            // Fetch books associated with the publisher using the new method
            var publisherBooks = await _bookService.GetBooksByPublisher(id);

            // Pass both the publisher and books to ViewData
            ViewData["Publisher"] = publisher;
            ViewData["Books"] = publisherBooks;

            return View();  // Renders the default Find.cshtml view
        }



        // GET: Publishers/Add

        [HttpGet("Add")]
        public IActionResult Add()
        {
            return View(); // Render the "Add" form (Views/Publishers/Add.cshtml)
        }

        // POST: Publishers/Add
        [Authorize]
        [HttpPost("Add")]
        public async Task<IActionResult> Add(PublisherDto publisherDto)
        {
            if (!ModelState.IsValid)
            {
                return View(publisherDto);  // If the model is invalid, return the same model to the view
            }

            // Make sure the publisherDto is not null
            if (publisherDto == null)
            {
                return View("Error", new ErrorViewModel { Errors = new List<string> { "Invalid data submitted." } });
            }

            // Call the service to add the publisher
            ServiceResponse response = await _publisherService.AddPublisher(publisherDto);
            if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return View("Error", new ErrorViewModel { Errors = response.Messages });
            }

            TempData["SuccessMessage"] = "Publisher added successfully!";
            // Redirect to the details page after successful creation
            return RedirectToAction("Find", new { id = response.CreatedId });

            
        }


        // GET: Publishers/Edit/{id}
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var publisher = await _publisherService.FindPublisher(id);
            if (publisher == null)
            {
                return View("Error", new ErrorViewModel { Errors = new List<string> { "Publisher not found." } });
            }

            // ✅ Fetch all books from the database for selection
            var allBooks = await _context.Books.ToListAsync();

            // ✅ Pass available books to the view
            ViewBag.AllBooks = allBooks;

            return View(publisher);
        }


        // POST: Publishers/Update
        [Authorize]
        [HttpPost("Update")]
        public async Task<IActionResult> Update(PublisherDto publisherDto)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", publisherDto); // Return to the "Edit" view with the invalid model
            }

            // Call the service to update the publisher
            ServiceResponse response = await _publisherService.UpdatePublisher(publisherDto);
            if (response.Status == ServiceResponse.ServiceStatus.Error || response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return View("Error", new ErrorViewModel { Errors = response.Messages });
            }

            // Redirect to the details page after successful update
            TempData["SuccessMessage"] = "Publisher updated successfully!";
            return RedirectToAction("Find", new { id = publisherDto.PublisherId });
        }


        // GET: Publishers/ConfirmDelete/{id}
        [HttpGet("ConfirmDelete/{id}")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            PublisherDto publisher = await _publisherService.FindPublisher(id);
            if (publisher == null)
            {
                return View("Error", new ErrorViewModel { Errors = new List<string> { "Publisher not found." } });
            }
            return View(publisher); // Looks for Views/Publishers/ConfirmDelete.cshtml
        }

        // POST: Publishers/Delete/{id}
        [Authorize]
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _publisherService.DeletePublisher(id);
            if (response.Status == ServiceResponse.ServiceStatus.Error || response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return View("Error", new ErrorViewModel { Errors = response.Messages });
            }

            TempData["SuccessMessage"] = "Publisher deleted successfully!";
            return RedirectToAction("List");
        }

        [HttpGet("ManageShowsForPublisher/{id}")]
        public async Task<IActionResult> ManageShows(int id)
        {
            var shows = await _publisherService.GetShowsByPublisherAsync(id);
            var allShows = await _context.Shows.ToListAsync();
            var publisher = await _context.Publishers.FindAsync(id);

            ViewBag.PublisherId = id;
            ViewBag.AllShows = allShows;
            ViewBag.PublisherName = publisher?.PublisherName ?? "No Publisher Associated";

            return View(shows);
        }

        [Authorize]
        [HttpPost("LinkShow")]
        public async Task<IActionResult> LinkShow(int publisherId, int showId)
        {
            var result = await _publisherService.LinkShowAsync(publisherId, showId);
             TempData["Message"] = result ? "Show linked." : "Already linked.";
            return RedirectToAction("ManageShows", new { id = publisherId });
            //return Ok(new { message = result ? "Show linked." : "Already linked." }); // ✅ For Swagger testing
        }

        [Authorize]
        [HttpPost("UnlinkShow")]
        public async Task<IActionResult> UnlinkShow(int publisherId, int showId)
        {
            await _publisherService.UnlinkShowAsync(publisherId, showId);
            TempData["Message"] = "Show unlinked.";
             return RedirectToAction("ManageShows", new { id = publisherId });
            //return Ok(new { message = "Show unlinked." }); // ✅ For Swagger testing
        }

        [HttpGet("ShowPublishersForShow/{id}")]
        public async Task<IActionResult> ShowPublishers(int id)
        {
            var publishers = await _publisherService.GetPublishersByShowAsync(id);
            ViewBag.ShowId = id;
            return View(publishers);
            //return Ok(publishers); // ✅ For Swagger testing
        }


    }
}
