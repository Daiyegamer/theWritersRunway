

using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using AdilBooks.Data.Migrations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AdilBooks.Models
{
    public class Book
    {//We are going to define what a book is here

        [Key]
        public int BookId { get; set; }
        public string Title { get; set; }

        public int Year { get; set; }
        public string Synopsis { get; set; }

        [ForeignKey("Publishers")]
        public int PublisherId { get; set; }

        public virtual Publisher Publisher { get; set; }


        public virtual ICollection<Author>? Authors { get; set; }

        public ICollection<DesignerBook> DesignerBooks { get; set; } = new List<DesignerBook>();

    }
    public class BookDto
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public List<string> AuthorNames { get; set; }
        public List<ListAuthorDto> LinkedAuthors { get; set; } = new List<ListAuthorDto>();
        public string PublisherName { get; set; }
        public string Synopsis { get; set; }

    }
    public class BookListDto
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }

    }
    public class UpdateBookDto
    {   public int BookId { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }        
        public string Synopsis { get; set; }

    }
    public class AddBookDto
    {
        public int BookId { get; set; }  // Auto-generated during output
        public string Title { get; set; }
        public int Year { get; set; }
        public string Synopsis { get; set; }

        
        public string PublisherName { get; set; }
        public List<string> AuthorNames { get; set; } = new List<string>();
    }
        public class ListAuthorDto
        {
            public int AuthorId { get; set; }
            public string AuthorName { get; set; } 
        }
        

    }
