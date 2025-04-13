using AdilBooks.Data;
using AdilBooks.Interfaces;
using AdilBooks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;


namespace AdilBooks.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public AuthorService(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
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
                Titles = string.Join(", ", author.Books.Select(b => b.Title)),
                ImagePath = author.ImagePath,
                ExistingImagePath = author.ImagePath // only needed if replacing old image on update
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

            authorDto.Titles = string.IsNullOrWhiteSpace(authorDto.Titles) ? "None" : authorDto.Titles.Trim();

            string? imagePath = null;

            // ✅ Save image if provided
            if (authorDto.ImageFile != null)
            {
                var savePath = Path.Combine("wwwroot", "uploads", "authors");
                var savedFile = await _fileService.SaveFileAsync(authorDto.ImageFile, savePath);
                imagePath = "/" + Path.GetRelativePath("wwwroot", savedFile).Replace("\\", "/");
            }

            var newAuthor = new Author
            {
                Name = authorDto.Name.Trim(),
                Bio = authorDto.Bio?.Trim() ?? "Biography not provided",
                ImagePath = imagePath
            };

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

            author.Name = authorDto.Name;
            author.Bio = authorDto.Bio;

            // ✅ Delete the old image first
            if (authorDto.ImageFile != null && !string.IsNullOrEmpty(author.ImagePath))
            {
                var oldImagePath = Path.Combine("wwwroot", author.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // ✅ Save image if a new one is uploaded
            if (authorDto.ImageFile != null)
            {
                var savePath = Path.Combine("wwwroot", "uploads", "authors");
                var savedFile = await _fileService.SaveFileAsync(authorDto.ImageFile, savePath);
                author.ImagePath = "/" + Path.GetRelativePath("wwwroot", savedFile).Replace("\\", "/");
            }

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

                // ✅ Delete image file from disk if it exists
                if (!string.IsNullOrEmpty(author.ImagePath))
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", author.ImagePath.TrimStart('/'));

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                // ✅ Remove author from DB
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();

                serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
                serviceResponse.Messages.Add("Author deleted successfully, including their image.");
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
