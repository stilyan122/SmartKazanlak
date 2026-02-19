using SmartKazanlak.Core.Enums;

namespace SmartKazanlak.Infrastructure.Entities
{
    public class MediaFile
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid EventRequestId { get; set; }
        public EventRequest EventRequest { get; set; } = null!;

        public MediaType Type { get; set; }

        public string OriginalFileName { get; set; } = null!;
        public string StoredFileName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public long Size { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
