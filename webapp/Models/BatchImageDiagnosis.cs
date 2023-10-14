using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace school_project.Models
{
    public class BatchImageDiagnosis{

        public BatchImageDiagnosis()
        {
            Photos = new List<Photo>(); 
            ImagesResults = new List<ImageRes>();
        }

        //Project ID.
        [Key]
        public int Id {get; set;}

        [Required]
        public string Name {get; set;}
      
        [Required]
        public List<Photo> Photos {get; set;}

        public List<ImageRes> ImagesResults {get; set; }
    }
}
