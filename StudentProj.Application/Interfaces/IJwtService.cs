using StudentProj.Domain.Entities;
using System.Security.Claims;

namespace StudentProj.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(Student student, List<string> roles);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetClaimsPrincipalFromExpiredToken(string? token);
    }
}
