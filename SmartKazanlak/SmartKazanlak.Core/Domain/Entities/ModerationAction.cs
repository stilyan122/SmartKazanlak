namespace SmartKazanlak.Core.Domain.Entities
{
    public class ModerationAction
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid EventRequestId { get; set; }
        public EventRequest EventRequest { get; set; } = null!;

        public string AdminUserId { get; set; } = null!;

        public string Action { get; set; } = null!; 
        public string? Note { get; set; }

        public DateTime ActionAt { get; set; } = DateTime.UtcNow;
    }
}
