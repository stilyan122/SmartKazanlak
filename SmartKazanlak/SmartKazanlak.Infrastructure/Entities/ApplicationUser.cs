using Microsoft.AspNetCore.Identity;

namespace SmartKazanlak.Infrastructure.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
