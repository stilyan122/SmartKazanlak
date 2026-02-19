namespace SmartKazanlak.Core.Models.EventRequest
{
    public class CreateEventRequestDto
    {
        public string Title { get; set; } = null!;
        public DateTime StartDateTime { get; set; }
        public string Location { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }
}
