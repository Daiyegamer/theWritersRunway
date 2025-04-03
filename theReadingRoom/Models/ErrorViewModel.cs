using System.Collections.Generic;

namespace AdilBooks.Models // Or: WritersRunway.Models.Common if I decide to rename later
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        // Optional: used to show a list of validation or system errors
        public List<string> Errors { get; set; } = new List<string>();
    }
}
