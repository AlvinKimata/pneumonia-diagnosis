using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace school_project.Models
{
    public class BatchImageDiagnosis{

        //Project ID.
        public int Id {get; set;}

        [Required]
        public string Name {get; set;}

        [Required]
        public string PhotoPath {get; set;}

        //Labels of batch images after model inference.
        // public List<string> ImageResults {get; set;}
    }
}


