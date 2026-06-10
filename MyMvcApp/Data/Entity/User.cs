using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entity;

[Table("User")]
public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = "User"; // default role is "User", can be "Admin"
    public DateTime CreatedAt { get; set; }
    public ICollection<ToDoList>? ToDoLists { get; set; }
}
