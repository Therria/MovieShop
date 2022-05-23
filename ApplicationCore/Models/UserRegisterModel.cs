using ApplicationCore.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class UserRegisterModel
    {
        [Required(ErrorMessage = "Email cannot be empty")]
        [EmailAddress(ErrorMessage = "Email address should be in right format")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password cannot be empty")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*()=+]).{8,}$", ErrorMessage = 
            "Password should have minimum 8 characters with at least onr upper, lower, number and specail character")]
        public string Password { get; set; }

        [Required(ErrorMessage = "First Name cannot be empty")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name cannot be empty")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [YearValidation(1900, 2020)]
        public DateTime DateOfBirth { get; set; }
    }
}
