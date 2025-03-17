using AdilBooks.Models;
using System.Collections.Generic;

namespace AdilBooks.Models.ViewModels
{
    public class BookWithAuthorsViewModel
    {
        public BookDto Book { get; set; }
        public List<AuthorListDto> AvailableAuthors { get; set; }
        public List<AuthorListDto> LinkedAuthors { get; set; } // authors already linked to the book
    }
}