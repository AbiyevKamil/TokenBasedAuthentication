using System.ComponentModel.DataAnnotations;

namespace TokenBasedAuthentication.Models;

public class RegisterModel
{
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] public string Username { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}