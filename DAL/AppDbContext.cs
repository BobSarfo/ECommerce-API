using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HubtelCommerce.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}