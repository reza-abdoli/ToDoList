using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Entity;

[Table("ToDoList")]
public class ToDoList : BaseEntity
{

    public string Title {get; set;} = string.Empty;
    public string Content {get; set;} = string.Empty;
    public bool IsDeleted {get; set;}
    public DateTime CreatedAt {get; set;}
    public DateTime UpdatedAt {get; set;}
    public int UserId {get;set;}
    public User User {get;set;} = null!;
}
