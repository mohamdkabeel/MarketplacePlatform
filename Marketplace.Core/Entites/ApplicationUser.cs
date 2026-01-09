using Microsoft.AspNetCore.Identity;

namespace Marketplace.Core.Entites
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsSeller { get; set; } = false;
    }
}
