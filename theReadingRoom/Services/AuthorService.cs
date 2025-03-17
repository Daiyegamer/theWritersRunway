using AdilBooks.Data;
using AdilBooks.Interfaces;
using AdilBooks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdilBooks.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDbContext _context;

        public AuthorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AuthorListDto>> ListAuthors()
        {
            var authors = await _context.Authors
                .Select(a => new AuthorListDto
                {
                    AuthorId = a.AuthorId,
                    Name = a.Name
                })
                .ToListAsync();

            return authors;
        }

        public async Task<AuthorDto?> FindAuthor(int id)
        {
            var author = await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.AuthorId == id);

            if (author == null)
            {
                return null;
            }

            return new AuthorDto
            {
                AuthorId = author.AuthorId,
                Name = author.Name,
                Bio = author.Bio,
                Titles = string.Join(", ", author.Books.Select(b => b.Title))
            };
        }

        public async Task<ServiceResponse> AddAuthor(AuthorDto authorDto)
        {
            if (string.IsNullOrWhiteSpace(authorDto.Name))
            {
                return new ServiceResponse
                {
                    Status = ServiceResponse.ServiceStatus.Error,
                    Messages = new List<string> { "Author name is required." }
                };
            }

            // ✅ Ensure Titles has a default value to avoid null errors
            authorDto.Titles = string.IsNullOrWhiteSpace(authorDto.Titles) ? "None" : authorDto.Titles.Trim();

            var newAuthor = new Author
            {
                Name = authorDto.Name.Trim(),
                Bio = authorDto.Bio?.Trim() ?? "Biography not provided",
            };

            // ✅ Add author to the database
            _context.Authors.Add(newAuthor);
            await _context.SaveChangesAsync();

            return new ServiceResponse
            {
                Status = ServiceResponse.ServiceStatus.Created,
                CreatedId = newAuthor.AuthorId,
                Messages = new List<string> { "Author added successfully." }
            };
        }


        public async Task<ServiceResponse> UpdateAuthor(AuthorDto authorDto)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.AuthorId == authorDto.AuthorId);

            if (author == null)
            {
                return new ServiceResponse
                {
                    Status = ServiceResponse.ServiceStatus.NotFound,
                    Messages = new List<string> { "Author not found." }
                };
            }

            // ✅ Update Author Details
            author.Name = authorDto.Name;
            author.Bio = authorDto.Bio;

            // ✅ Set a default "None" for Titles but do NOT update books
            authorDto.Titles = "None";

            await _context.SaveChangesAsync();

            return new ServiceResponse
            {
                Status = ServiceResponse.ServiceStatus.Updated,
                Messages = new List<string> { "Author updated successfully!" }
            };
        }



        public async Task<ServiceResponse> DeleteAuthor(int id)
        {
            ServiceResponse serviceResponse = new();

            try
            {
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                    serviceResponse.Messages.Add("Author not found.");
                    return serviceResponse;
                }

                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();

                serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
                serviceResponse.Messages.Add("Author deleted successfully.");
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add($"An error occurred while deleting the author: {ex.Message}");
                return serviceResponse;
            }
        }
    }
}
