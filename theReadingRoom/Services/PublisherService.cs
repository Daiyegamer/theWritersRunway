using AdilBooks.Data;
using AdilBooks.Interfaces;
using AdilBooks.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdilBooks.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PublisherService> _logger;

        public PublisherService(ApplicationDbContext context, ILogger<PublisherService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<PublisherDto>> ListPublishers()
        {
            var publishers = await _context.Publishers
                .Select(p => new PublisherDto
                {
                    PublisherId = p.PublisherId,
                    PublisherName = p.PublisherName
                })
                .ToListAsync();

            return publishers;
        }

        public async Task<PublisherDto?> FindPublisher(int id)
        {
            var publisher = await _context.Publishers
                .Include(p => p.Books) // ✅ Load related books
                .FirstOrDefaultAsync(p => p.PublisherId == id);

            if (publisher == null)
            {
                return null;
            }

            return new PublisherDto
            {
                PublisherId = publisher.PublisherId,
                PublisherName = publisher.PublisherName,
                Books = publisher.Books.Select(b => b.Title).ToList() // ✅ Extract book titles
            };
        }


        public async Task<ServiceResponse> AddPublisher(PublisherDto publisherDto)
        {
            ServiceResponse serviceResponse = new();

            try
            {
                var publisher = new Publisher
                {
                    PublisherName = publisherDto.PublisherName
                };

                _context.Publishers.Add(publisher);
                await _context.SaveChangesAsync();  // Ensure that the publisher is saved to the database

                serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
                serviceResponse.CreatedId = publisher.PublisherId;  // Return the ID of the newly created publisher
                serviceResponse.Messages.Add("Publisher added successfully.");
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add($"An error occurred while adding the publisher: {ex.Message}");
                return serviceResponse;
            }
        }


        public async Task<ServiceResponse> UpdatePublisher(PublisherDto publisherDto)
        {
            ServiceResponse serviceResponse = new();

            try
            {
                var publisher = await _context.Publishers
                    .Include(p => p.Books) // ✅ Load related books
                    .FirstOrDefaultAsync(p => p.PublisherId == publisherDto.PublisherId);

                if (publisher == null)
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                    serviceResponse.Messages.Add("Publisher not found.");
                    return serviceResponse;
                }

                // ✅ Update the publisher name
                publisher.PublisherName = publisherDto.PublisherName;

                // ✅ Update books if provided
                if (publisherDto.Books.Any())
                {
                    // 🔥 Find books by title
                    var booksToUpdate = await _context.Books
                        .Where(b => publisherDto.Books.Contains(b.Title))
                        .ToListAsync();

                    foreach (var book in booksToUpdate)
                    {
                        book.PublisherId = publisher.PublisherId; // ✅ Assign books to new publisher
                    }
                }

                _context.Entry(publisher).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
                serviceResponse.Messages.Add("Publisher updated successfully, books reassigned.");
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                return serviceResponse;
            }
        }
        public async Task<List<Show>> GetShowsByPublisherAsync(int publisherId)
        {
            return await _context.PublisherShows
                .Where(ps => ps.PublisherId == publisherId)
                .Select(ps => ps.Show)
                .ToListAsync();
        }

        public async Task<bool> LinkShowAsync(int publisherId, int showId)
        {
            var exists = await _context.PublisherShows
                .AnyAsync(ps => ps.PublisherId == publisherId && ps.ShowId == showId);

            if (exists) return false;

            _context.PublisherShows.Add(new PublisherShow
            {
                PublisherId = publisherId,
                ShowId = showId
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnlinkShowAsync(int publisherId, int showId)
        {
            var entry = await _context.PublisherShows
                .FirstOrDefaultAsync(ps => ps.PublisherId == publisherId && ps.ShowId == showId);

            if (entry == null) return false;

            _context.PublisherShows.Remove(entry);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Publisher>> GetPublishersByShowAsync(int showId)
        {
            return await _context.PublisherShows
                .Where(ps => ps.ShowId == showId)
                .Select(ps => ps.Publisher)
                .ToListAsync();
        }


        public async Task<ServiceResponse> DeletePublisher(int id)
        {
            ServiceResponse serviceResponse = new();

            try
            {
                var publisher = await _context.Publishers.Include(p => p.Books).FirstOrDefaultAsync(p => p.PublisherId == id);
                if (publisher == null)
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                    serviceResponse.Messages.Add("Publisher not found.");
                    return serviceResponse;
                }

                // Check if the publisher has any associated books
                if (publisher.Books.Any())
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                    serviceResponse.Messages.Add("Publisher cannot be deleted because it has linked books.");
                    return serviceResponse;
                }

                _context.Publishers.Remove(publisher);
                await _context.SaveChangesAsync();

                serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
                serviceResponse.Messages.Add("Publisher deleted successfully.");
                return serviceResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting publisher.");
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add($"An error occurred: {ex.Message}");
                return serviceResponse;
            }
        }
    }
}
