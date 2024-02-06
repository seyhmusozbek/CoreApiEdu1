using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiEdu1.Models
{
    public class LoginDTO
    {
        public string UserName { get; set; }
        [Required]
        [StringLength(8, ErrorMessage = "Şifreniz maximum 8 karakter olmalı")]
        public string Password { get; set; }
    }
    public class UserDTO
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
