using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using school_project.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;


namespace school_project.Controllers;

public class DiagnosisController: Controller
{
    
    public ViewResult Single_Diagnosis()
    {
        return View();
    }

     public ViewResult Batch_Diagnosis()
    {
        return View();
    }



}