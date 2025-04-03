namespace AdilBooks.Models
{
  public class DesignerBook
  {
    public int DesignerId { get; set; }
    public Designer Designer { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; }
  }

}
