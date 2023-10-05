using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace school_project.Models
{
    public class Employee
    {
        public int Id {get; set;}

        [Required]
        public string Name {get; set;}

        [Required]
        public string Email {get; set;}

        public Dept? Department {get; set;}

        public string PhotoPath {get; set;}

    }
}