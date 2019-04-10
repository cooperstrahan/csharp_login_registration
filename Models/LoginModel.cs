using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginReg.Models
{
    public class Login
    {
        [Required(ErrorMessage = "An Email is required")]
        [EmailAddress(ErrorMessage = "A Valid Email is required")]
        public string LogEmail { get; set; }
        [Required(ErrorMessage = "A Password is Required")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password Must be 8 characters or longer!")]
        public string LogPassword { get; set; }
    }
}