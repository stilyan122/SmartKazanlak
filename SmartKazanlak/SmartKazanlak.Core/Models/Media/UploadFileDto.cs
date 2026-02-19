namespace SmartKazanlak.Core.Models.Media
{
    public class UploadFileDto
    {
        public string OriginalFileName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public long Size { get; set; }
        public Stream Content { get; set; } = null!;
    }
}
