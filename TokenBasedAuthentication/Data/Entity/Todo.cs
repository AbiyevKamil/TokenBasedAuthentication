using System.ComponentModel.DataAnnotations;

namespace TokenBasedAuthentication.Data.Entity;

public class Todo
{
    public int Id { get; set; }
    [Required] public string Title { get; set; } = string.Empty;
    [Required] public string Content { get; set; } = string.Empty;
    [Required] public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Foreign Keys
    [Required] public string AppUserId { get; set; } = string.Empty;
    public virtual AppUser AppUser { get; set; } = new AppUser();
}