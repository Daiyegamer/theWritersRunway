using AdilBooks.Interfaces;

namespace AdilBooks.Services
{
    public class FileService : IFileService
    {
        private readonly List<string> _allowedExtensions = new() { ".jpg", ".jpeg", ".png", ".gif" };
        private const long MaxFileSize = 50 * 1024 * 1024; // 5MB

        public async Task<string> SaveFileAsync(IFormFile file, string destinationPath)
        {
            // Validation: Existence
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty or missing.");

            // Validation: Size
            if (file.Length > MaxFileSize)
                throw new ArgumentException("File is too large. Max allowed size is 5MB.");

            // Validation: Extension
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                throw new ArgumentException("Invalid file type. Only JPG, PNG, and GIF are allowed.");

            // Directory Setup
            if (string.IsNullOrWhiteSpace(destinationPath))
                throw new ArgumentException("Destination path must be provided.");

            if (!Directory.Exists(destinationPath))
                Directory.CreateDirectory(destinationPath);

            // Unique File Naming
            var uniqueName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(destinationPath, uniqueName);

            // Save File
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fullPath;
        }
    }
}
