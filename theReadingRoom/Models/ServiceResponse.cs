namespace AdilBooks.Models
{
    public class ServiceResponse
    {
        public enum ServiceStatus
        {
            Success,
            Error,
            Created,
            Updated,
            Deleted,
            NotFound
        }

        public ServiceStatus Status { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
        public int CreatedId { get; set; } // For when a new record is created
        public object Data { get; set; } // Holds the data returned by the service (optional)

        // ✅ Default Constructor
        public ServiceResponse() { }

        // ✅ Constructor for Status Only
        public ServiceResponse(ServiceStatus status)
        {
            Status = status;
        }

        // ✅ Constructor for Status & Messages
        public ServiceResponse(ServiceStatus status, List<string> messages)
        {
            Status = status;
            Messages = messages ?? new List<string>();
        }

        // ✅ Constructor for Status, Messages & Data
        public ServiceResponse(ServiceStatus status, List<string> messages, object data)
        {
            Status = status;
            Messages = messages ?? new List<string>();
            Data = data;
        }

        // ✅ Constructor for Created Responses
        public ServiceResponse(ServiceStatus status, int createdId)
        {
            Status = status;
            CreatedId = createdId;
        }
    }
}
