using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace school_project.Models
{

    public class ImageRes
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string imageresult { get; set; }

        public int ImageResultStatus {get; set;}
    }

}