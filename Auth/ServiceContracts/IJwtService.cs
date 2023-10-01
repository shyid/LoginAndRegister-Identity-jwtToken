using Auth.DTO;
using Auth.Identity;
using System.Security.Claims;

namespace Auth.ServiceContracts
{
     public interface IJwtService
     {
        AuthenticationResponse CreateJwtToken(ApplicationUser user);
        ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);
     }
}
