using System.ComponentModel.DataAnnotations;

namespace SmartKazanlak.Models.EventRequest
{
    public class CreateEventRequestInputModel
    {
        [Required, StringLength(120, MinimumLength = 3)]
        public string Title { get; set; } = null!;

        [Required]
        public DateTime StartDateTime { get; set; } = DateTime.Now.AddDays(1);

        [Required, StringLength(200, MinimumLength = 2)]
        public string Location { get; set; } = null!;

        [Required, StringLength(4000, MinimumLength = 10)]
        public string Description { get; set; } = null!;

        [Required, StringLength(30, MinimumLength = 5)]
        public string Phone { get; set; } = null!;
    }
}
