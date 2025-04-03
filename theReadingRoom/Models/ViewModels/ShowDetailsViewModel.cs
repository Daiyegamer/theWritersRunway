using AdilBooks.Models;
using System.Collections.Generic;

namespace AdilBooks.ViewModels
{
    public class ShowDetailsViewModel
    {
        public Show Show { get; set; }
        public List<Publisher> LinkedPublishers { get; set; }
    }
}
