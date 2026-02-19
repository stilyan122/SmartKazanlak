using Microsoft.AspNetCore.Identity;

namespace SmartKazanlak.Core.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
