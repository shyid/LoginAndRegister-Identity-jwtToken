using Auth.Identity;
using Auth.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.DatabaseContext
{
     public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
     {
          public ApplicationDbContext(DbContextOptions options) : base(options) 
          { 
          }

          public ApplicationDbContext()
          {
          }



         protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
               base.OnModelCreating(modelBuilder);

               }
     }
}
