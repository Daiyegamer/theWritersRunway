using AdilBooks.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AdilBooks.Models
{
    public class Publisher
    {
        [Key]
        public int PublisherId { get; set; }
        public string PublisherName { get; set; }
        //A publisher publishes many books
        
        public ICollection<Book> Books { get; set; }

        public ICollection<PublisherShow> PublisherShows { get; set; }
    }

    public class PublisherDto
    {
        public int PublisherId { get; set; }
        public string PublisherName { get; set; }
        //A publisher publishes many books
        public List<string> Books { get; set; } = new List<string>();
       // New: For listing linked shows
    public List<string> Shows { get; set; } = new List<string>();

        // Optional: If you need IDs to unlink
        public Dictionary<int, string> ShowMap { get; set; } = new Dictionary<int, string>();

    }
}
