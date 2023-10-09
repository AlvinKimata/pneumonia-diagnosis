using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using school_project.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;


namespace school_project.Controllers;

public class AnalysisController: Controller
{
    
    public ViewResult DataAnalysis()
    {
        return View();
    }

     public ViewResult ModelAnalysis()
    {
        return View();
    }



}