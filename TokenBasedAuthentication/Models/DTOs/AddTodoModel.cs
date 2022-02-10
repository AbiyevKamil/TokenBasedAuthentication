using System.ComponentModel.DataAnnotations;

namespace TokenBasedAuthentication.Models;

public class AddTodoModel
{
    [Required] public string Title { get; set; } = string.Empty;
    [Required] public string Content { get; set; } = string.Empty;

    // Foreign Keys
    // [Required] public string AppUserId { get; set; }
}