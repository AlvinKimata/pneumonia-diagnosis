using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;


namespace school_project.ViewModels
{
    public class UserClaimsViewModel
    {
        public UserClaimsViewModel()
        {
            Cliams = new List<UserClaims>();
        }

        public string UserId { get; set; }
        public List<UserClaims> Cliams { get; set; }
    }
}
