using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Dto
{
    public class SignupLoginDto
    {
            [Required(ErrorMessage = "name can not be empty")]
            [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
            public required string Name { get; set; }
            [Required(ErrorMessage = "password can not be empty")]
            public required string Password { get; set; }
    }
}