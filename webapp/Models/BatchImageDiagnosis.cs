using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace school_project.Models
{
    public class BatchImageDiagnosis{

        //Project ID.
        public int Id {get; set;}

        [Required]
        public string Name {get; set;}

        [Required]
        public List<string> Photos {get; set;}

        public List<string> ImagesResults {get; set; }
    }
}


