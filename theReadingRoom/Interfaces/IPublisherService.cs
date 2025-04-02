using AdilBooks.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdilBooks.Interfaces
{
    public interface IPublisherService
    {
        Task<IEnumerable<PublisherDto>> ListPublishers();

        Task<PublisherDto?> FindPublisher(int id);

        Task<ServiceResponse> AddPublisher(PublisherDto publisherDto);

        Task<ServiceResponse> UpdatePublisher(PublisherDto publisherDto);

        Task<ServiceResponse> DeletePublisher(int id);
        Task<List<Show>> GetShowsByPublisherAsync(int publisherId);
        Task<bool> LinkShowAsync(int publisherId, int showId);
        Task<bool> UnlinkShowAsync(int publisherId, int showId);
        Task<List<Publisher>> GetPublishersByShowAsync(int showId);

    }
}
