using AdilBooks.Models;

public class PublisherShow
{
    public int PublisherId { get; set; }
    public Publisher Publisher { get; set; }

    public int ShowId { get; set; }
    public Show Show { get; set; }
}
