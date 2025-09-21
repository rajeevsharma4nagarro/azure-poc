using Microsoft.AspNetCore.Identity;

namespace SCD.Services.AuthAPI.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FullName { get; set; } = "";
        public string Address { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string Zip { get; set; } = "";
    }
}
