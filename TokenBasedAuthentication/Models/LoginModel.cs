using System.ComponentModel.DataAnnotations;

namespace TokenBasedAuthentication.Models;

public class LoginModel
{
    [Required, EmailAddress] public string Email { get; set; } = String.Empty;
    [Required] public string Password { get; set; } = String.Empty;
}