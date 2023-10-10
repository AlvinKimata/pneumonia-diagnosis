using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace school_project.Models
{
    public class SingleImageDiagnosis{
        public int id {get; set;}

        [Required]
        public string Name {get; set;}

        [Required]
        public string Photos {get; set;}

        //The label of the image after model inference.
        public string ImageResult {get; set;}
    }
}