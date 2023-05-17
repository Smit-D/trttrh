using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstTask.Entities.ViewModels
{
    public class LoginVM
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[a-zA-Z0-9_\.-]+@([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,3}$", ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password, ErrorMessage = "Enter Valid Password")]
        public string Password { get; set; } = string.Empty;
    }
}
