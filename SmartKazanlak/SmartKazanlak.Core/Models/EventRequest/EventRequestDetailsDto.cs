namespace SmartKazanlak.Models.EventRequest
{
    public class EventRequestDetailsDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;
        public DateTime StartDateTime { get; set; }
        public string Location { get; set; } = null!;
        public string Description { get; set; } = null!;

        public string OrganizerName { get; set; } = null!;
        public string OrganizerEmail { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
