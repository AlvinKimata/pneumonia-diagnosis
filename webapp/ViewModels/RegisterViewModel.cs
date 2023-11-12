using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace school_project.ViewModels
{
    public class RegisterViewModel
    {   
        [Required]
        [Display(Name="Full Name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "password and confirmation passworddo not match.")]
        public string ConfirmPassword { get; set; }

        public string City { get; set; }
    }
}
