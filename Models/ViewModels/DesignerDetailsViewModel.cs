using System.Collections.Generic;
using AdilBooks.Models;

namespace AdilBooks.Models.ViewModels
{
    public class DesignerDetailsViewModel
    {
        public Designer Designer { get; set; }
        public List<Book> Books { get; set; } = new List<Book>();
        public bool ShowRemoveButton { get; set; }
    }
}
