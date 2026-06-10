using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Dto
{
    public class ToDoListDto
    {
        [Required(ErrorMessage = "enter name")]
        [MaxLength(10, ErrorMessage ="cannot be more than 10 chars")]
        public required string Title {get; set;}
        [MaxLength(100, ErrorMessage ="cannot be more than 100 chars")]
        public string Content {get; set;} = string.Empty;
    }
}