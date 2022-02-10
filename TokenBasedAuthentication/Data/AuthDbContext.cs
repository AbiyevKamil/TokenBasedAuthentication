using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TokenBasedAuthentication.Data.Entity;

namespace TokenBasedAuthentication.Data;

public class AuthDbContext : IdentityDbContext<AppUser, IdentityRole, string>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    public DbSet<Todo> Todos { get; set; }
}