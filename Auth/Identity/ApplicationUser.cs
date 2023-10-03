using Microsoft.AspNetCore.Identity;

namespace Auth.Identity
{
     public class ApplicationUser : IdentityUser<Guid>
     {
          //public Guid? Id { get; set; }
          public string? PersonName { get; set; }
          public string? RefreshToken { get; set; }
          public DateTime RefreshTokenExpirationDateTime { get; set; }
     }
}

