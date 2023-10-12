using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace school_project.ViewModels
{
    public class BatchImageDiagnosisViewModel
    {
        public int id {get; set;}

        [Required]
        public string Name {get; set;}

        [Required]
        public List<string> Photos { get; set; }
    }
}