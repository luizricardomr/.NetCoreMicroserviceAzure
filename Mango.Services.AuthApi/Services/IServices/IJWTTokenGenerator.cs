using Mango.Services.AuthApi.Models;

namespace Mango.Services.AuthApi.Services.IServices
{
    public interface IJWTTokenGenerator
    {
        string GenerateToken(ApplicationUser user, IEnumerable<string> roles);
    }
}
