namespace SmartKazanlak.Core.Models.EventRequest
{
    public class EventRequestListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime StartDateTime { get; set; }
        public string Location { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
