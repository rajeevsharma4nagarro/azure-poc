using SCD.Services.AuthAPI.Models;

namespace SCD.Services.AuthAPI.Service.IService
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser, List<string>? roles);
    }
}
