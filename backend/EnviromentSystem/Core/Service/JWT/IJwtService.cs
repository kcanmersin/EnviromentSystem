using System.Security.Claims;

namespace Core.Service.JWT
{
    public interface IJwtService
    {
        string GenerateToken(string email, Guid userId, IEnumerable<string> roles);
        ClaimsPrincipal? ValidateToken(string token);
    }
}
