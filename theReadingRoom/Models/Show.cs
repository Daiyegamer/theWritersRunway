using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdilBooks.Models
{
    public class Show
    {
        public int ShowId { get; set; }

        [Required(ErrorMessage = "Show name is required.")]
        public string ShowName { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        public DateTime EndTime { get; set; }


        // Initialize Collections to Prevent Validation Errors
        public ICollection<ParticipantShow> ParticipantShows { get; set; } = new List<ParticipantShow>();
        public ICollection<Participant> Participants { get; set; } = new List<Participant>();
        public ICollection<DesignerShow> DesignerShows { get; set; } = new List<DesignerShow>();
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}
