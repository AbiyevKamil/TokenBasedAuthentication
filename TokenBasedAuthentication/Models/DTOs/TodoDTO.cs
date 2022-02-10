using System.ComponentModel.DataAnnotations;
using TokenBasedAuthentication.Data.Entity;

namespace TokenBasedAuthentication.Models;

public class TodoDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public UserDTO User { get; set; }
}