
using AdilBooks.Models;
using System.Collections.Generic;

namespace AdilBooks.Models.DTOs;



    public class DesignerDTO
    {
        public int DesignerId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public List<ShowDto> Shows { get; set; }
        public List<string> Books { get; set; }


        public DesignerDTO(Designer designer)
        {
            DesignerId = designer.DesignerId;
            Name = designer.Name;
            Category = designer.Category;
            Shows = designer.DesignerShows?.Select(ds => new ShowDto(ds.Show)).ToList();
             Books = designer.DesignerBooks?.Select(db => db.Book.Title).ToList() ?? new();
        }
    }

