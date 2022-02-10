using Microsoft.AspNetCore.Identity;

namespace TokenBasedAuthentication.Data.Entity;

public class AppUser : IdentityUser
{
    public virtual List<Todo> Todos { get; set; } = new List<Todo>();
}