using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace school_project.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string City  { get; set; }
        public string Name {get; set;}
    }
}
