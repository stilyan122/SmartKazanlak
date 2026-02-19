using SmartKazanlak.Core.Enums;

namespace SmartKazanlak.Infrastructure.Entities
{
    public class EventRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; } = null!;
        public DateTime StartDateTime { get; set; }
        public string Location { get; set; } = null!;
        public string Description { get; set; } = null!;

        public string OrganizerName { get; set; } = null!;
        public string OrganizerEmail { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public EventStatus Status { get; set; } = EventStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? AdminReviewedAt { get; set; }
        public string? AdminReviewedByUserId { get; set; }
        public string? AdminNote { get; set; }

        public string UserId { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;

        public ICollection<MediaFile> MediaFiles { get; set; } = new List<MediaFile>();
        public ICollection<ModerationAction> ModerationActions { get; set; } = new List<ModerationAction>();
    }
}
