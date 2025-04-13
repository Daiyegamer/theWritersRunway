namespace AdilBooks.Interfaces
{
    
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string destinationPath);
    }

}
